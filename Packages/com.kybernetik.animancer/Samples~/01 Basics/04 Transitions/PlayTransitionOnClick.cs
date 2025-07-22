// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Basics
{
    /// <summary>
    /// This script is basically the same as <see cref="PlayAnimationOnClick"/>, except that it uses
    /// <see href="https://kybernetik.com.au/animancer/docs/manual/transitions">Transitions</see>.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/basics/transitions">
    /// Transitions</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Basics/PlayTransitionOnClick
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Basics - Play Transition On Click")]
    [AnimancerHelpUrl(typeof(PlayTransitionOnClick))]
    public class PlayTransitionOnClick : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private ClipTransition _Idle;
        [SerializeField] private ClipTransition _Action;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            // Transitions store their events so we only initialize them once on startup
            // instead of setting the event every time the animation is played.
            _Action.Events.OnEnd = OnEnable;
        }

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            // The Fade Duration of this transition will be ignored
            // the first time because nothing else is playing yet.
            _Animancer.Play(_Idle);
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            if (SampleInput.LeftMouseUp)
            {
                _Animancer.Play(_Action);

                // If you want to cross fade without using Transitions
                // or override the fade duration of a Transition
                // then you can simply use the second parameter in the Play method.
                // _Animancer.Play(_Action, 0.25f);

                // When cross fading, setting the state.Time like the PlayAnimationOnClick script
                // would prevent it from smoothly blending so if you want to restart the animation
                // you should use FadeMode.FromStart.
                // _Animancer.Play(_Action, 0.25f, FadeMode.FromStart);

                // But if you use transitions then you don't need to specify each of those
                // because the Fade Duration is set in the Inspector and it automatically
                // picks the FadeMode based on whether the Start Time check box is enabled or not.
            }
        }

        /************************************************************************************************************************/
    }
}
