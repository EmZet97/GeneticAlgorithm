using GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Crossovers
{
    public class TwoPointsCrossover : ICrossover
    {
        public float Probability { get; init; }

        public TwoPointsCrossover(float probablilty)
        {
            Probability = probablilty;
        }

        public IEnumerable<Entity> Crossover(IEnumerable<Entity> population, int finalPopulationSize)
        {
            if(!population.Any())
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
            var dividePoint1 = (uint)(new Random().NextDouble() * entity1.Chromosome.Genes.Length / 2);
            var dividePoint2 = (uint)(entity1.Chromosome.Genes.Length / 2 + new Random().NextDouble() * entity1.Chromosome.Genes.Length / 2);
            var chromosome1Parts = new[] { entity1.Chromosome.Extract(0, dividePoint1), entity1.Chromosome.Extract(dividePoint1, dividePoint2), entity1.Chromosome.Extract(dividePoint2, (uint)entity1.Chromosome.Genes.Length) };
            var chromosome2Parts = new[] { entity2.Chromosome.Extract(0, dividePoint1), entity2.Chromosome.Extract(dividePoint1, dividePoint2), entity2.Chromosome.Extract(dividePoint2, (uint)entity2.Chromosome.Genes.Length) };

            entity1 = entity1.Reproduct(chromosome1Parts[0] + chromosome2Parts[1] + chromosome1Parts[2]);
            entity2 = entity2.Reproduct(chromosome2Parts[0] + chromosome1Parts[1] + chromosome2Parts[2]);
        }
    }
}
