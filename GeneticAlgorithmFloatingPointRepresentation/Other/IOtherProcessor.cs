using GeneticAlgorithmFloatingPointRepresentation.Models;
using System.Collections.Generic;

namespace GeneticAlgorithmFloatingPointRepresentation.Other
{
    interface IOtherProcessor
    {
        public float Probability { get; init; }
        IEnumerable<Entity> Process(in IEnumerable<Entity> population);
    }
}
