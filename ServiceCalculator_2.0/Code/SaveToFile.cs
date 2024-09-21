using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace ServiceCalculator_2._0.Code
{
    public class SaveToFile
    {
        private readonly string _fileName;

        public SaveToFile(string fileName)
        {
            _fileName = fileName;
            //Preparing().Wait(); // Избегайте использования Wait в асинхронных сценариях, это для упрощения
        }

        public async Task Preparing()
        {
            if (!await IsFileExistsAsync(_fileName))
            {
                await Save(CreateDataSettings());
            }
        }

        public async Task Save(DataSettings data)
        {
            string json = JsonConvert.SerializeObject(data);

            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(_fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json); // Сохранение данных в файл
        }

        public async Task<DataSettings> Read()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.GetFileAsync(_fileName);
            string json = await FileIO.ReadTextAsync(file);
            DataSettings data = JsonConvert.DeserializeObject<DataSettings>(json);
            return data;
        }

        private async Task<bool> IsFileExistsAsync(string fileName)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.GetFileAsync(fileName); // Попытка получить файл
                return file != null;
            }
            catch (FileNotFoundException)
            {
                return false; // Файл не найден, возвращаем false
            }
            catch (Exception ex)
            {
                // Логирование исключения для отладки, если нужно
                System.Diagnostics.Debug.WriteLine($"Ошибка при проверке файла: {ex.Message}");
                return false;
            }
        }



        private DataSettings CreateDataSettings()
        {
            DataSettings data = new DataSettings
            {
                SmalBasePrice = 450f,
                LargeBasePrice = 990f,
                IsSmallType = true,
                OneKmPrice = 35f,
                MarginPercent = 1.12f,
                MaxWeight = 350f,
                SmallDefaultPrices = new float[] { 700, 900, 1300, 1700 },
                LargeDefaultPrices = new float[] { 1300, 1450, 1850, 2250 },
                KmLimits = new float[] { 5, 10, 20, 30 },
                WeightLimits = new float[] { 25, 50, 100, 150, 300, 500, 10000 },
                FloorAscentPrices = new float[] { 150, 300, 450, 600, 1200, 1800, 3000 },
                FloorAscentPricesNoElevator = new float[] { 75, 150, 300, 450, 600, 750, 150, 250 },
                AssemblyPercent = 0.12f,
                AssemblyKitchenPercent = 0.16f,
                AssemblyMinPrice = 4000f,
                AssemblyKmPrice = 50
            };

            return data;
        }
    }
}
