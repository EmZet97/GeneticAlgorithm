using GeneticAlgorithmFloatingPointRepresentation.Models;
using GeneticAlgorithmFloatingPointRepresentation.Mutation;
using System;
using System.Collections.Generic;

namespace GeneticAlgorithmFloatingPointRepresentation.Mutation
{
    public class GaussMutation : IMutation
    {
        public float Probability { get; init; }

        public GaussMutation(float probability)
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
            var x = (entity.ValueX + SampleGaussian(0, Entity.MinMaxRange) + Entity.MinMaxRange) % (2* Entity.MinMaxRange) - Entity.MinMaxRange;
            var y = (entity.ValueY + SampleGaussian(0, Entity.MinMaxRange) + Entity.MinMaxRange) % (2* Entity.MinMaxRange) - Entity.MinMaxRange;

            return entity.Reproduct(x, y);
        }

        public static double SampleGaussian(double mean, double stddev)
        {
            var random = new Random();
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }
    }
}
