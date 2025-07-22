// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.AnimatorControllers.GameKit
{
    /// <summary>
    /// A <see cref="CharacterState"/> which moves the character according to their
    /// <see cref="CharacterBrain.Movement"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/animator-controllers/3d-game-kit/locomotion">
    /// 3D Game Kit/Locomotion</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.AnimatorControllers.GameKit/LocomotionState
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Game Kit - Locomotion State")]
    [AnimancerHelpUrl(typeof(LocomotionState))]
    public class LocomotionState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField] private LinearMixerTransition _LocomotionMixer;
        [SerializeField] private ClipTransition _QuickTurnLeft;
        [SerializeField] private ClipTransition _QuickTurnRight;
        [SerializeField, MetersPerSecond] private float _QuickTurnMoveSpeed = 2;
        [SerializeField, Degrees] private float _QuickTurnAngle = 145;

        private AnimatedFloat _FootFall;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _QuickTurnLeft.Events.OnEnd =
                _QuickTurnRight.Events.OnEnd =
                () => Character.Animancer.Play(_LocomotionMixer);

            _FootFall = new(Character.Animancer, "FootFall");
        }

        /************************************************************************************************************************/

        public override bool CanEnterState => Character.Movement.IsGrounded;

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            Character.Animancer.Play(_LocomotionMixer);
        }

        /************************************************************************************************************************/

        protected virtual void FixedUpdate()
        {
            if (Character.CheckMotionState())
                return;

            Character.Movement.UpdateSpeedControl();
            _LocomotionMixer.State.Parameter = Character.Parameters.ForwardSpeed;

            UpdateRotation();
            UpdateAudio();
        }

        /************************************************************************************************************************/

        private void UpdateRotation()
        {
            // If the default locomotion state is not active we must be performing a quick turn.
            // Those animations use root motion to perform the turn so we don't want any scripted rotation during them.
            if (!_LocomotionMixer.State.IsCurrent)
                return;

            if (!Character.Movement.GetTurnAngles(
                Character.Parameters.MovementDirection,
                out float currentAngle,
                out float targetAngle))
                return;

            // Check if we should play a quick turn animation:

            // If we're moving fast enough.
            if (Character.Parameters.ForwardSpeed > _QuickTurnMoveSpeed)
            {
                // And turning sharp enough.
                float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
                if (Mathf.Abs(deltaAngle) > _QuickTurnAngle)
                {
                    // Determine which way we're turning.
                    ClipTransition turn = deltaAngle < 0 ? _QuickTurnLeft : _QuickTurnRight;

                    // Make sure the desired turn is not already active so we don't keep using it repeatedly.
                    if (turn.State == null || turn.State.Weight == 0)
                    {
                        Character.Animancer.Play(turn);

                        // Now that we're quick turning, we don't want to apply the scripted turning below.
                        return;
                    }
                }
            }

            Character.Movement.TurnTowards(currentAngle, targetAngle, Character.Movement.CurrentTurnSpeed);
        }

        /************************************************************************************************************************/

        [SerializeField] private UnityEvent _PlayFootstepAudio;// See the Read Me.

        private bool _CanPlayAudio;
        private bool _IsPlayingAudio;

        // This is the same logic used for locomotion audio in the original PlayerController.
        private void UpdateAudio()
        {
            const float Threshold = 0.01f;

            float footFallValue = _FootFall.Value;
            if (footFallValue > Threshold && !_IsPlayingAudio && _CanPlayAudio)
            {
                _IsPlayingAudio = true;
                _CanPlayAudio = false;

                // The full 3D Game Kit has different footstep sounds depending on the ground material and your speed
                // so it calls RandomAudioPlayer.PlayRandomClip with those parameters:
                //_FootstepAudio.PlayRandomClip(Character.GroundMaterial, Character.ForwardSpeed < 4 ? 0 : 1);

                // Unfortunately UnityEvents can't call methods with multiple parameters (UltEvents can), but it
                // doesn't realy matter because the 3D Game Kit Lite only has one set of footstep sounds anyway.

                _PlayFootstepAudio.Invoke();
            }
            else if (_IsPlayingAudio)
            {
                _IsPlayingAudio = false;
            }
            else if (footFallValue < Threshold && !_CanPlayAudio)
            {
                _CanPlayAudio = true;
            }
        }

        /************************************************************************************************************************/
    }
}
