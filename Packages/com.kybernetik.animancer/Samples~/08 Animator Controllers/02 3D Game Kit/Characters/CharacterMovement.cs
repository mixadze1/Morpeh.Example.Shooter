// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;
using static Animancer.Validate;

namespace Animancer.Samples.AnimatorControllers.GameKit
{
    /// <summary>The stats and logic for moving a <see cref="Character"/>.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/animator-controllers/3d-game-kit">
    /// 3D Game Kit</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.AnimatorControllers.GameKit/CharacterMovement
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Game Kit - Character Movement")]
    [AnimancerHelpUrl(typeof(CharacterMovement))]
    public class CharacterMovement : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        [SerializeField] private Character _Character;
        [SerializeField] private CharacterController _CharacterController;
        [SerializeField] private bool _FullMovementControl = true;

        /************************************************************************************************************************/

        [SerializeField, MetersPerSecond(Rule = Value.IsNotNegative)]
        private float _MaxSpeed = 8;
        public float MaxSpeed => _MaxSpeed;

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        private float _Acceleration = 20;
        public float Acceleration => _Acceleration;

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        private float _Deceleration = 25;
        public float Deceleration => _Deceleration;

        [SerializeField, DegreesPerSecond(Rule = Value.IsNotNegative)]
        private float _MinTurnSpeed = 400;
        public float MinTurnSpeed => _MinTurnSpeed;

        [SerializeField, DegreesPerSecond(Rule = Value.IsNotNegative)]
        private float _MaxTurnSpeed = 1200;
        public float MaxTurnSpeed => _MaxTurnSpeed;

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        private float _Gravity = 20;
        public float Gravity => _Gravity;

        [SerializeField, Multiplier(Rule = Value.IsNotNegative)]
        private float _StickingGravityProportion = 0.3f;
        public float StickingGravityProportion => _StickingGravityProportion;

        /************************************************************************************************************************/

        public bool IsGrounded { get; private set; }
        public Material GroundMaterial { get; private set; }

        /************************************************************************************************************************/

        public void UpdateSpeedControl()
        {
            Vector3 movement = _Character.Parameters.MovementDirection;

            _Character.Parameters.DesiredForwardSpeed = movement.magnitude * MaxSpeed;

            float deltaSpeed = movement != Vector3.zero ? Acceleration : Deceleration;
            _Character.Parameters.ForwardSpeed = Mathf.MoveTowards(
                _Character.Parameters.ForwardSpeed,
                _Character.Parameters.DesiredForwardSpeed,
                deltaSpeed * Time.deltaTime);
        }

        /************************************************************************************************************************/

        public float CurrentTurnSpeed
            => Mathf.Lerp(
                MaxTurnSpeed,
                MinTurnSpeed,
                _Character.Parameters.ForwardSpeed / _Character.Parameters.DesiredForwardSpeed);
                
        /************************************************************************************************************************/

        public bool GetTurnAngles(Vector3 direction, out float currentAngle, out float targetAngle)
        {
            if (direction == Vector3.zero)
            {
                currentAngle = float.NaN;
                targetAngle = float.NaN;
                return false;
            }

            currentAngle = transform.eulerAngles.y;
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            return true;
        }
        
        /************************************************************************************************************************/

        public void TurnTowards(float currentAngle, float targetAngle, float speed)
        {
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, speed * Time.deltaTime);

            transform.eulerAngles = new(0, currentAngle, 0);
        }

        public void TurnTowards(Vector3 direction, float speed)
        {
            if (GetTurnAngles(direction, out float currentAngle, out float targetAngle))
                TurnTowards(currentAngle, targetAngle, speed);
        }
        
        /************************************************************************************************************************/

        protected virtual void OnAnimatorMove()
        {
            Vector3 movement = GetRootMotion();
            CheckGround(ref movement);
            UpdateGravity(ref movement);
            _CharacterController.Move(movement);

            IsGrounded = _CharacterController.isGrounded;

            transform.rotation *= _Character.Animancer.Animator.deltaRotation;
        }

        /************************************************************************************************************************/

        private Vector3 GetRootMotion()
        {
            Vector3 rawMotion = _Character.StateMachine.CurrentState.RootMotion;

            if (!_FullMovementControl ||// If Full Movement Control is disabled in the Inspector.
                !_Character.StateMachine.CurrentState.FullMovementControl)// Or the current state doesn't want it.
                return rawMotion;// Return the raw Root Motion.

            // If the Brain is not trying to control movement,
            // let the animation do what it wants (it's probably Idle or transitioning to Idle anyway).
            Vector3 direction = _Character.Parameters.MovementDirection;
            direction.y = 0;
            if (direction == Vector3.zero)
                return rawMotion;

            // Otherwise calculate the Root Motion only in the specified direction.

            float magnitude = direction.magnitude;
            direction /= magnitude;

            Vector3 controlledMotion = direction * Vector3.Dot(direction, rawMotion);

            // Interpolate towards that based on the desired movement magnitude (i.e. control stick tilt).
            // 0 tilt = use only the raw motion (would have already returned above to skip these calculations).
            // 1 tilt = use only the controlled motion.
            // And values in between give proportional motion between those values.
            return Vector3.Lerp(rawMotion, controlledMotion, magnitude);
        }

        /************************************************************************************************************************/

        private void CheckGround(ref Vector3 movement)
        {
            if (!_CharacterController.isGrounded)
                return;

            const float GroundedRayDistance = 1f;

            Ray ray = new(
                transform.position + GroundedRayDistance * 0.5f * Vector3.up,
                -Vector3.up);

            if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                GroundedRayDistance,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore))
            {
                // Rotate the movement to lie along the ground vector.
                movement = Vector3.ProjectOnPlane(movement, hit.normal);

                // Store the current walking surface so the correct audio is played.
                Renderer groundRenderer = hit.collider.GetComponentInChildren<Renderer>();
                GroundMaterial = groundRenderer ? groundRenderer.sharedMaterial : null;
            }
            else
            {
                GroundMaterial = null;
            }
        }

        /************************************************************************************************************************/

        private void UpdateGravity(ref Vector3 movement)
        {
            if (_CharacterController.isGrounded && _Character.StateMachine.CurrentState.StickToGround)
                _Character.Parameters.VerticalSpeed = -Gravity * StickingGravityProportion;
            else
                _Character.Parameters.VerticalSpeed -= Gravity * Time.deltaTime;

            movement.y += _Character.Parameters.VerticalSpeed * Time.deltaTime;
        }

        /************************************************************************************************************************/

        // Ignore these Animation Events because the attack animations will only start when we tell them to, so it
        // would be silly to use additional events for something we already directly caused. That sort of thing is only
        // necessary in Animator Controllers because they run their own logic to decide what they want to do.
        private void MeleeAttackStart(int throwing = 0) { }
        private void MeleeAttackEnd() { }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingPhysics3DModuleError(this);
        }

        /************************************************************************************************************************/

        public bool IsGrounded => default;

        public float CurrentTurnSpeed => default;

        public void UpdateSpeedControl() { }

        public bool GetTurnAngles(Vector3 direction, out float currentAngle, out float targetAngle)
        {
            currentAngle = default;
            targetAngle = default;
            return default;
        }

        public void TurnTowards(float currentAngle, float targetAngle, float speed) { }

        public void TurnTowards(Vector3 direction, float speed) { }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
