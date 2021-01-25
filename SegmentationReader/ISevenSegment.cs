using System.Collections.Generic;

namespace SegmentationReader
{
    public interface ISevenSegment
    {
        void AccumulateSegment(int segmentEncodedWeight);
        List<int> OneCharacterDifferenceNeighbours();
        int? ToInteger();
        string ToString();
    }
}