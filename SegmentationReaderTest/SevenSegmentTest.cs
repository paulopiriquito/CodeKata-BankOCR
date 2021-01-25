using SegmentationReader;
using Shouldly;
using Xunit;

namespace SegmentationReaderTest
{
    public class SevenSegmentTest
    {
        [Fact]
        public void AccumulateSegmentMustAccumulateTest()
        {
            ISevenSegment sevenSegment = new SevenSegment();
            
            sevenSegment.AccumulateSegment(SevenSegment.UpperLineEncoding[0]);
            sevenSegment.ToInteger().ShouldBeNull();
            
            sevenSegment.AccumulateSegment(SevenSegment.UpperLineEncoding[1]);
            sevenSegment.ToInteger().ShouldBeNull();
            
            sevenSegment.AccumulateSegment(SevenSegment.MiddleLineEncoding[0]);
            sevenSegment.ToInteger().ShouldBeNull();
            
            sevenSegment.AccumulateSegment(SevenSegment.MiddleLineEncoding[0]);
            sevenSegment.ToInteger().ShouldBeNull();
            
            sevenSegment.AccumulateSegment(SevenSegment.MiddleLineEncoding[1]);
            sevenSegment.ToInteger().ShouldBeNull();
            
            sevenSegment.AccumulateSegment(SevenSegment.MiddleLineEncoding[2]);
            sevenSegment.ToInteger().ShouldBeNull();
            
            sevenSegment.AccumulateSegment(SevenSegment.BottomLineEncoding[0]);
            sevenSegment.ToInteger().ShouldBeNull();
            
            sevenSegment.AccumulateSegment(SevenSegment.BottomLineEncoding[1]);
            sevenSegment.ToInteger().ShouldBeNull();
            
            sevenSegment.AccumulateSegment(SevenSegment.BottomLineEncoding[2]);
            sevenSegment.ToInteger().ShouldNotBeNull();
            
            sevenSegment.ToInteger().ShouldBe(8);
        }
        
        [Theory]
        [InlineData(
            " _ " + "\n" +
            "| |" + "\n" +
            "|_|", 0)]
        [InlineData(
            "   " + "\n" +
            "  |" + "\n" +
            "  |", 1)]
        [InlineData(
            " _ " + "\n" +
            " _|" + "\n" +
            "|_ ", 2)]
        [InlineData(
            " _ " + "\n" +
            " _|" + "\n" +
            " _|", 3)]
        [InlineData(
            "   " + "\n" +
            "|_|" + "\n" +
            "  |", 4)]
        [InlineData(
            " _ " + "\n" +
            "|_ " + "\n" +
            " _|", 5)]
        [InlineData(
            " _ " + "\n" +
            "|_ " + "\n" +
            "|_|", 6)]
        [InlineData(
            " _ " + "\n" +
            "  |" + "\n" +
            "  |", 7)]
        [InlineData(
            " _ " + "\n" +
            "|_|" + "\n" +
            "|_|", 8)]
        [InlineData(
            " _ " + "\n" +
            "|_|" + "\n" +
            " _|", 9)]
        [InlineData(
            " _ " + "\n" +
            " _|" + "\n" +
            "|_|", null)]
        [InlineData(
            " _ " + "\n" +
            "|_|" + "\n" +
            "|_ ", null)]
        public void SegmentShouldEncodeAllDecimalBaseIntegers(string sevenSegmentDigit, int? correspondingInteger)
        {
            ISevenSegment sevenSegment = GetSevenSegmentFromStringRepresentation(sevenSegmentDigit);
            sevenSegment.ToInteger().ShouldBe(correspondingInteger);
            sevenSegment.ToString().ShouldBe(correspondingInteger == null ? "?" : correspondingInteger.ToString());
        }

        [Theory]
        [InlineData(
            " _ " + "\n" +
            "|_|" + "\n" +
            " _|", new[]{3, 5, 8})]
        [InlineData(
            " _ " + "\n" +
            "| |" + "\n" +
            "|_|", new[]{8})]
        [InlineData(
            " _ " + "\n" +
            "|_|" + "\n" +
            "|_|", new[]{0, 6, 9})]
        [InlineData(
            "   " + "\n" +
            "  |" + "\n" +
            "  |", new[]{7})]
        [InlineData(
            " _ " + "\n" +
            "|_ " + "\n" +
            " _|", new[]{6,9})]
        public void SegmentShouldFindAllSinglePermutationNeighbours(string sevenSegmentDigit, int[] neighbours)
        {
            ISevenSegment sevenSegment = GetSevenSegmentFromStringRepresentation(sevenSegmentDigit);
            var calculatedNeighbours = sevenSegment.OneCharacterDifferenceNeighbours();
            
            calculatedNeighbours.ShouldBeSubsetOf(neighbours);
        }

        public static ISevenSegment GetSevenSegmentFromStringRepresentation(string representation)
        {
            ISevenSegment sevenSegment = new SevenSegment();

            string[] split = representation.Split("\n");

            for (int i = 0; i < split.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        for (int j = 0; j < split[i].Length; j++)
                            if (split[i][j] != ' ')
                                sevenSegment.AccumulateSegment(SevenSegment.UpperLineEncoding[j]);
                        break;
                    case 1: 
                        for (int j = 0; j < split[i].Length; j++)
                            if (split[i][j] != ' ')
                                sevenSegment.AccumulateSegment(SevenSegment.MiddleLineEncoding[j]);
                        break;
                    case 2: 
                        for (int j = 0; j < split[i].Length; j++)
                            if (split[i][j] != ' ')
                                sevenSegment.AccumulateSegment(SevenSegment.BottomLineEncoding[j]);
                        break;
                }
            }

            return sevenSegment;
        }
    }
}