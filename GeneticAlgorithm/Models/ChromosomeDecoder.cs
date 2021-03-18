using System;

namespace GeneticAlgorithm.Models
{
    public class ChromosomeDecoder
    {
        public float MinValue { get; private set; }
        public uint SegmentsCount { get; private set; }
        public float SingleSegmentLength { get; private set; }
        public uint SegmentsBitLength { get; private set; }

        public ChromosomeDecoder(uint precision, float minValue, float maxValue)
        {
            var precisionLog = Math.Log2(precision);
            var isPrecisionReound = precisionLog % 1 == 0;
            SegmentsBitLength = (uint)precisionLog - (uint)(isPrecisionReound ? 1 : 0);
            SegmentsCount = (uint)Math.Pow(2, SegmentsBitLength + 1);

            MinValue = minValue;
            SingleSegmentLength = (maxValue - minValue) / SegmentsCount;
        }

        public float DecodeToValue(Chromosome bits)
        {
            var segments = bits.ToInt();
            if (segments > SegmentsCount)
                throw new InvalidOperationException();

            return MinValue + bits.ToInt() * SingleSegmentLength;
        }

        public static uint DecodeToSegment(Chromosome bits)
        {
            return (uint)bits.ToInt();
        }

        public Chromosome EncodeSegment(uint value)
        {
            return new Chromosome(value, SegmentsBitLength);
        }
    }
}
