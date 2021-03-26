using GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Mutation
{
    class OnePointMutation : IMutation
    {
        public float Probability { get; init; }

        public OnePointMutation(float probability)
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
            var mutationPoint = (uint)(new Random().NextDouble() * entity.Chromosome.Genes.Length);

            var frontPart = entity.Chromosome.Extract(0, mutationPoint);
            var reversedGene = new Chromosome(new[] { !entity.Chromosome.Extract(mutationPoint).Genes[0] });
            var back = mutationPoint < entity.Chromosome.Genes.Length - 1 ? entity.Chromosome.Extract(mutationPoint + 1, (uint)entity.Chromosome.Genes.Length) : new Chromosome(Array.Empty<bool>());

            var newChromosome = frontPart + reversedGene + back;

            return entity.Reproduct(newChromosome);
        }
    }
}
