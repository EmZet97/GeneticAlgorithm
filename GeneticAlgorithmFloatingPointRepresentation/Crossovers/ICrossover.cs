using GeneticAlgorithmFloatingPointRepresentation.Models;
using System.Collections.Generic;

namespace GeneticAlgorithmFloatingPointRepresentation.Crossovers
{
    interface ICrossover
    {
        public float Probability { get; init; }
        IEnumerable<Entity> Crossover(IEnumerable<Entity> population, int finalPopulationSize);
    }
}
