// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Mixers
{
    /// <summary>Plays a transition asset in <see cref="OnEnable"/>.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/mixers/linear">
    /// Linear Mixers</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Mixers/PlayTransitionAssetOnEnable
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Mixers - Play Transition Asset On Enable")]
    [AnimancerHelpUrl(typeof(PlayTransitionAssetOnEnable))]
    public class PlayTransitionAssetOnEnable : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private TransitionAssetBase _Transition;

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            _Animancer.Play(_Transition);
        }

        /************************************************************************************************************************/
    }
}
