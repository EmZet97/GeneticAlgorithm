using GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Mutation
{
    public class BorderMutation : IMutation
    {
        public float Probability { get; init; }

        public BorderMutation(float probability)
        {
            Probability = probability;
        }

        public IEnumerable<Entity> Mutate(IEnumerable<Entity> population)
        {
            var mutatedPopulation = new List<Entity>();

            foreach(var entity in population)
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
            var newChromosome = entity.Chromosome.Extract(0, (uint)entity.Chromosome.Genes.Length - 1) + new Chromosome(new[] { !entity.Chromosome.Genes.Last() });

            return entity.Reproduct(newChromosome);
        }
    }
}
