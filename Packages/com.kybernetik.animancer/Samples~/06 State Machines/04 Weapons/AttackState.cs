// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>A <see cref="CharacterState"/> which can perform <see cref="Weapon.AttackAnimations"/> in sequence.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/weapons">
    /// Weapons</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/AttackState
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Weapons - Attack State")]
    [AnimancerHelpUrl(typeof(AttackState))]
    public class AttackState : CharacterState
    {
        /************************************************************************************************************************/

        private int _AttackIndex = int.MaxValue;
        private AnimancerState _CurrentState;

        public Weapon Weapon
            => Character.Equipment.Weapon;

        /************************************************************************************************************************/

        public override bool CanEnterState
            => Weapon != null
            && Weapon.AttackAnimations.Length > 0;

        public override CharacterStatePriority Priority
            => CharacterStatePriority.Medium;

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            // Start at the beginning of the sequence by default,
            // but if the previous attack has not faded out yet
            // then perform the next attack instead.
            if (ShouldRestartCombo)
                _AttackIndex = 0;
            else
                _AttackIndex++;

            TransitionAsset animation = Weapon.AttackAnimations[_AttackIndex];

            _CurrentState = Character.Animancer.Play(animation);
            _CurrentState.Events(this).OnEnd ??= Character.StateMachine.ForceSetDefaultState;
        }

        /************************************************************************************************************************/

        private bool ShouldRestartCombo
            => _AttackIndex >= Weapon.AttackAnimations.Length - 1
            || _CurrentState == null
            || _CurrentState.Weight == 0;

        /************************************************************************************************************************/
    }
}
