using GeneticAlgorithm.Functions;
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

            for (int i = 0; i < 20; i++)
            {
                Population.Add(new Entity(
                    (float)rand.NextDouble() * 10 - 5,
                    (float)rand.NextDouble() * 10 - 5
                ));
            }

            var values = Population.Select(e => e.GetValue(new StyblinskiTangFunction())).ToArray();

            var bits = Population.Select(e => e.GetBytes()).ToArray();

            var backValues = bits.Select(b => new Entity(b)).ToArray();
        }
    }
}
