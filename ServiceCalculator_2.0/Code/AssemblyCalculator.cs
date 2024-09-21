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

        public float Calculate(bool isKitchen, float goodsCost, float km)
        {
            if (Settings == null) return -1f;
            float percent = 0;
            if (isKitchen) percent = Settings.AssemblyKitchenPercent;
            else percent = Settings.AssemblyPercent;

            float result = goodsCost * percent + km * Settings.AssemblyKmPrice;
            result *= Settings.MarginPercent;
            // Проверка на минимальную стоимость
            if (result < Settings.AssemblyMinPrice) result = Settings.AssemblyMinPrice;

            return result;
        }
    }
}
