using GeneticAlgorithmFloatingPointRepresentation.Models;
using GeneticAlgorithmFloatingPointRepresentation.Mutation;
using System;
using System.Collections.Generic;

namespace GeneticAlgorithmFloatingPointRepresentation.Mutation
{
    public class UniformMutation : IMutation
    {
        public float Probability { get; init; }

        public UniformMutation(float probability)
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
            var random = new Random();

            if (random.NextDouble() >= 0.5f)
            {
                var newX = random.NextDouble() * Entity.MinMaxRange * 2 - Entity.MinMaxRange;
                return entity.Reproduct(newX, entity.ValueY);
            }

            var newY = random.NextDouble() * Entity.MinMaxRange * 2 - Entity.MinMaxRange;
            return entity.Reproduct(entity.ValueX, newY);
        }
    }
}
