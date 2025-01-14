using ServiceCalculator_2._0.Code;
using System;

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

        public float GetRemotePrice(float weight, float remoteInMeters)
        {
            // Вычисляем расстояние проноса для оплаты
            if (remoteInMeters == 0) return 0;

            float remoteIndex = (float)Math.Ceiling((remoteInMeters - 50) / 50);

            if (remoteIndex < 0) return 0;

            //Рассчитываем стоимость проноса, исходя из кг

            for (int i = 0; i < settings.WeightLimits.Length; i++)
            {
                if (i < 6 && weight <= settings.WeightLimits[i])
                {
                    return settings.FloorAscentPricesNoElevator[i] * remoteIndex * settings.MarginPercent;
                }
                if (i >= 6 && weight <= settings.WeightLimits[i])
                {
                    return RemotePriceCustom(weight, remoteIndex);
                }
            }
            return 0;
        }

        private float RemotePriceCustom(float weight, float floorNumber)
        {
            float result = -2;
            if (weight >= settings.WeightLimits[5])
            {
                //Устанавливаем цену за 500 кг
                result = settings.FloorAscentPricesNoElevator[5];
                // Рассчитываем стоимость за вес, который свыше 500 кг.
                float underWeightLimit = settings.FloorAscentPricesNoElevator[7] *
                    (float)Math.Ceiling((weight - settings.WeightLimits[5]) / 100);
                return (result + underWeightLimit) * floorNumber;
            }
            else return -1;
        }

    }
}
