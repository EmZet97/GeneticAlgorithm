using GeneticAlgorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Selections
{
    public class RouletteSelector : ISelector
    {
        public float Probability { get; init; }

        class RouletteElement
        {
            public Entity Entity { get; init; }
            public double UpperBorder { get; init; }
            public RouletteElement Next { get; set; }

            public RouletteElement(double upperBorder, double sumOfValues, IEnumerable<Entity> entities)
            {
                Entity = entities.First();
                UpperBorder = upperBorder + Entity.ValueIndex / sumOfValues;

                if (entities.Count() > 1)
                    Next = new RouletteElement(UpperBorder, sumOfValues, entities.Skip(1));
            }

            public Entity FindElementInPosition(double position)
            {
                if (position < UpperBorder || Next is null)
                    return Entity;

                return Next.FindElementInPosition(position);
            }
        }

        public RouletteSelector(float probability)
        {
            Probability = probability;
        }

        public IEnumerable<Entity> Select(IEnumerable<Entity> population)
        {         
            uint elementsToSelection = (uint)(population.Count() * (Probability - (int)Probability));

            var random = new Random();
            List<Entity> selectedEntities = new();

            for (int i = 0; i < elementsToSelection; i++)
            {
                var roulette = CreateRoulette(population);
                var selectedEntity = roulette.FindElementInPosition(random.NextDouble());
                selectedEntities.Add(selectedEntity);
            }

            return selectedEntities.ToArray();
        }

        private static RouletteElement CreateRoulette(IEnumerable<Entity> entryPopulation)
        {
            var sumOfValues = entryPopulation.Select(e => e.ValueIndex).Sum();
            var firstElement = new RouletteElement(0, sumOfValues, entryPopulation);

            return firstElement;
        }
    }
}
