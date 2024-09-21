using ServiceCalculator_2._0.Code;

namespace WpfApp2
{
    public class DefaultPriceChecker
    {
        private DataSettings _settings;

        public DataSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }
        public DefaultPriceChecker(DataSettings settings)
        {
            Settings = settings;
        }

        public float Check(bool isSmallType, float km, float deliveryMultiplier)
        {
            if (_settings == null) return -3;
            if (km > Settings.KmLimits[3]) return -2;
            float[] defaultPrices;
            if (isSmallType) defaultPrices = Settings.SmallDefaultPrices;
            else defaultPrices = Settings.LargeDefaultPrices;

            float res = -1;
            for (int i = 0; i < Settings.KmLimits.Length; i++)
            {
                if (km > 0)
                {
                    if (i == 0 && km <= Settings.KmLimits[i])
                    {
                        res = defaultPrices[i] * deliveryMultiplier;
                    }
                    if (i > 0 && km > Settings.KmLimits[i - 1] && km <= Settings.KmLimits[i])
                    {
                        res = defaultPrices[i] * deliveryMultiplier;
                    }
                }
            }
            return res;

        }

    }
}
