// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Samples.AnimatorControllers.GameKit
{
    /// <summary>A <see cref="CharacterState"/> which plays a "dying" animation.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/animator-controllers/3d-game-kit/die">
    /// 3D Game Kit/Die</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.AnimatorControllers.GameKit/DieState
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Game Kit - Die State")]
    [AnimancerHelpUrl(typeof(DieState))]
    public class DieState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField] private ClipTransition _Animation;
        [SerializeField] private CharacterState _RespawnState;
        [SerializeField] private UnityEvent _OnEnterState;// See the Read Me.
        [SerializeField] private UnityEvent _OnExitState;// See the Read Me.

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            // Respawn immediately when the animation ends.
            _Animation.Events.OnEnd = _RespawnState.ForceEnterState;
        }

        /************************************************************************************************************************/

        public void OnDeath()
        {
            Character.StateMachine.ForceSetState(this);
        }

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            Character.Animancer.Play(_Animation);
            Character.Parameters.ForwardSpeed = 0;
            _OnEnterState.Invoke();
        }

        /************************************************************************************************************************/

        protected virtual void OnDisable()
        {
            _OnExitState.Invoke();
        }

        /************************************************************************************************************************/

        public override bool FullMovementControl => false;

        /************************************************************************************************************************/

        public override bool CanExitState => false;

        /************************************************************************************************************************/
    }
}
