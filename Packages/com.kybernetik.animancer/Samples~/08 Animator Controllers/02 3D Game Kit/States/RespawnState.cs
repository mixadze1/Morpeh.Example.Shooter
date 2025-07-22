// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.AnimatorControllers.GameKit
{
    /// <summary>
    /// A <see cref="CharacterState"/> which teleports back to the starting position, plays an animation then returns
    /// to the <see cref="Character.Idle"/> state.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/animator-controllers/3d-game-kit/respawn">
    /// 3D Game Kit/Respawn</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.AnimatorControllers.GameKit/RespawnState
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Game Kit - Respawn State")]
    [AnimancerHelpUrl(typeof(RespawnState))]
    public class RespawnState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField] private ClipTransition _Animation;
        [SerializeField] private UnityEvent _OnEnterState;// See the Read Me.
        [SerializeField] private UnityEvent _OnExitState;// See the Read Me.

        private Vector3 _StartingPosition;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _Animation.Events.OnEnd = Character.StateMachine.ForceSetDefaultState;
            _StartingPosition = transform.position;
        }

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            Character.Animancer.Play(_Animation);
            Character.transform.position = _StartingPosition;
            _OnEnterState.Invoke();
        }

        /************************************************************************************************************************/

        protected virtual void OnDisable()
        {
            _OnExitState.Invoke();
        }

        /************************************************************************************************************************/

        public override bool CanExitState => false;

        /************************************************************************************************************************/
    }
}
