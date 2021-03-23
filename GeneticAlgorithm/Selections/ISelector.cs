using GeneticAlgorithm.Models;
using System.Collections.Generic;

namespace GeneticAlgorithm.Selections
{
    interface ISelector
    {
        public float Probability { get; init; }
        IEnumerable<Entity> Select(IEnumerable<Entity> population);
    }
}
