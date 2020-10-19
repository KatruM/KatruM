using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
#if SERIALIZED_BUILD
#else
using ClosedXML.Excel;
#endif
using Newtonsoft.Json;

namespace ClinicalScoring.BundleParsers
{
    [Serializable]
    public abstract class ScoringParser
    {
        public static int NO_SCORE = -999;
        [NonSerialized]
        public readonly Dictionary<string, string> TextContents=new Dictionary<string, string>();
        [NonSerialized]
        public readonly Dictionary<string, Dictionary<string, Dictionary<int, List<string>>>> SheetContents=new Dictionary<string, Dictionary<string, Dictionary<int, List<string>>>>();
        public readonly Dictionary<ScoreLookup, List<ScoreValue>> ScoreLookup=new Dictionary<ScoreLookup, List<ScoreValue>>();
        public readonly Dictionary<string, List<ScoreLookup>> ScoreLookupsByContent=new Dictionary<string, List<ScoreLookup>>();
        public readonly Dictionary<Range, List<ScoreLookup>> ScoreLookupsByAgeRange=new Dictionary<Range, List<ScoreLookup>>();
        public readonly List<ScoreLookup> NonContentLookups = new List<ScoreLookup>();
        protected virtual string[] BundleNames => new[] {"Bdi3ScoringTables.zip"};
        protected ScoringParser()
        {
            
        }

        protected async Task Parse(Func<ZipArchiveEntry, string, Task<bool>> callback)
        {
#if SERIALIZED_BUILD
#else
            foreach (var bundleName in BundleNames)
            {
                ZipArchive zipArchive = null;
                var fs = Assembly.GetExecutingAssembly().GetManifestResourceStream($"ClinicalScoring.ScoringBundles.{bundleName}");
                if (fs != null)
                {
                    zipArchive = new ZipArchive(fs, ZipArchiveMode.Read);
                }  
                using (var archive = zipArchive)
                {
                    foreach (var zipArchiveEntry in archive.Entries)
                    {
                        var extension = zipArchiveEntry.Name.Substring(zipArchiveEntry.Name.LastIndexOf('.') + 1)
                            .ToUpperInvariant();
                        await callback(zipArchiveEntry, extension).ConfigureAwait(false);
                    }
                }
            }
            await ParseDoneAsync().ConfigureAwait(false);
#endif
        }

        public List<ScoreValue> GetOrCreateLookup(ScoreLookup sl)
        {
            if (ScoreLookup.ContainsKey(sl))
                return ScoreLookup[sl];
            if (string.IsNullOrEmpty(sl.SubDomain))
            {
                NonContentLookups.Add(sl);
            }
            else
            {
                if (sl.SubDomain.Length < 4)
                {
                    //logger?.LogWarning("Abbreviation detected!");
                }
                if (!ScoreLookupsByContent.ContainsKey(sl.SubDomain))
                {
                    ScoreLookupsByContent[sl.SubDomain] = new List<ScoreLookup>();
                }

                ScoreLookupsByContent[sl.SubDomain].Add(sl);
            }

            if (sl.AgeRange != null)
            {
                if (string.IsNullOrEmpty(sl.AgeRange.Definition))
                {
                    //logger?.LogWarning("Definition is missing!");
                }
                if(!ScoreLookupsByAgeRange.ContainsKey(sl.AgeRange))
                {
                    ScoreLookupsByAgeRange[sl.AgeRange] = new List<ScoreLookup>();
                }
                ScoreLookupsByAgeRange[sl.AgeRange].Add(sl);
            }
            return ScoreLookup[sl] = new List<ScoreValue>();
        }
        protected static void ForEachCell(IReadOnlyList<string> row, Action<int, string> action)
        {
            for (var i = 0; i < row.Count; i++)
            {
                action(i, row[i]);
            }
        }

        protected virtual Task ParseDoneAsync()
        {
            return Task.CompletedTask;
        }
        protected async Task<bool> EntryCallbackAsync(ZipArchiveEntry ze, string ext)
        {
#if SERIALIZED_BUILD
#else
            try
            {
                using (var zipStream = ze.Open())
                {
                    if (ext == "XLSX")
                    {
                        var wb = new XLWorkbook(zipStream);
                        //logger?.LogDebug($"Sheets: {string.Join(", ", wb.Worksheets.Select(t => t.Name).ToList())}");
                        SheetContents[ze.Name]=new Dictionary<string, Dictionary<int, List<string>>>(); 
                        foreach (var worksheet in wb.Worksheets)
                        {
                            var rowsUsed = worksheet.RowsUsed(false);
                            if (!rowsUsed.Any())
                            {
                                continue;
                            }
                            var maxLength = rowsUsed.Select(row => row.CellsUsed(true).Count()).Max() + 1;
                            if(!SheetContents[ze.Name].ContainsKey(worksheet.Name))
                                SheetContents[ze.Name][worksheet.Name]=new Dictionary<int, List<string>>();
                            //have to loop through twice... previous  method of getting used cells would skip cells that were never selected but had adjacent content... affecting positions
                            foreach (var row in rowsUsed) {
                                SheetContents[ze.Name][worksheet.Name][row.RowNumber()] =
                                    row.Cells(1, maxLength)
                                        .Select(t => t.GetString()).ToList();
                                //logger?.LogDebug($"file: {ze.Name} Sheet: {worksheet.Name} {string.Join(", ", row.CellsUsed(XLCellsUsedOptions.Contents).Select(t=>t.Value).ToList())}");
                            }
                        }
                        
                    }
                    else if (ext == "TXT")
                    {
                        byte[] data = new byte[ze.Length];
                        await zipStream.ReadAsync(data, 0, data.Length).ConfigureAwait(false);
                        var textContent = Encoding.UTF8.GetString(data);
                        
                        TextContents[ze.Name.ToUpperInvariant()]=textContent;

                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
#endif

            return true;
        }        
        public virtual Task LoadDataAsync()
        {
            return Parse(EntryCallbackAsync);
        }
    }
    [Serializable]

    public class Range : IEquatable<Range>
    {
        public int Start;
        public int End;
        public int Length => ((End+1) - Start); 
        public string Definition { get; set; }

        public bool Equals(Range other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Start == other.Start && End == other.End && Definition == other.Definition;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Range) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Start;
                hashCode = (hashCode * 397) ^ End;
                hashCode = (hashCode * 397) ^ (Definition != null ? Definition.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Range left, Range right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Range left, Range right)
        {
            return !Equals(left, right);
        }

        public bool Contains(int ageInMonths)
        {
            if (Start == End)
                return ageInMonths == Start;
            return ageInMonths >= Start && ageInMonths <= End;
        }
    }
    
}