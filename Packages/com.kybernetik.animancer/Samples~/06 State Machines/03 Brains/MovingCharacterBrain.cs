// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>Uses player input to control a <see cref="Character"/>.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/brains">
    /// Brains</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/MovingCharacterBrain
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Brains - Moving Character Brain")]
    [AnimancerHelpUrl(typeof(MovingCharacterBrain))]
    public class MovingCharacterBrain : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private Character _Character;
        [SerializeField] private CharacterState _Move;
        [SerializeField] private CharacterState _Action;

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            UpdateMovement();
            UpdateAction();
        }

        /************************************************************************************************************************/

        private void UpdateMovement()
        {
            Vector2 input = SampleInput.WASD;
            if (input != Vector2.zero)
            {
                // Convert the input to 3D in the XZ plane.
                Vector3 movementDirection = new Vector3(input.x, 0, input.y);

                // Apply the camera's rotation and set the parameter.
                Transform camera = Camera.main.transform;
                movementDirection = camera.TransformDirection(movementDirection);
                _Character.Parameters.MovementDirection = movementDirection;

                // Enter the movement state if we aren't already in it.
                _Character.StateMachine.TrySetState(_Move);
            }
            else// If we aren't trying to move, clear the movement vector and return to idle.
            {
                _Character.Parameters.MovementDirection = Vector3.zero;
                _Character.StateMachine.TrySetDefaultState();
            }

            // Try to run while holding shift.
            _Character.Parameters.WantsToRun = SampleInput.LeftShiftHold;
        }

        /************************************************************************************************************************/

        private void UpdateAction()
        {
            if (SampleInput.LeftMouseUp)
                _Character.StateMachine.TryResetState(_Action);
        }

        /************************************************************************************************************************/
    }
}
