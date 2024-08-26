using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using static Arstive.Model.Easing;

namespace Arstive.Model
{
    /// <summary>
    /// Related configurations of easing functions
    /// </summary>
    public class Easing(EasingFunctionType easingType, EasingMode easingMode)
    {
        /// <summary>
        /// Type of easing function
        /// </summary>
        public enum EasingFunctionType
        {
            /// <summary>
            /// The action of slightly retracting the animation before
            /// starting animation processing in the indicated path
            /// </summary>
            BackEase,

            /// <summary>
            /// Create bouncing effects for animations
            /// </summary>
            BounceEase,

            /// <summary>
            /// Create animations that accelerate circles and
            /// decelerate using loop functions
            /// </summary>
            CircleEase,

            /// <summary>
            /// Create an animation that uses the formula
            /// f(t) = t^3 for acceleration or deceleration
            /// </summary>
            CubicEase,

            /// <summary>
            /// Create an animation that resembles a spring
            /// vibrating back and forth until it stops
            /// </summary>
            ElasticEase,

            /// <summary>
            /// Create animations of acceleration or deceleration
            /// using exponential formulas
            /// </summary>
            ExponentialEase,

            /// <summary>
            /// Create an animation of acceleration or deceleration
            /// using the formula f(t) = t^2
            /// </summary>
            QuadraticEase,

            /// <summary>
            /// Create an animation of acceleration or deceleration
            /// using the formula f(t) = t^4
            /// </summary>
            QuarticEase,

            /// <summary>
            /// Create an animation of acceleration or deceleration
            /// using the formula f(t) = t^5
            /// </summary>
            QuinticEase,

            /// <summary>
            /// Create animations of acceleration or deceleration
            /// using sine formulas.
            /// </summary>
            SineEase
        }

        /// <summary>
        /// Type of easing function
        /// </summary>
        [JsonPropertyName("easing_func")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EasingFunctionType EasingFunction { get; set; } = easingType;


        /// <summary>
        /// Mode of easing
        /// </summary>
        [JsonPropertyName("easing_mode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EasingMode EasingMode = easingMode;

        internal EasingFunctionBase GetEasingFunction()
        {
            var funcName = this.EasingFunction.ToString();
            var easingFunction = 
                Type.GetType($"System.Windows.Media.Animation.{funcName}," +
                             $"PresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            var easing = (EasingFunctionBase)Activator.CreateInstance(easingFunction!)!;
            easing.EasingMode = this.EasingMode;
            return easing;
        }
    }
}
