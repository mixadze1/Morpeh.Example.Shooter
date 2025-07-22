// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Basics
{
    /// <summary>
    /// Starts with an idle animation and performs an action when the user clicks the mouse, then returns to idle.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/basics/action">
    /// Basic Action</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Basics/PlayAnimationOnClick
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Basics - Play Animation On Click")]
    [AnimancerHelpUrl(typeof(PlayAnimationOnClick))]
    public class PlayAnimationOnClick : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private AnimationClip _Idle;
        [SerializeField] private AnimationClip _Action;

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            _Animancer.Play(_Idle);
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            if (SampleInput.LeftMouseUp)
            {
                // Play the action animation and grab the returned state which we can use to control it.
                AnimancerState state = _Animancer.Play(_Action);

                // Rewind the animation because Play doesn't do that automatically if it was already playing.
                state.Time = 0;

                // When the animation reaches its end, call OnEnable to go back to idle.
                state.Events(this).OnEnd ??= OnEnable;
            }
        }

        /************************************************************************************************************************/
    }
}
