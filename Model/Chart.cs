using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Arstive.Model
{
    /// <summary>
    /// Represent the spectral surface instance of the game
    /// </summary>
    [Serializable]
    public class Chart
    {
        public static Chart Shared { get; set; } = new();

        public Chart()
        {

        }

        public Chart(List<JudgmentAngle> judgmentAngles, ChartBasicInfo? basicInfo)
        {
            JudgmentAngles = judgmentAngles;
            BasicInfo = basicInfo;
        }

        /// <summary>
        /// Chart basic info,include charters,composers,etc.
        /// </summary>
        [JsonPropertyName("basic")]
        public ChartBasicInfo? BasicInfo { get; set; }

        /// <summary>
        /// List of judgment angles
        /// </summary>
        [JsonPropertyName("angles")]
        public List<JudgmentAngle> JudgmentAngles { get; set; }

        /// <summary>
        /// Notes that not belongs to any judgment angle
        /// </summary>
        [JsonPropertyName("fotes")]
        public List<Interfaces.FreeNote> FreeNotes { get; set; }

        /// <summary>
        /// Save chart to file
        /// </summary>
        /// <param name="path">This is a comment</param>
        public static void Save(string path)
        {
            // Load chart instance
            var jsonString = JsonSerializer.Serialize(Shared);

            // Write to file
            using var writer = new StreamWriter(path);
            writer.WriteLine(jsonString);
            writer.Close();
        }

        public static void Load(string path)
        {
            // Load from file
            using var reader = new StreamReader(path);
            var jsonString = reader.ReadToEnd();
            reader.Close();

            // Load to instance
            Chart instance = JsonSerializer.Deserialize<Chart>(jsonString)!;
            Shared = instance;
        }
    }
}
