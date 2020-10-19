using System;

namespace ClinicalScoring.BundleParsers
{
    [Serializable]
    public class ScoreLookup : IEquatable<ScoreLookup>
    {
        public readonly ScoreType ScoreType;
        public readonly Range AgeRange;
        public readonly string Domain;
        public readonly string SubDomain;
        
        public readonly double ScoreValue;
        public readonly int ScoreModifier;
        public readonly ScoreType ScoreLookupType;
        public readonly Range ScoreRange;

        public ScoreLookup(ScoreType scoreType, Range ageRange, string domain, string subDomain, double scoreValue, int scoreModifier, ScoreType scoreLookupType)
        {
            ScoreType = scoreType;
            AgeRange = ageRange;
            Domain = KnownMismatchFixer.FixContent(domain);
            SubDomain = KnownMismatchFixer.FixContent(subDomain);
            ScoreValue = scoreValue;
            ScoreModifier = scoreModifier;
            ScoreLookupType = scoreLookupType;
        }
        public ScoreLookup(ScoreType scoreType, Range ageRange, string domain, string subDomain, double scoreValue, int scoreModifier, ScoreType scoreLookupType, Range scoreRange) 
        : this(scoreType, ageRange, domain, subDomain, scoreValue, scoreModifier, scoreLookupType)
        {
            ScoreRange = scoreRange;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ScoreLookup) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) ScoreType;
                hashCode = (hashCode * 397) ^ (AgeRange != null ? AgeRange.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Domain != null ? Domain.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SubDomain != null ? SubDomain.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ScoreRange != null ? ScoreRange.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ScoreValue.GetHashCode();
                hashCode = (hashCode * 397) ^ ScoreModifier.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)ScoreLookupType;
                return hashCode;
            }
        }

        public static bool operator ==(ScoreLookup left, ScoreLookup right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ScoreLookup left, ScoreLookup right)
        {
            return !Equals(left, right);
        }

        public bool Equals(ScoreLookup other)
        {
            if (other == null)
                return false;
            return ScoreType == other.ScoreType &&
                   Domain == other.Domain &&
                   SubDomain == other.SubDomain &&
                   ScoreModifier == other.ScoreModifier && 
                   Math.Abs(ScoreValue - other.ScoreValue) < 0.001 &&
                   ScoreRange == other.ScoreRange &&
                   AgeRange == other.AgeRange &&
                   ScoreLookupType == other.ScoreLookupType
                ;
        }

        public bool Match(double rawScore, int ageInMonths, string content, ScoreType scoreType)
        {
            if (Domain != content || SubDomain != content)
                return false;
            if (ScoreLookupType != scoreType)
                return false;
            if (!AgeRange.Contains(ageInMonths))
                return false;
            return MatchScore(rawScore);
        }
        public bool Match(double rs, string domain, string subDomain, ScoreType scoreType)
        {
            if (domain!=null && domain != this.Domain)
                return false;
            if (subDomain != this.SubDomain)
                return false;
            if (ScoreLookupType != scoreType)
                return false;
            return MatchScore(rs);
        }

        public bool MatchScore(double rs)
        {
            if (ScoreRange != null)
            {
                return ScoreRange.Contains((int)rs);
            }
            if (Math.Abs(ScoreValue - rs) < 0.001)
                return true;
            if (ScoreModifier > 0 && rs > ScoreValue)
                return true;
            if (ScoreModifier < 0 && rs < ScoreValue)
                return true;
            return false;
        }
    }
    
    
}