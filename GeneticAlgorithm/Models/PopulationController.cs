using GeneticAlgorithm.Crossovers;
using GeneticAlgorithm.Functions;
using GeneticAlgorithm.Mutation;
using GeneticAlgorithm.Selections;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Models
{
    class PopulationController
    {
        public List<Entity> Population { get; private set; }
        private IFunction ValueFunction;
        private ICrossover CrossoverMethod;
        private IMutation MutationMethod;
        private ISelector SelectionMethod;

        private uint Precision;
        private float MinValue;
        private float MaxValue;

        private uint PopulationSize;

        public PopulationController(IFunction valueFunction, ICrossover crossoverMethod, IMutation mutationMethod, ISelector selectionMethod, uint precision, float minValue, float maxValue, uint populationSize)
        {
            ValueFunction = valueFunction;
            CrossoverMethod = crossoverMethod;
            MutationMethod = mutationMethod;
            SelectionMethod = selectionMethod;
            Precision = precision;
            MinValue = minValue;
            MaxValue = maxValue;
            PopulationSize = populationSize;
        }

        public void StartEvolution(uint epochs)
        {
            InitPopulation();

            for (int i = 0; i < epochs; i++)
            {
                Evolve();
            }
        }

        private void InitPopulation()
        {
            Population = new();
            var decoder = new ChromosomeDecoder(Precision, MinValue, MaxValue);
            var generator = new EntityGenerator(decoder, ValueFunction);

            for (int i = 0; i < PopulationSize; i++)
            {
                Population.Add(generator.GetNext());
            }
        }

        private void Evolve()
        {
            var selectedPopulation = SelectionMethod.Select(Population, out var restOfPopulation);

            var crossoveredPopulation = CrossoverMethod.Crossover(selectedPopulation);

            var mutatedPopulation = MutationMethod.Mutate(crossoveredPopulation);

            var strongestEntities = Population.OrderByDescending(e => e.ValueIndex).Take(Population.Count - mutatedPopulation.Count());

            var finalPopulation = new List<Entity>();
            finalPopulation.AddRange(mutatedPopulation);
            finalPopulation.AddRange(strongestEntities);

            Population = finalPopulation;
        }
    }
}
