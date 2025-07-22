// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>A <see cref="CharacterState"/> which activates itself when the character takes damage.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/interruptions">
    /// Interruptions</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/FlinchState
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Interruptions - Flinch State")]
    [AnimancerHelpUrl(typeof(FlinchState))]
    public class FlinchState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField] private TransitionAsset _Animation;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            Character.Health.OnHitReceived += () => Character.StateMachine.TryResetState(this);
        }

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            AnimancerState state = Character.Animancer.Play(_Animation);
            state.Events(this).OnEnd ??= Character.StateMachine.ForceSetDefaultState;
        }

        /************************************************************************************************************************/

        public override CharacterStatePriority Priority => CharacterStatePriority.High;

        public override bool CanInterruptSelf => true;

        /************************************************************************************************************************/
    }
}
