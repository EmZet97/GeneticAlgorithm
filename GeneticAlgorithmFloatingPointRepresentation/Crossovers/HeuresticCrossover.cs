using GeneticAlgorithmFloatingPointRepresentation.Crossovers;
using GeneticAlgorithmFloatingPointRepresentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmFloatingPointRepresentation.Crossovers
{
    public class HeuresticCrossover : ICrossover
    {
        public float Probability { get; init; }

        public HeuresticCrossover(float probability)
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
                {
                    var crossed = CrossoverEntities(first, second);

                    if(crossed is not null)
                    {
                        finalPopulation.Add(crossed);
                        continue;
                    }
                }
                    

                finalPopulation.AddRange(new[] { first, second });
            }

            finalPopulation.AddRange(processedPopulation);

            return finalPopulation.Take(finalPopulationSize);
        }

        private static Entity CrossoverEntities(Entity entity1, Entity entity2)
        {
            var crossPoint = new Random().NextDouble();

            if(entity1.ValueX > entity2.ValueX || entity1.ValueY > entity2.ValueY)
            {
                return null;
            }

            var x = crossPoint * (entity2.ValueX - entity1.ValueX) + entity1.ValueX;
            var y = crossPoint * (entity2.ValueY - entity1.ValueY) + entity1.ValueY;

            return entity1.Reproduct(x, y);
        }
    }
}
