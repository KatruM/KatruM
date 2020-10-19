using System;
using System.Globalization;

namespace ClinicalScoring.BundleParsers
{
    [Serializable]
    public class ScoreValue
    {
        public ScoreType ScoreType;
        public double Value;
        public Range Range;
        public string StringValue;

        public string DisplayValue
        {
            get
            {
                if (!string.IsNullOrEmpty(StringValue))
                    return StringValue.Trim();
                return Value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}