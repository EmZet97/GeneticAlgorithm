using GeneticAlgorithm.Functions;
using System.Collections;

namespace GeneticAlgorithm.Models
{
    public class Entity
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public Entity(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Entity(BitArray bits)
        {
            var parts = new int[2];
            bits.CopyTo(parts, 0);

            X = ((float)parts[0]) / (1<<5);
            Y = ((float)parts[1]) / (1<<5);
        }

        public float GetValue(IFunction function)
        {
            return function.Compute(X, Y);
        }

        public BitArray GetBytes()
        {
            int xNormalized = (short)(X * (1 << 5));
            int yNormalized = (short)(Y * (1 << 5));

            return new BitArray(new [] { xNormalized, yNormalized });
        }
    }
}
