using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Models
{
    public class Chromosome
    {
        public bool[] Genes { get; init; }

        public Chromosome(uint number, uint length)
        {
            Genes = ConvertToBitList(number, length).ToArray();
        }

        public Chromosome(bool[] bits)
        {
            Genes = bits;
        }

        public int ToInt()
        {
            var number = 0;
            for (int i = 0; i < Genes.Length; i++) 
                if (Genes[i]) number |= 1 << (Genes.Length - i);

            return number/2;
        }

        private static List<bool> ConvertToBitList(uint number, uint length)
        {
            var processedNumber = number;
            var bitList = new List<bool>();

            for (int i = 0; i < length; i++)
            {
                bitList.Insert(0, processedNumber % 2 == 1);
                processedNumber /= 2;
            }

            return bitList;
        }

        public static Chromosome operator +(Chromosome a, Chromosome b)
        {
            return new Chromosome(a.Genes.Concat(b.Genes).ToArray());
        }

        public static Chromosome[] operator /(Chromosome a, uint b)
        {
            var splitedCollections = new List<Chromosome>();
            var elementsPerCollection = (int)(a.Genes.Length / b);

            for (int i = 0; i < b; i++)
            {
                var collection = a.Genes.Skip(i * elementsPerCollection).Take((i + 1) * elementsPerCollection);
                splitedCollections.Add(new Chromosome(collection.ToArray()));
            }

            return splitedCollections.ToArray();
        }

        public Chromosome Extract(uint from, uint to)
        {
            var seperatedGenes = Genes.Skip((int)from).Take((int)(to - from));

            return new Chromosome(seperatedGenes.ToArray());
        }

        public Chromosome Extract(uint index)
        {
            var seperatedGenes = Genes.Skip((int)index).Take(1);

            return new Chromosome(seperatedGenes.ToArray());
        }
    }
}
