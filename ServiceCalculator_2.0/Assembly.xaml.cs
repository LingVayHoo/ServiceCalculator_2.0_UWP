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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace ServiceCalculator_2._0
{
    

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Assembly : Page
    {
        private AssemblyCalculator assemblyCalculator;
        private DataSettings settings;

        public Assembly()
        {
            this.InitializeComponent();
            Loaded += AssemblyLoaded;
            
        }

        private async void AssemblyLoaded(object sender, RoutedEventArgs e)
        {
            await Preparing(); // Запуск асинхронной операции при загрузке страницы
        }

        private async Task Preparing()
        {
            SaveToFile s = new SaveToFile("dataBase.txt");
            await s.Preparing();
            settings = await s.Read();
            assemblyCalculator = new AssemblyCalculator(settings);
        }

        private void CalculateAssembly()
        {
            if (assemblyCalculator == null) return;

            if (!float.TryParse(GoodsCostText?.Text, out float goodsCost))
            {
                Warning("Введите корректную стоимость!");
                goodsCost = 0;
            }

            if (!float.TryParse(DistanceText_Assembly?.Text, out float distance))
            {
                Warning("Введите корректное расстояние!");
                distance = 0;
            }

            bool isKitchenChecked = false;

            (float, float) result = assemblyCalculator.Calculate(isKitchenChecked, goodsCost, distance);

            ResultText_Assembly.Text = Math.Floor(result.Item1).ToString();
            ResultText_Remote.Text = Math.Floor(result.Item2).ToString();
        }

        private void Warning(string message)
        {
            AlertText.Text = message;
            Task.Delay(3000);
            AlertText.Text = string.Empty;
        }

        private void GoodsCostText_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAssembly();
        }

        private void DistanceText_Assembly_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAssembly();
        }
    }
}
