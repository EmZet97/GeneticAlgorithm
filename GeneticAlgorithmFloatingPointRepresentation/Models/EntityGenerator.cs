using GeneticAlgorithmFloatingPointRepresentation.Functions;
using System;

namespace GeneticAlgorithmFloatingPointRepresentation.Models
{
    public class EntityGenerator
    {
        public IFunction Function { get; set; }
        private readonly double min;
        private readonly double max;

        public EntityGenerator(IFunction function, double min, double max)
        {
            Function = function;
            this.min = min;
            this.max = max;
        }

        public Entity GetNext()
        {
            var random = new Random();
            var x = random.NextDouble() * (max - min) + min;
            var y = random.NextDouble() * (max - min) + min;
            return new Entity(Function, x, y);
        }
    }
}
