using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SegmentationReader
{
    public class AccountNumber : IAccountNumber
    {
        private readonly SevenSegment[] _accountNumberEncoded = new SevenSegment[9];
        
        public AccountNumber(string threeLines)
        {
            InitAccountNumberEncoded(threeLines);
        }
        
        public AccountNumber(SevenSegment[] digits)
        {
            _accountNumberEncoded = digits;
        }

        private void InitAccountNumberEncoded(string threeLines)
        {
            var split = threeLines.Split("\n");
            if (split.Length != 3)
                throw new Exception("Bad input");
            for (var index = 0; index < _accountNumberEncoded.Length; index++)
            {
                _accountNumberEncoded[index] = new SevenSegment();
            }
            LineAccumulator(split[0], SevenSegment.UpperLineEncoding);
            LineAccumulator(split[1], SevenSegment.MiddleLineEncoding);
            LineAccumulator(split[2], SevenSegment.BottomLineEncoding);
        }

        private void LineAccumulator(string singleLine, int[] lineEncoding)
        {
            var accountNumberSegmentPointer = 0;
            for (int i = 0, j = 0; i < singleLine.Length; i++)
            {
                if (singleLine[i] != ' ')
                    _accountNumberEncoded[accountNumberSegmentPointer].AccumulateSegment(lineEncoding[j]);
                if (j != 2)
                {
                    ++j;
                    continue;
                }
                j = 0;
                ++accountNumberSegmentPointer;
            }
        }
        
        private bool Valid()
        {
            return !ToString().Contains("?") && CheckSum(_accountNumberEncoded);
        }

        private static bool CheckSum(SevenSegment[] number)
        {
            return Enumerable.Range(1, number.Length).Aggregate((int?)0, 
                (sum, position) => 
                    sum + position * number[number.Length - position].ToInteger()) % 11 == 0;
        }
        
        private AccountNumber[] AmbiguousValidAccountNumbers()
        {
            List<AccountNumber> validVariations = new List<AccountNumber>();
            for (int i = 0; i < _accountNumberEncoded.Length; i++)
            {
                var digitVariations = _accountNumberEncoded[i].OneCharacterDifferenceNeighbours();
                foreach (var digitVariation in digitVariations)
                {
                    var copy = (SevenSegment[]) _accountNumberEncoded.Clone();
                    copy[i] = new SevenSegment(digitVariation);
                    var accountVariationCandidate = new AccountNumber(copy);
                    if (accountVariationCandidate.Valid())
                        validVariations.Add(accountVariationCandidate);
                }
            }
            return validVariations.ToArray();
        }
        
        public override string ToString()
        {
            var accumulated = new StringBuilder();
            foreach (var digit in _accountNumberEncoded)
                accumulated.Append(digit);
            return accumulated.ToString();
        }

        public string ToStringWithStatus()
        {
            var asString = ToString();
            if (Valid())
                return asString;
            var allValidVariationsAsString = AmbiguousValidAccountNumbers()
                .Select(accountNumber => $"'{accountNumber}'")
                .ToArray();
            
            return allValidVariationsAsString.Length switch
            {
                0 when asString.Contains('?') => $"{asString} ILL",
                0 => $"{asString} ERR",
                1 => allValidVariationsAsString.First().Trim('\''),
                _ => $"{asString} AMB [{string.Join(", ", allValidVariationsAsString)}]"
            };
        }
    }
}