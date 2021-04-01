using GeneticAlgorithmFloatingPointRepresentation.Models;
using GeneticAlgorithmFloatingPointRepresentation.Mutation;
using System;
using System.Collections.Generic;

namespace GeneticAlgorithmFloatingPointRepresentation.Mutation
{
    public class IndexSwapMutation : IMutation
    {
        public float Probability { get; init; }

        public IndexSwapMutation(float probability)
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
            return entity.Reproduct(entity.ValueY, entity.ValueX);
        }
    }
}
