using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace ServiceCalculator_2._0
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ChangeWindowSize();
            ContentFrame.Navigate(typeof(DefaultPage));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (sender as ListBox).SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                var content = selectedItem.Content.ToString();
                if (content == "Доставка")
                {
                    ContentFrame.Navigate(typeof(Delivery));
                }
                else if (content == "Сборка")
                {
                    ContentFrame.Navigate(typeof(Assembly));
                }
                else if (content == "Карты")
                {
                    ContentFrame.Navigate(typeof(YaMaps));
                }
            }
            else 
            { 
                ContentFrame.Navigate(typeof(DefaultPage));
            }

        }

        private void ChangeWindowSize()
        {
            // Установи размеры окна
            var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            view.TryResizeView(new Size(900, 550)); // Установить размеры окна (ширина и высота)
        }
    }
}
