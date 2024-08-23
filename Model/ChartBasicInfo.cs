using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arstive.Model
{
    public class ChartBasicInfo
    {
        /// <summary>
        /// Version of chart 
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version;

        /// <summary>
        /// The name of song
        /// </summary>
        [JsonPropertyName("song")]
        public string? SongName;

        /// <summary>
        /// Composer(s) of current song
        /// </summary>
        [JsonPropertyName("composer")]
        public string? Composer;

        /// <summary>
        /// Charter(s) of current song
        /// </summary>
        [JsonPropertyName("charter")]
        public string? Charter;

        /// <summary>
        /// Chart difficulty level
        /// </summary>
        [JsonPropertyName("difficulty_name")]
        public ChartDifficulty ChartDifficultyName;

        /// <summary>
        /// Chart difficulty  
        /// </summary>
        [JsonPropertyName("difficulty")]
        public double ChartDifficultyNumber { get; set; }
    }

    public enum ChartDifficulty
    {
        /// <summary>
        /// Level 1
        /// </summary>
        Triangle,

        /// <summary>
        /// Level 2
        /// </summary>
        Quadrilateral,

        /// <summary>
        /// Level 3
        /// </summary>
        Pentagon,

        /// <summary>
        /// Level 4
        /// </summary>
        Hexagon,
    }
}
