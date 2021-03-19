using GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Crossovers
{
    public class OnePointCrossover : ICrossover
    {
        public float Probability { get; init; }

        public OnePointCrossover(float probablilty)
        {
            Probability = probablilty;
        }

        public IEnumerable<Entity> Crossover(IEnumerable<Entity> population)
        {
            var processedPopulation = population.ToList();
            var crossedPopulation = new List<Entity>();

            var random = new Random();
            for (int i = 0; i < population.Count() / 2; i++)
            {
                var first = processedPopulation[random.Next(0, processedPopulation.Count)];
                processedPopulation.Remove(first);
                var second = processedPopulation[random.Next(0, processedPopulation.Count)];
                processedPopulation.Remove(second);

                if(random.NextDouble() <= Probability)
                    CrossoverEntities(ref first, ref second);

                crossedPopulation.Add(first);
                crossedPopulation.Add(second);
            }

            crossedPopulation.AddRange(processedPopulation);

            return crossedPopulation;
        }

        private static void CrossoverEntities(ref Entity entity1, ref Entity entity2)
        {
            var dividePoint = (uint)(new Random().NextDouble() * entity1.Chromosome.Genes.Length);
            var chromosome1Parts = new[] { entity1.Chromosome.Extract(0, dividePoint), entity1.Chromosome.Extract(dividePoint, (uint)entity1.Chromosome.Genes.Length) };
            var chromosome2Parts = new[] { entity2.Chromosome.Extract(0, dividePoint), entity2.Chromosome.Extract(dividePoint, (uint)entity2.Chromosome.Genes.Length) };

            entity1 = entity1.Reproduct(chromosome1Parts[0] + chromosome2Parts[1]);
            entity2 = entity2.Reproduct(chromosome2Parts[0] + chromosome1Parts[1]);
        }
    }
}
