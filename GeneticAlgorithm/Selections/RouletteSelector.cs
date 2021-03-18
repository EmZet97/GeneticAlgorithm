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

        public IEnumerable<Entity> Select(IEnumerable<Entity> population, out IEnumerable<Entity> restOfPopulation)
        {         
            uint elementsToSelection = (uint)(population.Count() * (Probability - (int)Probability));

            var currentPopulation = population.ToList();
            var random = new Random();
            List<Entity> selectedEntities = new();

            for (int i = 0; i < elementsToSelection; i++)
            {
                var roulette = CreateRoulette(currentPopulation);
                var selectedEntity = roulette.FindElementInPosition(random.NextDouble());
                selectedEntities.Add(selectedEntity);
                currentPopulation.Remove(selectedEntity);
            }

            restOfPopulation = currentPopulation;
            return selectedEntities.ToArray();
        }

        private static RouletteElement CreateRoulette(IEnumerable<Entity> entryPopulation)
        {
            var sumOfValues = entryPopulation.Select(e => e.ValueIndex).Sum();

            RouletteElement firstRouletteElement = new ()
            {
                Entity = entryPopulation.First(),
                UpperBorder = entryPopulation.First().ValueIndex / sumOfValues
            };

            RouletteElement prevRouletteElement = null;
            foreach (var entity in entryPopulation)
            {
                var currentValue = entity.ValueIndex / sumOfValues;
                var rouletteElement = new RouletteElement()
                {
                    Entity = entity,
                    UpperBorder = prevRouletteElement is not null ? prevRouletteElement.UpperBorder + currentValue : currentValue
                };

                if (prevRouletteElement is not null)
                {
                    prevRouletteElement.Next = rouletteElement;
                }
            }

            return firstRouletteElement;
        }
    }
}
