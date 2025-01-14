using ServiceCalculator_2._0.Code;
using WpfApp2;

namespace ServiceCalculator
{

    public class AssemblyCalculator
    {
        private DataSettings _settings;

        public DataSettings Settings { get => _settings; set => _settings = value; }

        public AssemblyCalculator(DataSettings settings)
        {
            Settings = settings;
        }

        public (float, float) Calculate(bool isKitchen, float goodsCost, float km)
        {
            if (Settings == null) return (-1f, -1f);
            float percent = 0;
            if (isKitchen) percent = Settings.AssemblyKitchenPercent;
            else percent = Settings.AssemblyPercent;

            float assemblyResult = goodsCost * percent;
            float remoteResult = km * Settings.AssemblyKmPrice;
            assemblyResult *= Settings.MarginPercent;
            remoteResult *= Settings.MarginPercent;
            // Проверка на минимальную стоимость
            if (assemblyResult < Settings.AssemblyMinPrice) assemblyResult = Settings.AssemblyMinPrice;

            return (assemblyResult, remoteResult);
        }
    }
}
