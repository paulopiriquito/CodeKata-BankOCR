using System.Collections.Generic;

namespace SegmentationReader
{
    public class SevenSegment : ISevenSegment
    {
        private static readonly int[] Encoding = new [] {63, 6, 91, 79, 102, 109, 125, 7, 127, 111};
        public static readonly int[] UpperLineEncoding = new[] {0, 1, 0};
        public static readonly int[] MiddleLineEncoding = new[] {32, 64, 2};
        public static readonly int[] BottomLineEncoding = new[] {16, 8, 4};

        private int? _asInteger = null;
        
        private int AccumulatedWeight { get; set; }
        
        public SevenSegment() { }

        public SevenSegment(int number)
        {
            if (number < 0 || number > 9) return;
            AccumulatedWeight = Encoding[number];
            _asInteger = number;
        }

        public void AccumulateSegment(int segmentEncodedWeight)
        {
            AccumulatedWeight |= segmentEncodedWeight;
        }

        public List<int> OneCharacterDifferenceNeighbours()
        {
            var neighbours = new List<int>();
            for (var i = 0; i < 7; i++)
            {
                var iterationWeight = 1 << i;

                var alternativeAccumulatedWeight = AccumulatedWeight ^ iterationWeight;

                var encodedNumber = GetEncodedNumber(alternativeAccumulatedWeight);
                
                if (encodedNumber != null)
                {
                    neighbours.Add(encodedNumber.Value);
                }
            }
            return neighbours;
        }

        private static int? GetEncodedNumber(int bits)
        {
            int? number = null;
            for (var i = 0; i < Encoding.Length; i++)
            {
                if (bits == Encoding[i])
                {
                    number = i;
                    break;
                }
            }
            return number;
        }
        
        public int? ToInteger()
        {
            if (_asInteger != null) 
                return _asInteger;
            _asInteger = GetEncodedNumber(AccumulatedWeight);
            return _asInteger;
        }
        
        public override string ToString()
        {
            return ToInteger() == null ? "?" : ToInteger().ToString();
        }
    }
}