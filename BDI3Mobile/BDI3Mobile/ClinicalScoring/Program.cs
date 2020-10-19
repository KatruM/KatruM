using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ClinicalScoring.BundleParsers;

namespace ClinicalScoring
{
    public static class Program
    {
        private static void Main()
        {
            Bdi3ScoringParser parser;
#if SERIALIZED_BUILD
            parser = Bdi3ScoringParser.GetInstance();
            var res = parser.GetAeFromRs(12, "Communication", "Receptive Communication");
            Console.WriteLine(9 == res);
            res = parser.GetAeFromRs(27, "Motor", "Gross Motor");
            Console.WriteLine(8 == res);
            res = parser.GetAeFromRs(99, "Cognitive", "Perception and Concepts");
            
            Console.WriteLine(95 == res);
#else
            Bdi3ScoringParser parser;
            IFormatter formatter = new BinaryFormatter();  
            if (File.Exists("parserSerialized.bin"))
            {
                var readStream = new FileStream("parserSerialized.bin", FileMode.Open, FileAccess.Read, FileShare.None);
                parser = (Bdi3ScoringParser)formatter.Deserialize(readStream);
                readStream.Close();
                var res = parser.GetAeFromRs(12, "Communication", "Receptive Communication");
                Console.WriteLine(9 == res);
                res = parser.GetAeFromRs(27, "Motor", "Gross Motor");
                Console.WriteLine(8 == res);
                res = parser.GetAeFromRs(99, "Cognitive", "Perception and Concepts");
            
                Console.WriteLine(95 == res);
                
            }
                
            parser = new Bdi3ScoringParser();
            Stream stream = new FileStream("parserSerialized.bin", FileMode.Create, FileAccess.Write, FileShare.None);  
            formatter.Serialize(stream, parser);  
            stream.Close();              
            //File.WriteAllText("scoringParserSerialized.json", JsonConvert.SerializeObject(parser, Formatting.Indented));
#endif
        }
        
    }
}