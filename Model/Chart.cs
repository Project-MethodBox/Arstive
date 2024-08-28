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
        [JsonPropertyName("basic_info")]
        public ChartBasicInfo? BasicInfo { get; set; }

        /// <summary>
        /// List of judgment angles
        /// </summary>
        [JsonPropertyName("judgment_angles")] 
        public List<JudgmentAngle> JudgmentAngles;

        /// <summary>
        /// Notes that not belongs to any judgment angle
        /// </summary>
        [JsonPropertyName("free_notes")]
        public List<Interfaces.FreeNote> FreeNotes { get; set; }
    }
}
