using GeneticAlgorithm.Crossovers;
using GeneticAlgorithm.Functions;
using GeneticAlgorithm.Mutation;
using GeneticAlgorithm.Selections;
using OxyPlot;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Models
{
    class PopulationController
    {
        public List<Entity> Population { get; private set; }
        private uint CurrentEpoch = 0;

        public readonly IFunction ValueFunction;
        private readonly ICrossover CrossoverMethod;
        private readonly IMutation MutationMethod;
        private readonly ISelector SelectionMethod;

        private readonly uint Precision;
        private readonly float MinValue;
        private readonly float MaxValue;

        private readonly uint PopulationSize;

        public PopulationController() { }

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

        public IEnumerable<EvolutionResult> StartEvolution(uint epochs)
        {
            InitPopulation();
            yield return GetEvolutionResult();

            for (int i = 0; i < epochs; i++)
            {
                yield return Evolve();
            }
        }

        private void InitPopulation()
        {
            Population = new();
            CurrentEpoch = 0;

            var decoder = new ChromosomeDecoder(Precision, MinValue, MaxValue);
            var generator = new EntityGenerator(decoder, ValueFunction);

            for (int i = 0; i < PopulationSize; i++)
            {
                Population.Add(generator.GetNext());
            }
        }

        private EvolutionResult Evolve()
        {
            var processedPopulation = SelectionMethod.Select(Population);

            if (processedPopulation.Any())
            {
                var crossoveredPopulation = CrossoverMethod.Crossover(processedPopulation, Population.Count);
                var mutatedPopulation = MutationMethod.Mutate(crossoveredPopulation);
                processedPopulation = mutatedPopulation;
            }
            else
            {
                processedPopulation = Population.ToArray();
            }


            //var strongestEntities = Population.OrderByDescending(e => e.ValueIndex).Take(Population.Count - mutatedPopulation.Count());

            //var finalPopulation = new List<Entity>();
            //finalPopulation.AddRange(mutatedPopulation);
            //finalPopulation.AddRange(strongestEntities);

            Population.Clear();
            Population = processedPopulation.ToList();
            CurrentEpoch += 1;

            return GetEvolutionResult();
        }

        private EvolutionResult GetEvolutionResult()
        {
            var average = Population.Select(e => e.ValueIndex).Average();
            var max = Population.Select(e => e.ValueIndex).Max();

            var result = new EvolutionResult()
            {
                Best = new DataPoint(CurrentEpoch, max),
                Indexes = new DataPoint(CurrentEpoch, average),
                Points = Population.Select(x => new Point(x.ValueX, x.ValueY)).ToArray()
            };

            return result;
        }
    }
}
