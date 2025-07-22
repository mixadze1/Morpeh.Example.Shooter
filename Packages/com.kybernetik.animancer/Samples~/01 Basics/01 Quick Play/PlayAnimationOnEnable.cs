// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Basics
{
    /// <summary>Plays an animation to demonstrate the basic usage of Animancer.</summary>
    /// 
    /// <remarks>
    /// If you actually want to only play one animation on an object and don't need any of the other features of
    /// Animancer, you can use the <see cref="SoloAnimation"/> component to do so without needing an extra script.
    /// <para></para>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/basics/quick-play">
    /// Quick Play</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Basics/PlayAnimationOnEnable
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Basics - Play Animation On Enable")]
    [AnimancerHelpUrl(typeof(PlayAnimationOnEnable))]
    public class PlayAnimationOnEnable : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private AnimationClip _Animation;

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            _Animancer.Play(_Animation);
        }

        /************************************************************************************************************************/
    }
}
