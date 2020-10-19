using System;
using System.Collections.Generic;

namespace ClinicalScoring.BundleParsers
{
    [Serializable]
    public class Bdi3ScoringConfig
    {
        public List<Range> AgeRanges=new List<Range>();
        public List<Range> FileRanges=new List<Range>();
        
    }
}