using GeneticAlgorithm.Functions;
using System;
using System.Collections;

namespace GeneticAlgorithm.Models
{
    public class Entity
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float GetValue(IFunction function)
        {
            return function.Compute(X, Y);
        }

        public BitArray GetBytes()
        {
            return new BitArray(new [] { Convert.ToByte(X), Convert.ToByte(Y) });
        }
    }
}
