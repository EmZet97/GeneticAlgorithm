using GeneticAlgorithm.Models;
using System.Collections.Generic;

namespace GeneticAlgorithm.Extractors
{
    interface IExtractor
    {
        public float Percent { get; init; }
        IEnumerable<Entity> ExtractFrom(in IEnumerable<Entity> population);
    }
}
