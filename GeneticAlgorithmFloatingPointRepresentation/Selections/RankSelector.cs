using GeneticAlgorithmFloatingPointRepresentation.Models;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmFloatingPointRepresentation.Selections
{
    public class RankSelector : ISelector
    {
        public float Probability { get; init; }

        public RankSelector(float probability)
        {
            Probability = probability;
        }

        public IEnumerable<Entity> Select(IEnumerable<Entity> population)
        {
            var selectionCount = (int)(population.Count() * Probability);

            var bests = population.OrderBy(e => e.ValueIndex).Take(selectionCount).ToArray();

            return bests;
        }
    }
}
