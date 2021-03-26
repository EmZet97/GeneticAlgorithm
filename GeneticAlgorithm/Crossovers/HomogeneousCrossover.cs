using GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Crossovers
{
    public class HomogeneousCrossover : ICrossover
    {
        public float Probability { get; init; }

        public HomogeneousCrossover(float probablilty)
        {
            Probability = probablilty;
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
            var genes1 = new Chromosome(Array.Empty<bool>());
            var genes2 = new Chromosome(Array.Empty<bool>());

            for (int i = 0; i < entity1.Chromosome.Genes.Length; i++)
            {
                genes1 += i % 2 == 0 ? entity1.Chromosome.Extract((uint)i) : entity2.Chromosome.Extract((uint)i);
                genes2 += i % 2 == 0 ? entity2.Chromosome.Extract((uint)i) : entity1.Chromosome.Extract((uint)i);
            }

            entity1 = entity1.Reproduct(genes1);
            entity2 = entity2.Reproduct(genes2);
        }
    }
}