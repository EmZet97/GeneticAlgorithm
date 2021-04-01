using GeneticAlgorithmFloatingPointRepresentation.Models;
using System.Collections.Generic;

namespace GeneticAlgorithmFloatingPointRepresentation.Extractors
{
    interface IExtractor
    {
        public float Percent { get; init; }
        IEnumerable<Entity> ExtractFrom(in IEnumerable<Entity> population);
    }
}
