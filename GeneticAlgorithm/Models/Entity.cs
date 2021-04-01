using GeneticAlgorithm.Functions;
using System.Linq;

namespace GeneticAlgorithm.Models
{
    public class Entity
    {
        public ChromosomeDecoder ValueDecoder { get; private set; }
        public Chromosome Chromosome { get; private set; }
        public IFunction ValueFunction { get; set; }

        public Entity(ChromosomeDecoder valueDecoder, IFunction valueFunction, uint xSegment, uint ySegment)
        {
            ValueDecoder = valueDecoder;
            Chromosome = new Chromosome(xSegment, valueDecoder.SegmentsBitLength) + new Chromosome(ySegment, valueDecoder.SegmentsBitLength);
            ValueFunction = valueFunction;
        }

        private Entity(Chromosome chromosome, ChromosomeDecoder valueDecoder, IFunction valueFunction)
        {
            Chromosome = chromosome;
            ValueDecoder = valueDecoder;
            ValueFunction = valueFunction;
        }

        public Entity Reproduct(Chromosome reproductedChromosome)
        {
            if (reproductedChromosome.Genes.Length != Chromosome.Genes.Length)
                throw new System.InvalidOperationException();

            return new Entity(reproductedChromosome, ValueDecoder, ValueFunction);
        }

        public Entity Reproduct(Chromosome[] reproductedChromosomes)
        {
            if (reproductedChromosomes.Select(x => x.Genes.Length).Sum() != Chromosome.Genes.Length)
                throw new System.InvalidOperationException();

            var finalChromosome = new Chromosome(System.Array.Empty<bool>());
            foreach(var gene in reproductedChromosomes)
            {
                finalChromosome += gene;
            }

            return new Entity(finalChromosome, ValueDecoder, ValueFunction);
        }

        public float ValueX {
            get
            {
                return ValueDecoder.DecodeToValue((Chromosome / 2)[0]);
            }
        }

        public float ValueY
        {
            get
            {
                return ValueDecoder.DecodeToValue((Chromosome / 2)[1]);
            }
        }

        public float ValueZ
        {
            get
            {
                return (float)ValueFunction.Compute(ValueX, ValueY);
            }
        }

        public float ValueIndex
        {
            get
            {
                return ComputeValueIndex(ValueFunction, ValueZ);
            }
        }

        private static float ComputeValueIndex(IFunction valueFunction, double value)
        {
            var rangeToExtremum = valueFunction.Extremum.Value - value;
            rangeToExtremum *= rangeToExtremum < 0 ? -1 : 1;

            return ((float)rangeToExtremum);
        }
    }
}
