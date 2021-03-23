using GeneticAlgorithm.Crossovers;
using GeneticAlgorithm.Functions;
using GeneticAlgorithm.Mutation;
using GeneticAlgorithm.Selections;
using OxyPlot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Models
{
    class PopulationController
    {
        public List<Entity> Population { get; private set; }
        private EvolutionResult results = new();
        public readonly IFunction ValueFunction;
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

        public IEnumerable<EvolutionResult> StartEvolution(uint epochs)
        {
            InitPopulation();

            for (int i = 0; i < epochs; i++)
            {
                yield return Evolve(i);
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

        private EvolutionResult Evolve(int epoch)
        {
            var selectedPopulation = SelectionMethod.Select(Population);

            var crossoveredPopulation = CrossoverMethod.Crossover(selectedPopulation, Population.Count);

            var mutatedPopulation = MutationMethod.Mutate(crossoveredPopulation);

            //var strongestEntities = Population.OrderByDescending(e => e.ValueIndex).Take(Population.Count - mutatedPopulation.Count());

            //var finalPopulation = new List<Entity>();
            //finalPopulation.AddRange(mutatedPopulation);
            //finalPopulation.AddRange(strongestEntities);

            Population.Clear();
            Population = mutatedPopulation.ToList();

            var average = Population.Select(e => e.ValueIndex).Average();
            var max = Population.Select(e => e.ValueIndex).Max();

            var result = new EvolutionResult()
            {
                Best = new DataPoint(epoch, max),
                Indexes = new DataPoint(epoch, average),
                Points = Population.Select(x => new Point(x.ValueX, x.ValueY)).ToArray()
            };

            return result;
        }
    }
}
