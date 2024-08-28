using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace Arstive.Model
{
    public class ChartBasicInfo:INotifyPropertyChanged
    {
        /// <summary>
        /// Version of chart 
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; set; }

        /// <summary>
        /// The name of song
        /// </summary>
        [JsonPropertyName("song")]
        public string? SongName { get; set; }

        /// <summary>
        /// Composer(s) of current song
        /// </summary>
        [JsonPropertyName("composer")]
        public string? Composer { get; set; }

        /// <summary>
        /// Charter(s) of current song
        /// </summary>
        [JsonPropertyName("charter")]
        public string? Charter { get; set; }

        /// <summary>
        /// Chart difficulty level
        /// </summary>
        [JsonPropertyName("difficulty_name")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ChartDifficulty ChartDifficultyName { get; set; }

        /// <summary>
        /// Chart difficulty  
        /// </summary>
        [JsonPropertyName("difficulty_num")]
        public double ChartDifficultyNumber { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
