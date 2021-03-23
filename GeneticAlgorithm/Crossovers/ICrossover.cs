using GeneticAlgorithm.Models;
using System.Collections.Generic;

namespace GeneticAlgorithm.Crossovers
{
    interface ICrossover
    {
        public float Probability { get; init; }
        IEnumerable<Entity> Crossover(IEnumerable<Entity> population, int finalPopulationSize);
    }
}
