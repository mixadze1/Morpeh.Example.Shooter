// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
#pragma warning disable UNT0028 // Use non-allocating physics APIs.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.AnimatorControllers.GameKit
{
    /// <summary>A <see cref="CharacterState"/> which plays a "getting hit" animation.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/animator-controllers/3d-game-kit/flinch">
    /// 3D Game Kit/Flinch</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.AnimatorControllers.GameKit/FlinchState
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Game Kit - Flinch State")]
    [AnimancerHelpUrl(typeof(FlinchState))]
    public class FlinchState : CharacterState
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        [SerializeField] private MixerTransition2D _Animation;
        [SerializeField] private LayerMask _EnemyLayers;
        [SerializeField, Meters] private float _EnemyCheckRadius = 1;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _Animation.Events.OnEnd = Character.StateMachine.ForceSetDefaultState;
        }

        /************************************************************************************************************************/

        public void OnDamageReceived() => Character.StateMachine.ForceSetState(this);

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            Character.Parameters.ForwardSpeed = 0;
            Character.Animancer.Play(_Animation);

            Vector3 direction = DetermineHitDirection();

            // Once we know which direction the hit came from, we need to convert it to be relative to the model.
            // The Parameter X represents left/right so we project the direction onto the right vector.
            // The Parameter Y represents forward/back so we project the direction onto the forward vector.
            _Animation.State.Parameter = new(
                Vector3.Dot(Character.Animancer.transform.right, direction),
                Vector3.Dot(Character.Animancer.transform.forward, direction));
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Since Animancer does not actually depend on the 3D Game Kit (except for this sample), we cannot reference
        /// any of its scripts from here so we cannot use their <c>IMessageReceiver</c> system which informs the
        /// defending character where the incoming hit came from.
        /// <para></para>
        /// So instead we just find the closest enemy and use that as the direction.
        /// </summary>
        private Vector3 DetermineHitDirection()
        {
            Vector3 position = Character.transform.position;
            float closestEnemySquaredDistance = float.PositiveInfinity;
            Vector3 closestEnemyDirection = Vector3.zero;

            Collider[] enemies = Physics.OverlapSphere(position, _EnemyCheckRadius, _EnemyLayers);
            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3 direction = enemies[i].transform.position - position;
                float squaredDistance = direction.magnitude;
                if (closestEnemySquaredDistance > squaredDistance)
                {
                    closestEnemySquaredDistance = squaredDistance;
                    closestEnemyDirection = direction;
                }
            }

            return closestEnemyDirection.normalized;
        }

        /************************************************************************************************************************/

        public override bool FullMovementControl => false;

        /************************************************************************************************************************/

        public override bool CanExitState => false;

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingPhysics3DModuleError(this);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
