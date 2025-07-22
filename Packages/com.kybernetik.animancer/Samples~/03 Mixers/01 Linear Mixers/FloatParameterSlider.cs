// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

#if UNITY_UGUI
using UnityEngine.UI;
#endif

namespace Animancer.Samples.Mixers
{
    /// <summary>Binds a <see cref="Parameter{T}"/> to a <see cref="Slider"/>.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/mixers/linear">
    /// Linear Mixers</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Mixers/FloatParameterSlider
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Mixers - Float Parameter Slider")]
    [AnimancerHelpUrl(typeof(FloatParameterSlider))]
    public class FloatParameterSlider : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_UGUI
        /************************************************************************************************************************/

        [SerializeField] private Slider _Slider;
        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private StringAsset _ParameterName;

        /************************************************************************************************************************/

        private Parameter<float> _Parameter;

        protected virtual void Awake()
        {
            _Parameter = _Animancer.Parameters.GetOrCreate<float>(_ParameterName);

            // When the slider changes, set the parameter.
            _Slider.onValueChanged.AddListener(_Parameter.SetValue);

            // When the parameter changes, set the slider.
            _Parameter.OnValueChanged += value => _Slider.value = value;

            // This won't cause an infinite loop because both systems will only
            // invoke their change event if the value is actually different.
        }

        /************************************************************************************************************************/

        // You can also get and set parameters by name instead of caching the Parameter<float>.
        // But that's a bit slower because it requires a dictionary lookup
        // to find the target parameter each time it's accessed.

        public float ParameterValueLazy
        {
            get => _Animancer.Parameters.GetFloat(_ParameterName);
            set => _Animancer.Parameters.SetValue(_ParameterName, value);
        }

        /************************************************************************************************************************/

        // Smoothly move the value over time for recording the documentation video.
        //protected virtual void Update()
        //{
        //    _Parameter.Value = (Mathf.Sin(Time.timeSinceLevelLoad / 3) * 0.5f + 0.5f) * 1.5f;
        //}

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingUnityUIModuleError(this);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
