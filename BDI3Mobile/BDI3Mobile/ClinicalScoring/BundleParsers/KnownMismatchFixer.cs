namespace ClinicalScoring.BundleParsers
{
    public static class KnownMismatchFixer
    {
        public static string FixContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                return content;
            content = content.Trim();
            switch (content)
            {
                case "Numbers and Counting Sets":
                    content = "Numbers, Counting, and Sets";
                    break;
                case "Letter ID":
                    content = "Letter Identification";
                    break;
                case "Letter Sound Correspondence":
                    content = "Letter-Sound Correspondence";
                    break;
                case "Phoneme Blending and Segmentation":
                    content = "Phoneme Blending and Segmenting";
                    break;
                case "Reasoning and Academic":
                    content = "Reasoning and Academic Skills";
                    break;
                case "Self-concept/ Social Role":
                case "Social Role":
                    content = "Self-Concept and Social Role";
                    break;
                case "LITERACY":
                    content = "Literacy";
                    break;
                case "MATH":
                    content = "Mathematics";
                    break;
            }
            return content;

        }
    }
}