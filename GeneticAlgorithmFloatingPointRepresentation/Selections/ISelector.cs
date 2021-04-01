using GeneticAlgorithmFloatingPointRepresentation.Models;
using System.Collections.Generic;

namespace GeneticAlgorithmFloatingPointRepresentation.Selections
{
    interface ISelector
    {
        public float Probability { get; init; }
        IEnumerable<Entity> Select(IEnumerable<Entity> population);
    }
}
