using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClinicalScoring.BundleParsers
{
    [Serializable]
    public class Bdi3ScoringParser : ScoringParser
    {
        private static readonly Regex _ageParser = new Regex("^([0-9]+)\\D*([0-9]+)", RegexOptions.Compiled);
        private static readonly Regex _simpleRangeParser = new Regex("([0-9]+)\\D*([0-9]+)", RegexOptions.Compiled);
        private static readonly Regex _rangeInParenthesisParser = new Regex(@"\(([0-9]+)\D*([0-9]+)\)", RegexOptions.Compiled);
        private static readonly Regex _simpleNumberFinder=new Regex(@"^\D*(\d+)\D*.*$",RegexOptions.Compiled);
        private static readonly Regex _parenRemover=new Regex(@"^\(([^\)]*)\).*$", RegexOptions.Compiled);
        private static readonly Regex _contentAbbrevParser = new Regex(@"^\(([^\)]*)\).*$", RegexOptions.Compiled);
        private static readonly Dictionary<string, int> MaxScoresBySubdomain = new Dictionary<string, int>();
        public readonly Dictionary<string, string> ContentAbbreviationLookup = new Dictionary<string, string>();
        public readonly List<Bdi3ScoringConfig> ScoringConfigs = new List<Bdi3ScoringConfig>();
        private static Dictionary<string, Dictionary<Range, int>> _screenerCutScores=new Dictionary<string, Dictionary<Range, int>>();
        private static Bdi3ScoringParser _instance;
        static Bdi3ScoringParser()
        {
            FillScreenerCutScores();
        }


        private Bdi3ScoringParser()
        {
#if SERIALIZED_BUILD
#else
            Task.Run(async () => await LoadDataAsync()).Wait();
#endif
        }

        public static Bdi3ScoringParser GetInstance()
        {
            if (_instance != null)
                return _instance;
#if SERIALIZED_BUILD
            var fs = Assembly.GetExecutingAssembly().GetManifestResourceStream($"ClinicalScoring.ScoringBundles.parserSerialized.bin");
            IFormatter formatter = new BinaryFormatter();
            _instance = (Bdi3ScoringParser)formatter.Deserialize(fs);
            return _instance;
#else
            _instance = new Bdi3ScoringParser()
#endif

        }
        protected override string[] BundleNames => new [] { "Bdi3ScoringTables.zip" };

#if SERIALIZED_BUILD
#else
        
        private void ParseSheetKeyVal(string key, ScoreType scoreType = ScoreType.None,
            ScoreType toScoreType = ScoreType.None)
        {
            var contents = SheetContents[key];
            var dataSheetKey = "Sheet1";
            if (contents.Keys.Count == 1)
                dataSheetKey = contents.Keys.First();
            var dataSheet = contents[dataSheetKey];
            foreach (var rowIdx in dataSheet.Keys)
            {
                var row = dataSheet[rowIdx];

                var cellValue = row[0];
                var modifier = 0;
                if (cellValue.StartsWith("GTEQ"))
                {
                    cellValue = cellValue.Substring("GTEQ".Length).Trim();
                    modifier = 1;
                } else if (cellValue.StartsWith("LTEQ"))
                {
                    cellValue = cellValue.Substring("LTEQ".Length).Trim();
                    modifier = -1;
                }
                if (!double.TryParse(cellValue, out var result))
                    continue;
                var sl = new ScoreLookup(scoreType, null, null, null, result, modifier, toScoreType);
                var scores=GetOrCreateLookup(sl);
                double.TryParse(row[1], out var scoreValue);
                scores.Add(new ScoreValue {StringValue = row[1], Value = scoreValue, ScoreType = toScoreType});
            }
            
        }

        private void ParseSsDqNce(string key)
        {
            var contents = SheetContents[key];
            var dataSheetKey = "Final";
            if (contents.Keys.Count == 1)
                dataSheetKey = contents.Keys.First();
            var dataSheet = contents[dataSheetKey];
            var scoreModifier = 1;
            foreach (var rowIdx in dataSheet.Keys)
            {
                if (rowIdx < 4)
                    continue;
                var row = dataSheet[rowIdx];
                var dq = row[1];
                var pr = row[2];
                var nce = row[3];
                var ss = row[4];
                var zScore = row[5];
                var tScore = row[6];

                double.TryParse(ss, out var realSs);
                double.TryParse(dq, out var realDq);
                double.TryParse(zScore, out var realZ);
                double.TryParse(tScore, out var realT);
                
                var dqLookup = new ScoreLookup(ScoreType.DevelopmentalQuotient, null, null, null, realDq, scoreModifier, ScoreType.Multiple);
                var scoresDq = GetOrCreateLookup(dqLookup);
                var scoreDestinations = new[] {scoresDq};
                if (!string.IsNullOrEmpty(ss))
                {
                    var sl=new ScoreLookup(ScoreType.ScaleScore, null, null, null, realSs, 0, ScoreType.Multiple);
                    var scoresSs = GetOrCreateLookup(sl);
                    scoreDestinations = new[] {scoresSs, scoresDq};
                }
                scoreModifier = 0;
                foreach (var scores in scoreDestinations)
                {
                    scores.Add(new ScoreValue
                    {
                        Value = realDq,
                        ScoreType = ScoreType.DevelopmentalQuotient,
                        StringValue = dq
                    });
                    scores.Add(new ScoreValue
                    {
                        Value = realSs,
                        ScoreType = ScoreType.ScaleScore,
                        StringValue = ss
                    });
                    scores.Add(new ScoreValue
                    {
                        ScoreType = ScoreType.NCE,
                        StringValue = nce
                    });
                    scores.Add(new ScoreValue
                    {
                        Value = realZ,
                        ScoreType = ScoreType.ZScore,
                        StringValue = zScore
                    });
                    scores.Add(new ScoreValue
                    {
                        Value = realT,
                        ScoreType = ScoreType.TScore,
                        StringValue = tScore
                    });
                    
                }
            }
        }
        private void ParseRsToAe(string key)
        {
            var contents = SheetContents[key];
            var dataSheetKey = contents.Keys.First();
            var dataSheet = contents[dataSheetKey];
            var maxCols = dataSheet.Values.Max(t => t.Count);
            var domains=new string[maxCols];
            var subDomains=new string[maxCols];
            var abbreviations = new string[maxCols];
            foreach (var rowIdx in dataSheet.Keys)
            {
                var row = dataSheet[rowIdx];
                var lastVal = row[0];
                if (rowIdx == 3 || row[0] == "Age Equivalent")
                {
                    ForEachCell(row, (cx, s) =>
                    {
                        domains[cx] = s;
                        if (string.IsNullOrEmpty(s))
                            domains[cx] = lastVal;
                        else
                            lastVal = s;
                    });
                }

                if (rowIdx == 4 || row[0] == "Year-month")
                {
                    ForEachCell(row, (i, s) =>
                    {
                        subDomains[i] = KnownMismatchFixer.FixContent(s.Trim());
                    });
                }
                if (rowIdx == 5)
                {
                    ForEachCell(row, (i, s) => abbreviations[i] = _parenRemover.Replace(s, "$1"));
                }
                if (rowIdx >= 6)
                {
                    var ageInMonths = 0;
                    ForEachCell(row, (idx, data) =>
                    {
                        if (string.IsNullOrEmpty(data?.Trim()) || idx==0)
                            return;
                        if (idx == 1)
                            ageInMonths = int.Parse(data);
                        else if (idx <= 15)
                        {
                            var modifier = 0;
                            if (data.StartsWith(">"))
                            {
                                modifier = 1;
                                data = data.Substring(1);
                            }

                            int[] values = null;
                            try
                            {
                                #if NETSTANDARD2_0
                                values = data.Split(new []{ '-' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(t => int.Parse(t)).ToArray();
                                #else
                                values = data.Split('-', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(t => int.Parse(t)).ToArray();
                                #endif
                            }
                            catch (Exception e)
                            {
                                //logger?.LogError(e, "Error parsing date spread");
                            }

                            var minV = values[0];
                            var maxV = values.Max();
                            for (var val = minV; val <= maxV; val++)
                            {
                                var sl = new ScoreLookup(ScoreType.RawScore,
                                    null, domains[idx], subDomains[idx],
                                    val, modifier, ScoreType.AgeEquivalent);
                                var scores = GetOrCreateLookup(sl);
                            
                                scores.Add(new ScoreValue
                                {
                                    ScoreType = ScoreType.AgeEquivalent,
                                    Value = ageInMonths
                                });
                                
                            }
                        }
                    });
                }
            }
        }
#endif
        
        public List<ScoreValue> GetNonScoreLookups(double? ss, ScoreType scoreType = ScoreType.ScaleScore)
        {
            var scores = new List<ScoreValue>();
            if (!ss.HasValue)
            {
                return scores;
            }
            var lookups = NonContentLookups.Where(t => t.SubDomain == null
                                                       && t.ScoreType == scoreType
                                                       && t.ScoreLookupType == ScoreType.Multiple
                                                       && t.MatchScore(ss.Value));
            foreach (var scoreLookup in lookups)
            {
                scores.AddRange(ScoreLookup[scoreLookup]);
            }
            return scores;
            
        }
        public List<ScoreValue> GetPrFromSs(double ss)
        {
            var lookups = NonContentLookups.Where(t => t.SubDomain == null
                                                      && t.ScoreType == ScoreType.ScaleScore
                                                      && t.ScoreLookupType == ScoreType.PercentileRank
                                                      && t.MatchScore(ss));
            var scores = new List<ScoreValue>();
            foreach (var scoreLookup in lookups)
            {
                scores.AddRange(ScoreLookup[scoreLookup]);
            }
            return scores;
            
        }

        public string GetRdiFromWdiff(double wDiff)
        {
            var lookups = NonContentLookups.Where(t => t.SubDomain == null
                                                       && t.ScoreType == ScoreType.WDiff
                                                       && t.ScoreLookupType == ScoreType.RPI
                                                       && t.MatchScore(wDiff));
            var scores = new List<ScoreValue>();
            foreach (var scoreLookup in lookups)
            {
                scores.AddRange(ScoreLookup[scoreLookup]);
            }
            return scores.FirstOrDefault()?.StringValue;
            
        }
        public Dictionary<string, int> GetMaxScoresByContent()
        {
            return MaxScoresBySubdomain;
        }
        public int GetMaxScoreForContent(string content)
        {
            if (MaxScoresBySubdomain.ContainsKey(content))
                return MaxScoresBySubdomain[content];
            else
            {
                //logger?.LogError($"Cannot find max value for content - {content}");
                return NO_SCORE;
            }
        }

        public List<ScoreValue> GetScores(double rawScore, string subDomain, ScoreType? scoresOfType, int? ageInMonths=null, ScoreType? lookupScoreType=null)
        {
            Func<ScoreLookup, bool> lookupPredicate = t => t.SubDomain == subDomain
                                                     && (!ageInMonths.HasValue ||
                                                         (t.AgeRange != null && t.AgeRange.Contains((int) ageInMonths))
                                                     )
                                                     && (!lookupScoreType.HasValue ||
                                                         t.ScoreType == lookupScoreType.Value
                                                         )
                                                     && t.MatchScore(rawScore);
            IEnumerable<ScoreLookup> lookups;
            if (ScoreLookupsByContent.ContainsKey(subDomain))
            {
                lookups = ScoreLookupsByContent[subDomain].Where(lookupPredicate);
            }
            else
            {
                lookups = ScoreLookup.Keys.Where(lookupPredicate);
            }

            var scores = new List<ScoreValue>();
            foreach (var scoreLookup in lookups)
            {
                scores.AddRange(ScoreLookup[scoreLookup]);
            }
            if(scoresOfType.HasValue)
                return scores.Where(t=>t.ScoreType == scoresOfType.Value).ToList();
            return scores.ToList();
        }
        
        public double? GetSsFromRs(double rs, string subDomain, int ageInMonths)
        {
            var scores = GetScores(rs, subDomain, ScoreType.ScaleScore, ageInMonths);
            if (scores.Count > 0)
            {
                return scores.First().Value;
            }
            var lookup = ScoreLookup.Keys.FirstOrDefault(t => t.Match(rs, ageInMonths, subDomain, ScoreType.ScaleScore));
            if (lookup == null)
                return NO_SCORE;
            var matches = ScoreLookup[lookup];
            var ssScore = matches.FirstOrDefault(t => t.ScoreType == ScoreType.ScaleScore);
            return ssScore?.Value;
        }
        
        public int GetAeFromRs(double rs, string domain, string subDomain)
        {
            var scores = GetScores(rs, subDomain, ScoreType.AgeEquivalent);
            if (scores.Count > 0)
            {
                return (int)scores.First().Value;
            }
            
            var s=ScoreLookup.Keys.FirstOrDefault(t => t.Match(rs, domain, subDomain, ScoreType.AgeEquivalent));
            if (s == null)
                return NO_SCORE;
            scores = ScoreLookup[s];
            return (int)scores.First().Value;

        }
        private void ParseItemStats(string key)
        {
            var contents = SheetContents[key];
            var dataSheetKey = contents.Keys.First();
            var dataSheet = contents[dataSheetKey];
            var contentAbbrev = dataSheetKey;
            if (key.StartsWith("BDI3_"))
            {
                contentAbbrev = key.Substring("BDI3_".Length);
                contentAbbrev = contentAbbrev.Substring(0, contentAbbrev.IndexOf("_", StringComparison.Ordinal));
            }
            foreach (var idx in dataSheet.Keys)
            {
                var row = dataSheet[idx];
                if (!int.TryParse(row[0], out var itemNo))
                    continue;
                var subDomain = ContentAbbreviationLookup[contentAbbrev];
                var sl = new ScoreLookup(ScoreType.Item, null, null, subDomain, itemNo, 0, ScoreType.Item);
                var scores = GetOrCreateLookup(sl);
                scores.Add(new ScoreValue { ScoreType = ScoreType.CSS_0_5, Value = double.Parse(row[1])});
                scores.Add(new ScoreValue { ScoreType = ScoreType.ChangeSensitiveScore, Value = double.Parse(row[2])});
                scores.Add(new ScoreValue { ScoreType = ScoreType.CSS_1_5, Value = double.Parse(row[3])});
            }            
        }
#if SERIALIZED_BUILD
#else
        
        private void ParseSsPrToCssByAge(string key)
        {
            var contents = SheetContents[key];
            var dataSheetKey = contents.Keys.First();
            var dataSheet = contents[dataSheetKey];
            var contentAbbrev = dataSheetKey;
            if (key.StartsWith("BDI3_"))
            {
                contentAbbrev = key.Substring("BDI3_".Length);
                contentAbbrev = contentAbbrev.Substring(0, contentAbbrev.IndexOf("_", StringComparison.Ordinal));
            }
            //layout seems mainly fixed...
            var ssRow = dataSheet[1];
            var ssValues = new double[ssRow.Count];
            for (var i = ssRow.Count - 1; i >= 1; i--)
            {
                if (string.IsNullOrEmpty(ssRow[i]))
                    continue;
                if (!double.TryParse(_simpleNumberFinder.Replace(ssRow[i], "$1"), out var _s))
                {
                    //logger?.LogWarning($"Could not parse {ssRow[i]}");
                }
                ssValues[i] = _s;
            }
            var prRow = dataSheet[2];
            var prValues = new double[prRow.Count];
            for (var i = prRow.Count - 1; i >= 1; i--)
            {
                if (string.IsNullOrEmpty(prRow[i]))
                    continue;
                if (!double.TryParse(_simpleNumberFinder.Replace(prRow[i], "$1"), out var _s))
                {
                    //logger?.LogWarning($"Could not parse {prRow[i]}");
                }
                prValues[i] = _s;
            }
            //load data
            for (var rx = 3; rx <= dataSheet.Count; rx++)
            {
                var row = dataSheet[rx];
                var ageText = row[0];
                var match = _rangeInParenthesisParser.Match(ageText);
                var ageRange=new Range();
                if (match.Groups.Count == 3)
                {
                    ageRange.Start = int.Parse(match.Groups[1].Value);
                    ageRange.End = int.Parse(match.Groups[2].Value);
                    ageRange.Definition = $"({ageRange.Start} - {ageRange.End})";
                }
                else
                {
                    var ageInMonths = _simpleNumberFinder.Replace(ageText, "$1");
                    if (int.TryParse(ageInMonths, out var _age))
                    {
                        ageRange.Start = _age;
                        ageRange.End = _age;
                        ageRange.Definition = $"({ageRange.Start} - {ageRange.End})";
                    }
                    else
                    {
                       // logger?.LogWarning($"Could not determine age range from {ageText}");
                    }

                }

                var lastSS = 0;
                for (var cx = 1; cx < row.Count; cx++)
                {
                    if (string.IsNullOrEmpty(row[cx]))
                        continue;
                    if (!int.TryParse(row[cx], out var css))
                        continue;
                    var ss = ssValues[cx];
                    var pr = prValues[cx];
                    var subDomain = ContentAbbreviationLookup[contentAbbrev];
                    var sl = new ScoreLookup(ScoreType.PercentileRank, ageRange, null, subDomain, pr, 0, ScoreType.ChangeSensitiveScore);
                    var scores=GetOrCreateLookup(sl);
                    scores.Add(new ScoreValue { Value = css, ScoreType = ScoreType.ChangeSensitiveScore});
                    //scores.Add(new ScoreValue { Value = pr, ScoreType = ScoreType.PercentileRank});
                    
                }
            }
        }
        private void ParseSheetWithLayout(string key, ScoreType scoreType = ScoreType.None,
            ScoreType toScoreType = ScoreType.None)
        {
            var contents = SheetContents[key];
            var layoutSheet = contents["Layout"];
            var dataSheet = contents[contents.Keys.First(t => t!="Layout")];
            var contentLookup = new Dictionary<int, ScoreLookup>();
            var col = 0;
            foreach (var rowIndex in layoutSheet.Keys)
            {
                var row = layoutSheet[rowIndex];
                if (row[0] == "Header")
                    continue;
                var content = row[0];
                if (content.Trim().ToLower().Equals("raw score") && scoreType!=ScoreType.RawScore)
                    scoreType = ScoreType.RawScore;
                var destScoreType = toScoreType;
                if (content.Contains("CSS"))
                    destScoreType = ScoreType.ChangeSensitiveScore;
                else if (content.Contains("90% CI"))
                    destScoreType = ScoreType.ChangeSensitiveScore90;
                else if(col > 0)
                {
                    Console.WriteLine($"Cannot determine score type {content}");
                }
                var subDomainAbbrev = _contentAbbrevParser.Replace(content, "$1");
                if (subDomainAbbrev.ToLower().Contains("raw score"))
                {
                    contentLookup[col++] = new ScoreLookup(scoreType, null, null, subDomainAbbrev, 0,
                        0, destScoreType);
                    
                }                
                else if (!ContentAbbreviationLookup.ContainsKey(subDomainAbbrev))
                {
                    //logger?.LogError($"Cannot find content abbreviation for: {subDomainAbbrev}");
                }
                else
                {
                    var realSubDomain = ContentAbbreviationLookup[subDomainAbbrev];
                    contentLookup[col++] = new ScoreLookup(scoreType, null, null, realSubDomain, 0,
                        0, destScoreType);
                }
            }

            foreach (var dataRowIdx in dataSheet.Keys)
            {
                var row = dataSheet[dataRowIdx];
                //is the header set of rows
                if (row.Count < 2)
                    continue;
                //raw score first
                double.TryParse(row[0], out var rawScore);
                var type = scoreType==ScoreType.None ? contentLookup[0].SubDomain.Trim().GetScoreType() : scoreType;
                Range ageRange = null;
                if (type == ScoreType.AgeInMonths)
                {
                    ageRange = new Range {Start = 0, End = 0};
                    var match = _rangeInParenthesisParser.Match(row[0]);
                    if (match.Groups.Count == 3)
                    {
                        ageRange.Start = int.Parse(match.Groups[1].Value);
                        ageRange.End = int.Parse(match.Groups[2].Value);
                    }
                    else
                    {
                        //logger?.LogWarning($"Cannot determine range from {row[0]} ");
                    }
                    
                }
                
                for (var c = 1; c < row.Count; c++)
                {
                    var value = row[c];
                    if (string.IsNullOrEmpty(value))
                        continue;
                    if (row[0].ToLower().Contains("raw score"))
                    {
                        // this is a header
                        break;
                    }

                    var content = contentLookup[c];
                    if(value.Contains($"({content.SubDomain})") || value.Contains($"{content.SubDomain}"))
                        continue;
                    var range = ParseRange(value);
                    double.TryParse(value, out var tempScore);
                    
                    var sl = new ScoreLookup(content.ScoreType, ageRange, content.Domain, content.SubDomain, rawScore, 0,
                        content.ScoreLookupType);
                    
                    var currentLookup = GetOrCreateLookup(sl);
                    
                    currentLookup.Add(new ScoreValue
                        {
                            ScoreType = content.ScoreLookupType,
                            Value = tempScore,
                            StringValue = value,
                            Range = range
                            
                        }
                    );
                }
            }
        }

        private void ParseSpecificSheet(string key)
        {
            if (key.Contains("RS_CSS"))
            {
                //RS to CSS 
                ParseSheetWithLayout(key, ScoreType.None, ScoreType.ChangeSensitiveScore);
            }
            else if (key.Contains("CSS_PR50_by_Age"))
            {
                ParseSheetWithLayout(key, ScoreType.None, ScoreType.ChangeSensitiveScorePr50);
            }
            else if (key.Contains("Wdiff_to_RPI"))
            {
                ParseSheetKeyVal(key, ScoreType.WDiff, ScoreType.RPI);
            }
            else if (key.Contains("ss_pr"))
            {
                ParseSheetKeyVal(key, ScoreType.ScaleScore, ScoreType.PercentileRank);
            }
            else if (key.Contains("CSS_SS_PR_by_AgeMonths"))
            {
                ParseSsPrToCssByAge(key);
            }
            else if (key.Contains("Item_Stats"))
            {
                ParseItemStats(key);
            }
            else if (key.Contains("RS_AE"))
            {
                ParseRsToAe(key);
            }
            else if (key.Contains("item_label"))
            {
                //logger?.LogInformation($"Not handling item label sheet {key} ");
            }
            else if (key.Contains("Screener_Cut"))
            {
                //logger?.LogInformation($"Not handling screener cut scores sheet {key} ");
            }
            else if (key.Contains("SSDQ_to_NCE"))
            {
                ParseSsDqNce(key);
            }
            else
            {
                //logger?.LogCritical($"File not handled! {key}");
            }
        }
#endif
        protected override async Task ParseDoneAsync()
        {
#if SERIALIZED_BUILD
            
#else
            var keys= SheetContents.Keys.ToList();
            keys.Sort((s1, s2) =>
            {
                if (s1.Contains("_Layout") && !s2.Contains("_Layout"))
                    return -1;
                if (s2.Contains("_Layout") && !s1.Contains("_Layout"))
                    return 1;
                return string.Compare(s1, s2, StringComparison.Ordinal);
            });
            // look at the spreadsheet data and handle the text files....
            foreach (var key in keys)
            {
                if (!key.Contains("_Layout"))
                {
                    ParseSpecificSheet(key);
                    continue;
                }

                var scoringConfig = new Bdi3ScoringConfig();
                var ageRanges = scoringConfig.AgeRanges;
                ScoringConfigs.Add(scoringConfig);

                var layoutWb = SheetContents[key];
                var fileSheetKey = layoutWb.Keys.FirstOrDefault(t => t.Contains("fileName") ||
                                                                     t.Contains("file_test") ||
                                                                     t.Contains("_files"));
                var layoutKey = layoutWb.Keys.FirstOrDefault(t => t.Contains("layout") ||
                                                                  t.Contains("_Layout"));
                List<Range> currentRanges = null;
                if (layoutKey != null) currentRanges = ParseLayoutSheet(layoutWb, layoutKey, scoringConfig);

                // 
                if (fileSheetKey != null) ParseFileSheet(layoutWb, fileSheetKey, ageRanges, currentRanges, key);
            }
#endif
        }

#if SERIALIZED_BUILD
#else
        private void ParseFileSheet(Dictionary<string, Dictionary<int, List<string>>> layoutWb, string fileSheetKey,
            List<Range> ageRanges, List<Range> fileRanges, string currentFileName)
        {
            var fileInfoSheet = layoutWb[fileSheetKey];
            //logger?.LogDebug($"FileInfoSheet: {fileInfoSheet}");
            foreach (var i in fileInfoSheet.Keys)
            {
                var row = fileInfoSheet[i];
                var fileName = row[0].ToUpperInvariant();
                if (!fileName.EndsWith(".TXT"))
                    continue;
                var subDomain = KnownMismatchFixer.FixContent(row[1]);
                var subContentPart = "";
                var ageRange = new Range {Start = 0, End = 0};
                if (row.Count > 2 && !string.IsNullOrEmpty(row[2]))
                {
                    var match = _ageParser.Match(row[2]);
                    try
                    {
                        ageRange.Start = int.Parse(match.Groups[1].Value);
                        ageRange.End = int.Parse(match.Groups[2].Value);
                        ageRange.Definition = $"({ageRange.Start} - {ageRange.End})";
                    }
                    catch (Exception e)
                    {
                        //logger?.LogError($"{row[2]} is unparsable as an age range");
                    }
 
                    ageRanges.Add(ageRange);
                }

                if (row.Count > 3) subContentPart = row[3];

                var contentAbbrev = "";
                var startIndex = 0;
                if (fileName.StartsWith("BDI3_A_"))
                    startIndex = "BDI3_A_".Length;
                else if (fileName.StartsWith("BDI3_")) startIndex = "BDI3_".Length;

                if (startIndex > 0)
                {
                    contentAbbrev = fileName.Substring(startIndex,
                        fileName.IndexOf("_", startIndex + 1, StringComparison.Ordinal) -
                        startIndex);
                    if (!string.IsNullOrEmpty(contentAbbrev))
                        ContentAbbreviationLookup[contentAbbrev] = KnownMismatchFixer.FixContent(subDomain);
                }

                if (!TextContents.ContainsKey(fileName))
                {
                    if (fileName.Contains("DUMMY"))
                    {
                        fileName = fileName.Replace("DUMMY", "FINAL");
                    }
                    else
                    {
                        fileName = fileName.Replace("FINAL", "DUMMY");
                    }

                    if (!TextContents.ContainsKey(fileName))
                    {
                        //logger?.LogError($"Cannot find file in text contents: {fileName}");
                        throw new FileNotFoundException(
                            $"Cannot find file in text contents: {fileName}");
                    }
                }

                var fileContents = TextContents[fileName.ToUpperInvariant()];
                #if NETSTANDARD2_0
                var lines = fileContents.Split('\n');
                #else
                var lines = fileContents.Split("\n");
                #endif
                var currentLookup = fileRanges.First();

                if (string.IsNullOrEmpty(subContentPart))
                {
                    subContentPart = ContentAbbreviationLookup[contentAbbrev];
                }
                var valueLookups = new Range[fileRanges.Count - 1];
                fileRanges.CopyTo(1, valueLookups, 0, valueLookups.Length);
                foreach (var line in lines)
                {
                    if (line.Length < currentLookup.End)
                        continue;
                    if (!double.TryParse(line.Substring(currentLookup.Start, currentLookup.Length), out var scoreValue))
                        continue;
                    var lookupScoreType = currentLookup.Definition.GetScoreType();
                    try
                    {
                        if (MaxScoresBySubdomain.ContainsKey(subDomain))
                        {
                            if (scoreValue > MaxScoresBySubdomain[subDomain])
                            {
                                MaxScoresBySubdomain[subDomain] = (int) scoreValue;
                            }
                        }
                        else
                        {
                            MaxScoresBySubdomain[subDomain] = (int) scoreValue;
                        }
                    }
                    catch (Exception e)
                    {
                        //logger?.LogError(e, "Max value parse");
                    }

                    
                    //if the first column value isn't a number this isn't a lookup or if this isn't long enough
                    for (var index = 0; index < valueLookups.Length; index++)
                    {
                        var lookup = valueLookups[index];
                        var sv = new ScoreValue();
                        sv.ScoreType = lookup.Definition.GetScoreType();
                        if (lookup.Definition.Contains("Age in Months"))
                        {
                            ageRange = new Range();
                            int.TryParse(_simpleNumberFinder.Replace(lookup.Definition, "$1"),
                                out var simpleAgeInMonths);
                            var rangeMatches = _rangeInParenthesisParser.Match(lookup.Definition);
                            if (simpleAgeInMonths > 0 && rangeMatches.Groups.Count == 3)
                            {
                                ageRange.Definition = rangeMatches.Groups[0].Value;
                                ageRange.Start = int.Parse(rangeMatches.Groups[1].Value);
                                if(index == valueLookups.Length - 1)
                                {
                                    ageRange.End = Int32.MaxValue;
                                }
                                else
                                {
                                    ageRange.End = int.Parse(rangeMatches.Groups[2].Value);
                                }
                            }
                            else
                            {
                                ageRange.Start = simpleAgeInMonths;
                                ageRange.End = simpleAgeInMonths;
                                ageRange.Definition = $"({ageRange.Start} - {ageRange.End})";
                            }
                        }

                        var sl = new ScoreLookup(
                            lookupScoreType, ageRange, subContentPart, 
                                      subDomain, scoreValue, 0, sv.ScoreType);
                        var scoreValues = GetOrCreateLookup(sl);
                        var valueString = line.Substring(lookup.Start, lookup.Length);
                        if(string.IsNullOrEmpty(valueString.Trim()))
                        {
                            continue;
                        }
                        if (!double.TryParse(valueString, out sv.Value))
                            sv.Value = NO_SCORE;
                        sv.StringValue = valueString;
                        scoreValues.Add(sv);
                    }
                }
            }
        }
        
#endif         
        public int GetScreenerCutScore(string stdDev, string domain, int ageInMonths)
        {
            var key = $"{domain.ToLower()}_{stdDev}";
            if (!_screenerCutScores.ContainsKey(key))
                return NO_SCORE;
            var lookup = _screenerCutScores[key];
            var range = lookup.Keys.FirstOrDefault(t => t.Contains(ageInMonths));
            if (range == null)
                return NO_SCORE;
            return lookup[range];
        }
        private Range ParseRange(string value, Range range=null)
        {
            if(range==null)
            {
                range = new Range();
            }
            var parseable = int.TryParse(value, out var intValue);
            if (parseable)
            {
                range.End = range.Start = intValue;
                return range;
            }
            var res = _simpleRangeParser.Match(value);
            if (res.Groups.Count <= 2)
            {
                //logger?.LogWarning($"Range was not parseable!: {value}");
                return range;
            }
            range.Start = int.Parse(res.Groups[1].Value);
            range.End = int.Parse(res.Groups[2].Value);

            return range;

        }
        private List<Range> ParseLayoutSheet(Dictionary<string, Dictionary<int, List<string>>> layoutWb,
            string layoutKey, Bdi3ScoringConfig scoringConfig)
        {
            var layoutSheet = layoutWb[layoutKey];
            var foundRanges = new List<Range>();
            foreach (var i in layoutSheet.Keys)
            {
                var row = layoutSheet[i];
                if (row.Count < 2 || string.IsNullOrEmpty(row[1]))
                    continue;
                var range = new Range();
                range.Definition = row[0];
                var parseable = int.TryParse(row[1], out range.Start);
                if (row.Count > 3)
                    int.TryParse(row[3], out range.End);
                if (!parseable)
                {
                    ParseRange(row[1], range);
                }
                // ranges are in as human readable, thankfully we're not human (0 index the world)
                range.Start -= 1;
                range.End -= 1;
                foundRanges.Add(range);
            }

            scoringConfig.FileRanges.AddRange(foundRanges);
            return foundRanges;
        }
        private static void FillScreenerCutScores()
        {
            #region screener cut score fill
            var contentAndStdDev = "total_-1.0";
            var currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 8,
                [new Range {Start = 6, End = 11}] = 28,
                [new Range {Start = 12, End = 17}] = 49,
                [new Range {Start = 18, End = 23}] = 61,
                [new Range {Start = 24, End = 35}] = 81,
                [new Range {Start = 36, End = 47}] = 99,
                [new Range {Start = 48, End = 59}] = 122,
                [new Range {Start = 60, End = 71}] = 145,
                [new Range {Start = 72, End = 83}] = 165,
                [new Range {Start = 84, End = 95}] = 185
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "total_-1.5";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 5,
                [new Range {Start = 6, End = 11}] = 24,
                [new Range {Start = 12, End = 17}] = 43,
                [new Range {Start = 18, End = 23}] = 58,
                [new Range {Start = 24, End = 35}] = 75,
                [new Range {Start = 36, End = 47}] = 90,
                [new Range {Start = 48, End = 59}] = 107,
                [new Range {Start = 60, End = 71}] = 127,
                [new Range {Start = 72, End = 83}] = 146,
                [new Range {Start = 84, End = 95}] = 172
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "total_-2.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 2,
                [new Range {Start = 6, End = 11}] = 21,
                [new Range {Start = 12, End = 17}] = 39,
                [new Range {Start = 18, End = 23}] = 52,
                [new Range {Start = 24, End = 35}] = 68,
                [new Range {Start = 36, End = 47}] = 85,
                [new Range {Start = 48, End = 59}] = 100,
                [new Range {Start = 60, End = 71}] = 115,
                [new Range {Start = 72, End = 83}] = 131,
                [new Range {Start = 84, End = 95}] = 154
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "cognitive_-1.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 1,
                [new Range {Start = 6, End = 11}] = 5,
                [new Range {Start = 12, End = 17}] = 10,
                [new Range {Start = 18, End = 23}] = 13,
                [new Range {Start = 24, End = 35}] = 16,
                [new Range {Start = 36, End = 47}] = 19,
                [new Range {Start = 48, End = 59}] = 23,
                [new Range {Start = 60, End = 71}] = 26,
                [new Range {Start = 72, End = 83}] = 31,
                [new Range {Start = 84, End = 95}] = 36
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "cognitive_-1.5";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 0,
                [new Range {Start = 6, End = 11}] = 4,
                [new Range {Start = 12, End = 17}] = 8,
                [new Range {Start = 18, End = 23}] = 11,
                [new Range {Start = 24, End = 35}] = 14,
                [new Range {Start = 36, End = 47}] = 17,
                [new Range {Start = 48, End = 59}] = 20,
                [new Range {Start = 60, End = 71}] = 23,
                [new Range {Start = 72, End = 83}] = 27,
                [new Range {Start = 84, End = 95}] = 32
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "cognitive_-2.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 0,
                [new Range {Start = 6, End = 11}] = 3,
                [new Range {Start = 12, End = 17}] = 7,
                [new Range {Start = 18, End = 23}] = 10,
                [new Range {Start = 24, End = 35}] = 13,
                [new Range {Start = 36, End = 47}] = 15,
                [new Range {Start = 48, End = 59}] = 19,
                [new Range {Start = 60, End = 71}] = 22,
                [new Range {Start = 72, End = 83}] = 25,
                [new Range {Start = 84, End = 95}] = 28
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "adaptive_-1.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 3,
                [new Range {Start = 6, End = 11}] = 5,
                [new Range {Start = 12, End = 17}] = 8,
                [new Range {Start = 18, End = 23}] = 11,
                [new Range {Start = 24, End = 35}] = 14,
                [new Range {Start = 36, End = 47}] = 19,
                [new Range {Start = 48, End = 59}] = 23,
                [new Range {Start = 60, End = 71}] = 28,
                [new Range {Start = 72, End = 83}] = 32,
                [new Range {Start = 84, End = 95}] = 36
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "adaptive_-1.5";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 2,
                [new Range {Start = 6, End = 11}] = 4,
                [new Range {Start = 12, End = 17}] = 7,
                [new Range {Start = 18, End = 23}] = 10,
                [new Range {Start = 24, End = 35}] = 13,
                [new Range {Start = 36, End = 47}] = 17,
                [new Range {Start = 48, End = 59}] = 20,
                [new Range {Start = 60, End = 71}] = 25,
                [new Range {Start = 72, End = 83}] = 29,
                [new Range {Start = 84, End = 95}] = 34
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "adaptive_-2.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 1,
                [new Range {Start = 6, End = 11}] = 3,
                [new Range {Start = 12, End = 17}] = 6,
                [new Range {Start = 18, End = 23}] = 9,
                [new Range {Start = 24, End = 35}] = 12,
                [new Range {Start = 36, End = 47}] = 15,
                [new Range {Start = 48, End = 59}] = 18,
                [new Range {Start = 60, End = 71}] = 23,
                [new Range {Start = 72, End = 83}] = 27,
                [new Range {Start = 84, End = 95}] = 32
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "communication_-1.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 1,
                [new Range {Start = 6, End = 11}] = 4,
                [new Range {Start = 12, End = 17}] = 8,
                [new Range {Start = 18, End = 23}] = 10,
                [new Range {Start = 24, End = 35}] = 14,
                [new Range {Start = 36, End = 47}] = 19,
                [new Range {Start = 48, End = 59}] = 23,
                [new Range {Start = 60, End = 71}] = 27,
                [new Range {Start = 72, End = 83}] = 31,
                [new Range {Start = 84, End = 95}] = 35
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "communication_-1.5";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 0,
                [new Range {Start = 6, End = 11}] = 3,
                [new Range {Start = 12, End = 17}] = 7,
                [new Range {Start = 18, End = 23}] = 9,
                [new Range {Start = 24, End = 35}] = 13,
                [new Range {Start = 36, End = 47}] = 17,
                [new Range {Start = 48, End = 59}] = 21,
                [new Range {Start = 60, End = 71}] = 24,
                [new Range {Start = 72, End = 83}] = 27,
                [new Range {Start = 84, End = 95}] = 31
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "communication_-2.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 0,
                [new Range {Start = 6, End = 11}] = 2,
                [new Range {Start = 12, End = 17}] = 5,
                [new Range {Start = 18, End = 23}] = 7,
                [new Range {Start = 24, End = 35}] = 10,
                [new Range {Start = 36, End = 47}] = 15,
                [new Range {Start = 48, End = 59}] = 18,
                [new Range {Start = 60, End = 71}] = 22,
                [new Range {Start = 72, End = 83}] = 24,
                [new Range {Start = 84, End = 95}] = 28
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "motor_-1.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 1,
                [new Range {Start = 6, End = 11}] = 4,
                [new Range {Start = 12, End = 17}] = 9,
                [new Range {Start = 18, End = 23}] = 12,
                [new Range {Start = 24, End = 35}] = 17,
                [new Range {Start = 36, End = 47}] = 20,
                [new Range {Start = 48, End = 59}] = 24,
                [new Range {Start = 60, End = 71}] = 30,
                [new Range {Start = 72, End = 83}] = 34,
                [new Range {Start = 84, End = 95}] = 37
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "motor_-1.5";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 0,
                [new Range {Start = 6, End = 11}] = 3,
                [new Range {Start = 12, End = 17}] = 7,
                [new Range {Start = 18, End = 23}] = 11,
                [new Range {Start = 24, End = 35}] = 15,
                [new Range {Start = 36, End = 47}] = 18,
                [new Range {Start = 48, End = 59}] = 22,
                [new Range {Start = 60, End = 71}] = 27,
                [new Range {Start = 72, End = 83}] = 31,
                [new Range {Start = 84, End = 95}] = 35
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "motor_-2.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 0,
                [new Range {Start = 6, End = 11}] = 2,
                [new Range {Start = 12, End = 17}] = 6,
                [new Range {Start = 18, End = 23}] = 9,
                [new Range {Start = 24, End = 35}] = 13,
                [new Range {Start = 36, End = 47}] = 17,
                [new Range {Start = 48, End = 59}] = 20,
                [new Range {Start = 60, End = 71}] = 24,
                [new Range {Start = 72, End = 83}] = 28,
                [new Range {Start = 84, End = 95}] = 32
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "social-emotional_-1.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 2,
                [new Range {Start = 6, End = 11}] = 6,
                [new Range {Start = 12, End = 17}] = 9,
                [new Range {Start = 18, End = 23}] = 12,
                [new Range {Start = 24, End = 35}] = 15,
                [new Range {Start = 36, End = 47}] = 18,
                [new Range {Start = 48, End = 59}] = 22,
                [new Range {Start = 60, End = 71}] = 27,
                [new Range {Start = 72, End = 83}] = 32,
                [new Range {Start = 84, End = 95}] = 36
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "social-emotional_-1.5";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 1,
                [new Range {Start = 6, End = 11}] = 5,
                [new Range {Start = 12, End = 17}] = 8,
                [new Range {Start = 18, End = 23}] = 10,
                [new Range {Start = 24, End = 35}] = 13,
                [new Range {Start = 36, End = 47}] = 15,
                [new Range {Start = 48, End = 59}] = 18,
                [new Range {Start = 60, End = 71}] = 22,
                [new Range {Start = 72, End = 83}] = 26,
                [new Range {Start = 84, End = 95}] = 32
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            
            contentAndStdDev = "social-emotional_-2.0";
            currentLookup = new Dictionary<Range, int>
            {
                [new Range {Start = 0, End = 5}] = 0,
                [new Range {Start = 6, End = 11}] = 4,
                [new Range {Start = 12, End = 17}] = 6,
                [new Range {Start = 18, End = 23}] = 9,
                [new Range {Start = 24, End = 35}] = 11,
                [new Range {Start = 36, End = 47}] = 14,
                [new Range {Start = 48, End = 59}] = 16,
                [new Range {Start = 60, End = 71}] = 19,
                [new Range {Start = 72, End = 83}] = 22,
                [new Range {Start = 84, End = 95}] = 26
            };
            _screenerCutScores[contentAndStdDev] = currentLookup;
            #endregion
        }        
    }
}