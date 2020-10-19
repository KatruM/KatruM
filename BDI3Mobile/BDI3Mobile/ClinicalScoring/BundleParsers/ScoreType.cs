using System;

namespace ClinicalScoring.BundleParsers
{
    public enum ScoreType
    {
        None,
        RawScore,
        ScaleScore,
        SumScaleScore,
        StandardAgeScore,
        DevelopmentalQuotient,
        Dq95PctConfidenceInterval,
        Dq90PctConfidenceInterval,
        AgeInMonths,
        PercentileRank,
        ChangeSensitiveScore,
        ChangeSensitiveScore90,
        ChangeSensitiveScorePr50,
        NCE,
        WDiff,
        RPI,
        Item,
        CSS_0_5,
        W,
        CSS_1_5,
        AgeEquivalent,
        ZScore,
        TScore,
        Multiple,
        Unknown
        
    }
    public static class ScoreTypeEnumHelper
    {
        public static ScoreType GetScoreType(this string root)
        {
            if (root.Equals("Sum of Scale Scores",StringComparison.OrdinalIgnoreCase))
                return ScoreType.SumScaleScore;
            if (root.Equals("Standard Age Score",StringComparison.OrdinalIgnoreCase))
                return ScoreType.StandardAgeScore;
            if (root.Equals("DQ 95% Confidence Interval",StringComparison.OrdinalIgnoreCase))
                return ScoreType.Dq95PctConfidenceInterval;
            if (root.Equals("DQ 90% Confidence Interval",StringComparison.OrdinalIgnoreCase))
                return ScoreType.Dq90PctConfidenceInterval;
            if (root.Equals("Percentile Rank",StringComparison.OrdinalIgnoreCase))
                return ScoreType.PercentileRank;
            if (root.Equals("Raw Score", StringComparison.OrdinalIgnoreCase))
                return ScoreType.RawScore;
            if (root.StartsWith("Scale Score", StringComparison.OrdinalIgnoreCase))
                return ScoreType.ScaleScore;
            if (root.Equals("Developmental Quotient", StringComparison.OrdinalIgnoreCase))
                return ScoreType.DevelopmentalQuotient;
            if (root.Equals("Change Sensitive Score", StringComparison.OrdinalIgnoreCase))
                return ScoreType.ChangeSensitiveScore;
            if (root.Equals("90% CI"))
                return ScoreType.ChangeSensitiveScore90;
            if (root.Equals("Change Sensitive Score PR50", StringComparison.OrdinalIgnoreCase))
                return ScoreType.ChangeSensitiveScorePr50;
            if (root.Equals("Age In Months", StringComparison.OrdinalIgnoreCase))
                return ScoreType.AgeInMonths;
            if (root.Equals("WDiff", StringComparison.OrdinalIgnoreCase))
                return ScoreType.WDiff;
            if (root.Equals("RPI", StringComparison.OrdinalIgnoreCase))
                return ScoreType.RPI;
            if (root.Equals("Age Equivalent", StringComparison.OrdinalIgnoreCase))
                return ScoreType.AgeEquivalent;
            if (root.Equals("NCE", StringComparison.OrdinalIgnoreCase))
                return ScoreType.NCE;
            if (root.Equals("Item"))
                return ScoreType.Item;
            if (root.Equals("None"))
                return ScoreType.None;
            if (root.Equals("0.5 CSS"))
                return ScoreType.CSS_0_5;
            if (root.Equals("T-Score"))
                return ScoreType.TScore;
            if (root.Equals("Z-Score"))
                return ScoreType.ZScore;
            if (root.Equals("CSS"))
                return ScoreType.ChangeSensitiveScore;
            if(root.Equals("1.5 CSS"))
                return ScoreType.CSS_1_5;
            if (root.Equals("Multiple"))
                return ScoreType.Multiple;
            return ScoreType.Unknown;
        }

        public static string DisplayName(this ScoreType scoreType)
        {
            switch (scoreType)
            {
                case ScoreType.SumScaleScore:
                    return "Sum of Scale Scores";
                case ScoreType.ScaleScore:
                    return "Scale Score";
                case ScoreType.StandardAgeScore:
                    return "Standard Age Score";
                case ScoreType.DevelopmentalQuotient:
                    return "Developmental Quotient";
                case ScoreType.Dq95PctConfidenceInterval:
                    return "DQ 95% Confidence Interval";
                case ScoreType.Dq90PctConfidenceInterval:
                    return "DQ 90% Confidence Interval";
                case ScoreType.PercentileRank:
                    return "Percentile Rank";
                case ScoreType.RawScore:
                    return "Raw Score";
                case ScoreType.ChangeSensitiveScore:
                    return "Change Sensitive Score";
                case ScoreType.ChangeSensitiveScorePr50:
                    return "Change Sensitive Score PR 50";
                case ScoreType.ChangeSensitiveScore90:
                    return "Change Sensitive Score 90% CI";
                case ScoreType.AgeInMonths:
                    return "Age In Months";
                case ScoreType.WDiff:
                    return "WDiff";
                case ScoreType.RPI:
                    return "RPI";
                case ScoreType.AgeEquivalent:
                    return "Age Equivalent";
                case ScoreType.NCE:
                    return "NCE";
                case ScoreType.None:
                    return "None";
                case ScoreType.Item:
                    return "Item";
                case ScoreType.CSS_0_5:
                    return "0.5 CSS";
                case ScoreType.CSS_1_5:
                    return "1.5 CSS";
                case ScoreType.W:
                    return "W Score";
                case ScoreType.ZScore:
                    return "Z-Score";
                case ScoreType.TScore:
                    return "T-Score";
                case ScoreType.Unknown:
                    return "Unknown";
                case ScoreType.Multiple:
                    return "Multiple";
                default:
                    throw new ArgumentOutOfRangeException(nameof(scoreType), scoreType, null);
            }
        }
    }
    
}