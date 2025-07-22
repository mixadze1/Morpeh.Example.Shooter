// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Basics
{
    /// <summary>
    /// Implements the same behaviour as <see cref="BasicCharacterAnimations"/>
    /// using <see cref="TransitionAsset"/>s.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/basics/library">
    /// Library Basics</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Basics/LibraryCharacterAnimations
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Basics - Library Character Animations")]
    [AnimancerHelpUrl(typeof(LibraryCharacterAnimations))]
    public class LibraryCharacterAnimations : MonoBehaviour
    {
        /************************************************************************************************************************/
        // This script is almost identical to BasicCharacterAnimations, with a few differences:
        // - It uses TransitionAssets instead of ClipTransitions.
        // - It assigns the Action state's End Event after playing it instead of on startup.
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private TransitionAsset _Idle;
        [SerializeField] private TransitionAsset _Move;
        [SerializeField] private TransitionAsset _Action;

        private State _CurrentState;

        private enum State
        {
            NotActing,// Idle and Move can be interrupted.
            Acting,// Action can only be interrupted by itself.
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            switch (_CurrentState)
            {
                case State.NotActing:
                    UpdateMovement();
                    UpdateAction();
                    break;

                case State.Acting:
                    UpdateAction();
                    break;
            }
        }

        /************************************************************************************************************************/

        private void UpdateMovement()
        {
            _CurrentState = State.NotActing;

            float forward = SampleInput.WASD.y;
            if (forward > 0)
            {
                _Animancer.Play(_Move);
            }
            else
            {
                _Animancer.Play(_Idle);
            }
        }

        /************************************************************************************************************************/

        private void UpdateAction()
        {
            if (SampleInput.LeftMouseUp)
            {
                _CurrentState = State.Acting;

                // _Action is an asset that could be shared by multiple different characters
                // as well as instances of the same character so we can't set up the events
                // in the transition because the characters would conflict with each other.
                // Instead, we add events to the state so each character's events are separate.

                AnimancerState state = _Animancer.Play(_Action);
                state.Events(this).OnEnd ??= UpdateMovement;
            }
        }

        /************************************************************************************************************************/
    }
}
