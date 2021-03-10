using GeneticAlgorithm.Functions;
using GeneticAlgorithm.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Models
{
    class PopulationController
    {
        public List<Entity> Population { get; set; } = new ();

        public PopulationController()
        {
            var rand = new Random((int)DateTime.Now.ToFileTime());
            var function = new StyblinskiTangFunction();

            for (int i = 0; i < 20; i++)
            {
                Population.Add(new Entity(
                    (float)rand.NextDouble() * 10 - 5,
                    (float)rand.NextDouble() * 10 - 5,
                    function
                ));
            }

            var selected = new RouletteSelector().Select(Population, 0.25f, out IEnumerable<Entity> restOfPopulation).ToArray();
            var rest = restOfPopulation.ToArray();
        }
    }
}
