using GeneticAlgorithm.Models;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Extractors
{
    public class ElitistStrategyExtractor : IExtractor
    {
        public float Percent { get; init; }

        public ElitistStrategyExtractor(float percent)
        {
            Percent = percent;
        }
        public IEnumerable<Entity> ExtractFrom(in IEnumerable<Entity> population)
        {
            var entitiesNumber = (int)(population.Count() * Percent);

            var extracted = population.OrderBy(e => e.ValueIndex).Take(entitiesNumber);

            return extracted;
        }
    }
}
