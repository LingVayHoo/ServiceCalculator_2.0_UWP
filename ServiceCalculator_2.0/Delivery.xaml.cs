using ServiceCalculator;
using ServiceCalculator_2._0.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WpfApp2;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace ServiceCalculator_2._0
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Delivery : Page
    {
        private DataSettings settings;
        private DeliveryCalculator deliveryCalculator;
        private DefaultPriceChecker priceChecker;
        private FloorAscent floorAscent;

        public Delivery()
        {
            this.InitializeComponent();
            Loaded += Delivery_Loaded; // Подписка на событие загрузки
        }

        private async void Delivery_Loaded(object sender, RoutedEventArgs e)
        {
            await Preparing(); // Запуск асинхронной операции при загрузке страницы
        }

        private async Task Preparing()
        {
            SaveToFile s = new SaveToFile("dataBase.txt");
            await s.Preparing();
            settings = await s.Read();
            deliveryCalculator = new DeliveryCalculator(settings);
            priceChecker = new DefaultPriceChecker(settings);
            floorAscent = new FloorAscent(settings);
            if (settings != null) FillAll();

        }

        private void CalculateDelivery()
        {
            ClearResults();
            var km = KmCountText.Text != string.Empty ? KmCountText.Text : 0.ToString();
            if (Int32.TryParse(km, out var result))
            {
                var res = deliveryCalculator.Calculate(result);
                var deliveryMultiplier = CheckWeight();
                if (deliveryMultiplier > 0) res *= deliveryMultiplier;
                else
                {
                    Warning("Неправильно введен вес!");
                    return;
                }

                res = priceChecker.Check(settings.IsSmallType, result, deliveryMultiplier) >= 0 ?
                    priceChecker.Check(settings.IsSmallType, result, deliveryMultiplier) : res;
                if (NeedAscent.IsOn)
                {
                    ResultTextAscent.Text = CalculateAscent().ToString();
                    if (CalculateAscent() >= 1000000) 
                        Warning("Не удалось посчитать подъем из-за размеров коробок! Нужен подсчет вручную!");
                }

                ResultText.Text = res.ToString();
            }
        }

        private void ClearResults()
        {
            ResultText.Text = string.Empty;
            ResultTextAscent.Text = string.Empty;
        }

        private float CheckWeight()
        {

            if (float.TryParse(WeightText.Text, out float result))
            {

                if (settings.MaxWeight > 0 && (result / settings.MaxWeight) > 1)
                {
                    var r = (float)Math.Floor(result / settings.MaxWeight) + 1;
                    return r;
                }
                else if (settings.MaxWeight > 0 && (result / settings.MaxWeight) <= 1)
                    return 1;
            }
            return -1;
        }

        private void Warning(string message)
        {
            AlertText.Text = message;
            Task.Delay(3000);
            AlertText.Text = string.Empty;
        }

        private float CalculateAscent()
        {
            if (string.IsNullOrEmpty(WeightText?.Text))
            {
                Warning("Введите вес!");
                return 0;
            }

            if (string.IsNullOrEmpty(FloorNumberText?.Text))
            {
                Warning("Введите номер этажа!");
                return 0;
            }

            if (!float.TryParse(WeightText.Text, out float weight) || weight <= 0)
            {
                Warning("Введите корректный вес!");
                return 0;
            }

            if (!float.TryParse(FloorNumberText.Text, out float floor))
            {
                Warning("Введите корректный номер этажа!");
                return 0;
            }

            if (floorAscent == null) return 0;

            float result = floorAscent.Calculate(
                ElevatorCheckBox?.IsOn ?? false,
                false,        //LargeCheckBox?.IsChecked ?? false,
                floor,
                weight);

            return result;
        }

        private void FillAll()
        {
            ProductTypeComboBox.ItemsSource = new string[]
            {
                "Мелкогабаритный",
                "Крупногабаритный"
            };
            if (settings.IsSmallType) ProductTypeComboBox.SelectedItem = "Мелкогабаритный";
            else ProductTypeComboBox.SelectedItem = "Крупногабаритный";
        }

        private void ProductTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (settings == null) return;
            if (ProductTypeComboBox.SelectedIndex == 0) settings.IsSmallType = true;
            else if (ProductTypeComboBox.SelectedIndex == 1) settings.IsSmallType = false;

            CalculateDelivery();
        }

        private void KmCountText_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateDelivery();
        }

        private void WeightText_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateDelivery();
        }

        private void NeedAscent_Toggled(object sender, RoutedEventArgs e)
        {
            CalculateDelivery();
        }

        private void ElevatorCheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            CalculateDelivery();
        }

        private void FloorNumberText_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateDelivery();
        }
    }


}
