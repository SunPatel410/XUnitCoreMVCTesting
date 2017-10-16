using System;
using System.Globalization;
using System.Linq;
using CreditCards.Core.Interfaces;

namespace CreditCards.Core.Model
{
    /// <summary>
    /// A frequent flyer number consist of 2 parts separated by a '-':
    /// [member number]-[scheme identifer]
    /// Member numbers consist of 6 numeric digits
    /// Scheme identifiers are a single uppercase alphabetic character
    /// </summary>
    public class FrequentFlyerNumberValidator : IFrequentFlyerNumberValidator
    {
        private readonly char[] _validSchemeIdentifiers = {'A', 'Q', 'Y'};
        private const int ExpectedTotalLength = 8;
        private const int ExpectedMemberNumberLength = 6;

        public bool IsValid(string frequentFlyerNumber)
        {
            if (frequentFlyerNumber == null)
            {
                throw new ArgumentNullException(nameof(frequentFlyerNumber));
            }

            if (frequentFlyerNumber.Length != ExpectedTotalLength)
            {
                return false;
            }

            var memberNumberPart = frequentFlyerNumber.Substring(0, ExpectedMemberNumberLength);

            //int _ c# 7 - dont need to decalare the output parameter for the tryparse method and not going to use the return value
            if (!int.TryParse(memberNumberPart, NumberStyles.None, null, out int _))
            {
                return false;
            }

            var schemaIdentifier = frequentFlyerNumber.Last();
            return _validSchemeIdentifiers.Contains(schemaIdentifier);

        }
    }
}
