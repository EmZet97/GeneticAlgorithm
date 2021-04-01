using GeneticAlgorithmFloatingPointRepresentation.Models;
using System.Collections.Generic;

namespace GeneticAlgorithmFloatingPointRepresentation.Mutation
{
    interface IMutation
    {
        public float Probability { get; init; }
        IEnumerable<Entity> Mutate(IEnumerable<Entity> population);
    }
}
