// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.FineControl
{
    /// <summary>Demonstrates how to save some performance by updating Animancer less often.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fine-control/update-rate">
    /// Update Rate</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.FineControl/LowUpdateRate
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Fine Control - Low Update Rate")]
    [AnimancerHelpUrl(typeof(LowUpdateRate))]
    public class LowUpdateRate : MonoBehaviour
    {
        /************************************************************************************************************************/

        // This script doesn't play any animations.
        // In a real game, you would have other scripts doing that.
        // But for this sample, we're just using a NamedAnimancerComponent for its Play Automatically field.

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField, PerSecond] private float _UpdatesPerSecond = 5;

        private float _LastUpdateTime;

        /************************************************************************************************************************/
        // The DynamicUpdateRate script will enable and disable this script.
        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            _Animancer.Graph.PauseGraph();
            _LastUpdateTime = Time.time;
        }

        protected virtual void OnDisable()
        {

            // This will get called when destroying the object as well (such as when loading a different scene).
            // So we need to make sure the AnimancerComponent still exists and is still initialized.
            if (_Animancer != null && _Animancer.IsGraphInitialized)
                _Animancer.Graph.UnpauseGraph();
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            float time = Time.time;
            float timeSinceLastUpdate = time - _LastUpdateTime;
            if (timeSinceLastUpdate > 1 / _UpdatesPerSecond)
            {
                _Animancer.Evaluate(timeSinceLastUpdate);
                _LastUpdateTime = time;
            }
        }

        /************************************************************************************************************************/
    }
}
