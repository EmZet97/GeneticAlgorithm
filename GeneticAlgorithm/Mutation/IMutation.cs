using GeneticAlgorithm.Models;
using System.Collections.Generic;

namespace GeneticAlgorithm.Mutation
{
    interface IMutation
    {
        public float Probability { get; init; }
        IEnumerable<Entity> Mutate(IEnumerable<Entity> population);
    }
}
