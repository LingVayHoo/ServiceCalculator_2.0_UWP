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
            if (await IsFileExistsAsync(_fileName))
            {
                DataSettings s = await Read();
                if (s != null && s.AppVersion < 2.1f)
                {
                    await Save(CreateDataSettings());
                }
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
                SmalBasePrice = 475f,
                LargeBasePrice = 1040f,
                IsSmallType = false,
                OneKmPrice = 53f,
                MarginPercent = 1.15f,
                MaxWeight = 3500f,
                SmallDefaultPrices = new float[] { 850, 1150, 1800, 2200 },
                LargeDefaultPrices = new float[] { 1500, 1800, 2250, 3000 },
                KmLimits = new float[] { 5, 10, 20, 30 },
                WeightLimits = new float[] { 25, 50, 100, 150, 300, 500, 10000 },
                FloorAscentPrices = new float[] { 240, 365, 550, 725, 1450, 2175, 3625 },
                FloorAscentPricesNoElevator = new float[] { 120, 185, 360, 550, 725, 910, 185 },
                AssemblyPercent = 0.10f,
                AssemblyKitchenPercent = 0.16f,
                AssemblyMinPrice = 4000f,
                AssemblyKmPrice = 50,
                AppVersion = 2.2f
            };

            return data;
        }
    }
}
