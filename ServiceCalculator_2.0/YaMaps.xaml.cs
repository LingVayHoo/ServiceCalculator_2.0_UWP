using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace ServiceCalculator_2._0
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class YaMaps : Page
    {
        public YaMaps()
        {
            this.InitializeComponent();

            YaMapsView.Navigate(new Uri("https://yandex.com/maps/org/enkel/178777208077/?ll=30.315644%2C59.989589&z=15.74"));
            YaMapsView.NavigationCompleted += WebView_NavigationCompleted;
        }

        private async void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            string script = "document.body.style.zoom = '60%';"; // Установи желаемый масштаб
            await YaMapsView.InvokeScriptAsync("eval", new string[] { script });
        }
    }
}
