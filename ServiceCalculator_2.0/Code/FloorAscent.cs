using ServiceCalculator_2._0.Code;
using System;

namespace WpfApp2
{
    internal class FloorAscent
    {
        private float margin;
        private float[] _weightLimits;
        private float[] _floorAscentPrices;
        private float[] _floorAscentPricesNoElevator;

        public float[] WeightLimits
        {
            get { return _weightLimits; }
            set { _weightLimits = value == null ? [0, 0, 0, 0, 0, 0, 0] : value; }
        }

        public float[] FloorAscentPrices
        {
            get { return _floorAscentPrices; }
            set { _floorAscentPrices = value == null ? [0, 0, 0, 0, 0, 0, 0] : value; }
        }

        public float[] FloorAscentPricesNoElevator
        {
            get { return _floorAscentPricesNoElevator; }
            set { _floorAscentPricesNoElevator = value == null ? [0, 0, 0, 0, 0, 0, 0, 0] : value; }
        }

        public FloorAscent(DataSettings settings)
        {
            WeightLimits = settings.WeightLimits;
            FloorAscentPrices = settings.FloorAscentPrices;
            FloorAscentPricesNoElevator = settings.FloorAscentPricesNoElevator;
            margin = settings.MarginPercent;
        }

        public float Calculate(bool isElevator, bool isLargeBox, float floorNumber, float weight)
        {
            if (FloorAscentPrices == null) return -1;

            float result = -1;

            if (isElevator) result = PriceWithElevator(weight);
            else result = PriceWithoutElevator(weight, floorNumber);

            if (isLargeBox) result = 1000000;

            return result;
        }

        private float PriceWithElevator(float weight)
        {
            float result = -2;
            for (int i = 0; i < WeightLimits.Length; i++)
            {
                if (weight <= WeightLimits[i])
                {
                    return FloorAscentPrices[i];
                }
            }
            return result;
        }

        private float PriceWithoutElevator(float weight, float floorNumber)
        {
            float result = -3;
            for (int i = 0; i < WeightLimits.Length; i++)
            {
                if (i < 6 && weight <= WeightLimits[i])
                {
                    return FloorAscentPricesNoElevator[i] * floorNumber * margin;
                }
                if (i >= 6 && weight <= WeightLimits[i])
                {
                    return PriceWithoutElevatorCustom(weight, floorNumber);
                }
            }
            return result;
        }

        private float PriceWithoutElevatorCustom(float weight, float floorNumber)
        {
            float result = -2;
            if (weight >= WeightLimits[5])
            {
                //Устанавливаем цену за 500 кг
                result = FloorAscentPricesNoElevator[5];
                // Рассчитываем стоимость за вес, который свыше 500 кг.
                float underWeightLimit = FloorAscentPricesNoElevator[7] *
                    (float)Math.Ceiling((weight - WeightLimits[5]) / 100);
                return (result + underWeightLimit) * floorNumber;
            }
            else return -1;
        }

        private float LargeBoxesCalculate(float numberOfBoxes)
        {
            return 0;
        }
    }
}
