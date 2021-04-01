using GeneticAlgorithmFloatingPointRepresentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmFloatingPointRepresentation.Selections
{
    public class TurnamentSelector : ISelector
    {
        public float Probability { get; init; }

        public TurnamentSelector(float probability)
        {
            Probability = probability;
        }

        public IEnumerable<Entity> Select(IEnumerable<Entity> population)
        {
            var countOfGroups = (int)(population.Count() * Probability);

            var groups = CreateTurnamentGroups(population, countOfGroups);

            var winners = new List<Entity>();

            foreach (var group in groups)
            {
                var rank = group.OrderBy(e => e.ValueIndex);
                winners.Add(rank.First());              
            }

            return winners;
        }

        private static List<List<Entity>> CreateTurnamentGroups(IEnumerable<Entity> entities, int countOfGroups)
        {
            List<List<Entity>> turnamentGroups = new();

            for (int i = 0; i < countOfGroups; i++)
            {
                turnamentGroups.Add(new List<Entity>());
            }

            var random = new Random();
            var unassignedEntities = entities.ToList();

            for (int i = 0; i < entities.Count(); i++)
            {
                var selectedEntity = unassignedEntities[random.Next(unassignedEntities.Count)];
                turnamentGroups[i % countOfGroups].Add(selectedEntity);

                unassignedEntities.Remove(selectedEntity);
            }

            return turnamentGroups;
        }
    }
}
