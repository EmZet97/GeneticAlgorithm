using GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Mutation
{
    class TwoPointsMutation : IMutation
    {
        public float Probability { get; init; }

        public TwoPointsMutation(float probability)
        {
            Probability = probability;
        }

        public IEnumerable<Entity> Mutate(IEnumerable<Entity> population)
        {
            var mutatedPopulation = new List<Entity>();

            foreach (var entity in population)
            {
                if (new Random().NextDouble() <= Probability)
                {
                    mutatedPopulation.Add(MutateEntity(entity));
                    continue;
                }

                mutatedPopulation.Add(entity);
            }

            return mutatedPopulation;

        }

        private static Entity MutateEntity(Entity entity)
        {
            var mutationPoint1 = (uint)(new Random().NextDouble() * entity.Chromosome.Genes.Length / 2);
            var mutationPoint2 = (uint)(entity.Chromosome.Genes.Length / 2 + new Random().NextDouble() * entity.Chromosome.Genes.Length/2);

            var frontPart = entity.Chromosome.Extract(0, mutationPoint1);
            var reversedGene1 = new Chromosome(new[] { !entity.Chromosome.Extract(mutationPoint1).Genes[0] });
            var middlePart = entity.Chromosome.Extract(mutationPoint1 + 1, mutationPoint2);
            var reversedGene2 = new Chromosome(new[] { !entity.Chromosome.Extract(mutationPoint2).Genes[0] });
            var back = mutationPoint2 < entity.Chromosome.Genes.Length - 1 ? entity.Chromosome.Extract(mutationPoint2 + 1, (uint)entity.Chromosome.Genes.Length) : new Chromosome(Array.Empty<bool>());

            var newChromosome = frontPart + reversedGene1 + middlePart + reversedGene2 + back;

            return entity.Reproduct(newChromosome);
        }
    }
}
