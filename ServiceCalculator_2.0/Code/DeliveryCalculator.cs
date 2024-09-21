﻿using ServiceCalculator_2._0.Code;

namespace WpfApp2
{
    internal class DeliveryCalculator
    {
        private DataSettings settings;

        public DeliveryCalculator(DataSettings settings)
        {
            this.settings = settings;
        }

        public float Calculate(float km)
        {
            var basePrice = settings.IsSmallType ? settings.SmalBasePrice : settings.LargeBasePrice;
            var result = (basePrice + (km * settings.OneKmPrice)) * settings.MarginPercent;
            return result;
        }

    }
}
