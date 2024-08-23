using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arstive.Model
{
    /// <summary>
    /// Represent the spectral surface instance of the game
    /// </summary>
    [Serializable]
    public class Chart
    {
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

    }
}
