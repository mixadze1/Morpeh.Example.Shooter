// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

using UnityEngine;

namespace Animancer.Samples
{
    /// <summary>A simple Inspector slider to control <see cref="Time.timeScale"/>.</summary>
    /// <remarks>
    /// <strong>Documentation:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/basics/scene-setup#time-scale">
    /// Time Scale</see>
    /// </remarks>
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples/TimeScale
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Time Scale")]
    [AnimancerHelpUrl(typeof(TimeScale))]
    public class TimeScale : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField, Range(0, 1)]
        private float _Value = 0.5f;

        /// <summary>
        /// The current <see cref="Time.timeScale"/> or the value that will be assigned when this component is enabled.
        /// </summary>
        public float Value
        {
            get => _Value;
            set
            {
                _Value = value;
                Time.timeScale = _Value;
            }
        }

        /************************************************************************************************************************/

        private float _PreviousValue = 1;

        protected virtual void OnEnable()
        {
            _PreviousValue = Time.timeScale;
            Time.timeScale = _Value;
        }

        protected virtual void OnDisable()
        {
            Time.timeScale = _PreviousValue;
        }

        /************************************************************************************************************************/

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;
#endif

            if (enabled)
                Time.timeScale = _Value;
        }

        /************************************************************************************************************************/
    }
}
