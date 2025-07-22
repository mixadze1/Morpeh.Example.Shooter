// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using Animancer.Units;
using UnityEngine;
using static Animancer.Validate;

namespace Animancer.Samples.AnimatorControllers.GameKit
{
    /// <summary>A brain which controls the character using keyboard input.</summary>
    /// <remarks>
    /// This class serves the same purpose as <c>PlayerInput</c> from the 3D Game Kit.
    /// <para></para>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/animator-controllers/3d-game-kit">
    /// 3D Game Kit</see>
    /// </remarks>
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.AnimatorControllers.GameKit/GameKitCharacterBrain
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Game Kit - Game Kit Character Brain")]
    [AnimancerHelpUrl(typeof(GameKitCharacterBrain))]
    public class GameKitCharacterBrain : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private Character _Character;
        [SerializeField] private AirborneState _Jump;
        [SerializeField] private CharacterState _Attack;

        [SerializeField]
        [Seconds(Rule = Value.IsNotNegative)]
        private float _AttackInputTimeOut = 0.5f;

        private StateMachine<CharacterState>.InputBuffer _InputBuffer;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _InputBuffer = new(_Character.StateMachine);
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            UpdateMovement();
            UpdateActions();
        }

        /************************************************************************************************************************/

        private void UpdateMovement()
        {
            Vector2 input = SampleInput.WASD;
            if (input == Vector2.zero)
            {
                _Character.Parameters.MovementDirection = Vector3.zero;
                return;
            }

            // Convert the input to 3D in the XZ plane.
            Vector3 movementDirection = new Vector3(input.x, 0, input.y);

            // Apply the camera's rotation and set the parameter.
            Transform camera = Camera.main.transform;
            movementDirection = camera.TransformDirection(movementDirection);
            _Character.Parameters.MovementDirection = movementDirection;
        }

        /************************************************************************************************************************/

        private void UpdateActions()
        {
            // Jump gets priority for better platforming.
            if (SampleInput.SpaceDown)
            {
                _Jump.TryJump();
            }
            else if (SampleInput.SpaceUp)
            {
                _Jump.CancelJump();
            }

            if (SampleInput.LeftMouseDown)
            {
                _InputBuffer.Buffer(_Attack, _AttackInputTimeOut);
            }

            _InputBuffer.Update();
        }

        /************************************************************************************************************************/
    }
}
