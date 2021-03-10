using GeneticAlgorithm.Models;
using System.Collections.Generic;

namespace GeneticAlgorithm.Selections
{
    interface ISelector
    {
        IEnumerable<Entity> Select(IEnumerable<Entity> population, float partOfPopulation, out IEnumerable<Entity> restOfPopulation);
    }
}
