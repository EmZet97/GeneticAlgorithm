using GeneticAlgorithm.Functions;
using System;

namespace GeneticAlgorithm.Models
{
    public class EntityGenerator
    {
        public ChromosomeDecoder Decoder { get; set; }
        public IFunction Function { get; set; }

        public EntityGenerator(ChromosomeDecoder decoder, IFunction function)
        {
            Decoder = decoder;
            Function = function;
        }

        public Entity GetNext()
        {
            var random = new Random();
            var x = (uint)random.Next(0, (int)Decoder.SegmentsCount);
            var y = (uint)random.Next(0, (int)Decoder.SegmentsCount);
            return new Entity(Decoder, Function, x, y);
        }
    }
}
