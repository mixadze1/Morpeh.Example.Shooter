// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>Uses player input to control a <see cref="Character"/>.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/characters">
    /// Characters</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/BasicCharacterBrain
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Characters - Basic Character Brain")]
    [AnimancerHelpUrl(typeof(BasicCharacterBrain))]
    public class BasicCharacterBrain : MonoBehaviour
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
            float forward = SampleInput.WASD.y;
            if (forward > 0)
            {
                _Character.StateMachine.TrySetState(_Move);
            }
            else
            {
                _Character.StateMachine.TrySetDefaultState();
            }
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
