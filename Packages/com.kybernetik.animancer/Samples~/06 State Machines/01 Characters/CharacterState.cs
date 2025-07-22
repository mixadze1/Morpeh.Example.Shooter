// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>A state for a <see cref="Character"/>.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/characters">
    /// Characters</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/CharacterState
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Characters - Character State")]
    [AnimancerHelpUrl(typeof(CharacterState))]
    public abstract class CharacterState : StateBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private Character _Character;
        public Character Character => _Character;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif

        /************************************************************************************************************************/
        // Explained in the Interruptions sample.
        /************************************************************************************************************************/

        public virtual CharacterStatePriority Priority => CharacterStatePriority.Low;

        public virtual bool CanInterruptSelf => false;

        public override bool CanExitState
        {
            get
            {
                // There are several different ways of accessing the state change details:
                // CharacterState nextState = StateChange<CharacterState>.NextState;
                // CharacterState nextState = this.GetNextState();
                CharacterState nextState = _Character.StateMachine.NextState;
                if (nextState == this)
                    return CanInterruptSelf;
                else if (Priority == CharacterStatePriority.Low)
                    return true;
                else
                    return nextState.Priority > Priority;
            }
        }

        /************************************************************************************************************************/
    }
}
