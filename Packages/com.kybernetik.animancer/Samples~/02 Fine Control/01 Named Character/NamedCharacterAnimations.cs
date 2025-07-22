// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.FineControl
{
    /// <summary>
    /// Implements the same behaviour as <see cref="Basics.LibraryCharacterAnimations"/>
    /// but refers to the animations by name instead of using direct references.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fine-control/named">
    /// Named Character</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.FineControl/NamedCharacterAnimations
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Fine Control - Named Character Animations")]
    [AnimancerHelpUrl(typeof(NamedCharacterAnimations))]
    public class NamedCharacterAnimations : MonoBehaviour
    {
        /************************************************************************************************************************/
        // This script is almost identical to LibraryCharacterAnimations, with a few differences:
        // - It uses StringAsset instead of TransitionAssets.
        // - It calls TryPlay instead of Play.
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private StringAsset _Idle;
        [SerializeField] private StringAsset _Move;
        [SerializeField] private StringAsset _Action;

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
                _Animancer.TryPlay(_Move);
            }
            else
            {
                _Animancer.TryPlay(_Idle);
            }
        }

        /************************************************************************************************************************/

        private void UpdateAction()
        {
            if (SampleInput.LeftMouseUp)
            {
                _CurrentState = State.Acting;

                AnimancerState state = _Animancer.TryPlay(_Action);
                state.Events(this).OnEnd ??= UpdateMovement;
            }
        }

        /************************************************************************************************************************/
    }
}
