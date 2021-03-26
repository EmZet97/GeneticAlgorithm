using GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Other
{
    public class InversionProcessor : IOtherProcessor
    {
        public float Probability { get; init; }

        public InversionProcessor(float probability)
        {
            Probability = probability;
        }

        public IEnumerable<Entity> Process(in IEnumerable<Entity> population)
        {
            var processedPopulation = new List<Entity>();

            foreach (var entity in population)
            {
                if (new Random().NextDouble() <= Probability)
                {
                    processedPopulation.Add(Inverse(entity));
                    continue;
                }

                processedPopulation.Add(entity);
            }

            return processedPopulation;
        }

        private static Entity Inverse(Entity entity)
        {
            var inversionPoint1 = (uint)(new Random().NextDouble() * entity.Chromosome.Genes.Length / 2);
            var inversionPoint2 = (uint)(entity.Chromosome.Genes.Length / 2 + new Random().NextDouble() * entity.Chromosome.Genes.Length / 2);

            var frontPart = entity.Chromosome.Extract(0, inversionPoint1);
            var reversedGenes = new Chromosome(entity.Chromosome.Extract(inversionPoint1, inversionPoint2).Genes.Reverse().ToArray());
            var backPart = entity.Chromosome.Extract(inversionPoint2, (uint)entity.Chromosome.Genes.Length);

            return entity.Reproduct(frontPart + reversedGenes + backPart);
        }
    }
}
