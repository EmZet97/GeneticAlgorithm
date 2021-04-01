using GeneticAlgorithmFloatingPointRepresentation.Crossovers;
using GeneticAlgorithmFloatingPointRepresentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmFloatingPointRepresentation.Crossovers
{
    public class ArytmeticCrossover : ICrossover
    {
        public float Probability { get; init; }

        public ArytmeticCrossover(float probability)
        {
            Probability = probability;
        }

        public IEnumerable<Entity> Crossover(IEnumerable<Entity> population, int finalPopulationSize)
        {
            if (!population.Any())
                return population;

            var processedPopulation = population.ToList();
            var finalPopulation = new List<Entity>();

            var random = new Random();
            while (finalPopulation.Count < finalPopulationSize)
            {
                if (population.Count() < 2)
                {
                    finalPopulation.AddRange(processedPopulation);
                    continue;
                }

                var currentPopulation = processedPopulation.ToArray().ToList();
                var first = currentPopulation[random.Next(0, currentPopulation.Count)];
                currentPopulation.Remove(first);
                var second = currentPopulation[random.Next(0, currentPopulation.Count)];

                if (random.NextDouble() <= Probability)
                    CrossoverEntities(ref first, ref second);

                finalPopulation.AddRange(new[] { first, second });
            }

            finalPopulation.AddRange(processedPopulation);

            return finalPopulation.Take(finalPopulationSize);
        }

        private static void CrossoverEntities(ref Entity entity1, ref Entity entity2)
        {
            var crossPoint1 = new Random().NextDouble();
            var crossPoint2 = new Random().NextDouble();

            var x1 = crossPoint1 * entity1.ValueX + (1 - crossPoint1) * entity2.ValueX;
            var x2 = crossPoint1 * entity2.ValueX + (1 - crossPoint1) * entity1.ValueX;
            var y1 = crossPoint2 * entity1.ValueY + (1 - crossPoint2) * entity2.ValueY;
            var y2 = crossPoint2 * entity2.ValueY + (1 - crossPoint2) * entity1.ValueY;


            entity1 = entity1.Reproduct(x1, y1);
            entity2 = entity2.Reproduct(x2, y2);
        }
    }
}
