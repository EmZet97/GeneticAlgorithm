using GeneticAlgorithm.Functions;
using System.Collections;

namespace GeneticAlgorithm.Models
{
    public class Entity
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Value { get; private set; }
        public float ValueIndex { get; set; }

        public Entity(float x, float y, IFunction valueFunction)
        {
            X = x;
            Y = y;

            Value = valueFunction.Compute(x, y);

            var rangeToExtremum = valueFunction.Extremum.Value - Value;
            rangeToExtremum *= rangeToExtremum < 0 ? -1 : 1;
            ValueIndex = 1 / ((float)rangeToExtremum + 1);
        }

        public Entity(BitArray bits, IFunction valueFunction)
        {
            var parts = new int[2];
            bits.CopyTo(parts, 0);

            X = ((float)parts[0]) / (1_000_000);
            Y = ((float)parts[1]) / (1_000_000);

            Value = valueFunction.Compute(X, Y);

            var rangeToExtremum = valueFunction.Extremum.Value - Value;
            rangeToExtremum *= rangeToExtremum < 0 ? -1 : 1;
            ValueIndex = 1 / ((float)rangeToExtremum + 1);
        }

        public BitArray GetBytes()
        {
            int xNormalized = (short)(X * (1_000_000));
            int yNormalized = (short)(Y * (1_000_000));

            return new BitArray(new [] { xNormalized, yNormalized });
        }
    }
}
