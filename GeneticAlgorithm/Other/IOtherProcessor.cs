using GeneticAlgorithm.Models;
using System.Collections.Generic;

namespace GeneticAlgorithm.Other
{
    interface IOtherProcessor
    {
        public float Probability { get; init; }
        IEnumerable<Entity> Process(in IEnumerable<Entity> population);
    }
}
