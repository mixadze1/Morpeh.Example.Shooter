// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>
    /// A centralised group of references to the common parts of a character and a state machine for their actions.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/characters">
    /// Characters</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/Character
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Characters - Character")]
    [AnimancerHelpUrl(typeof(Character))]
    [DefaultExecutionOrder(-10000)]// Initialize the StateMachine before anything uses it.
    public class Character : MonoBehaviour
    {
        /************************************************************************************************************************/
        // Used in the Characters sample.
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;
        public AnimancerComponent Animancer => _Animancer;

        /************************************************************************************************************************/

        [SerializeField]
        private StateMachine<CharacterState>.WithDefault _StateMachine;
        public StateMachine<CharacterState>.WithDefault StateMachine => _StateMachine;

        protected virtual void Awake()
        {
            _StateMachine.InitializeAfterDeserialize();
        }

        /************************************************************************************************************************/
        // Used in the Interruptions sample.
        /************************************************************************************************************************/

        [SerializeField]
        private HealthPool _Health;
        public HealthPool Health => _Health;

        /************************************************************************************************************************/
        // Used in the Brains sample.
        /************************************************************************************************************************/

        [SerializeField]
        private CharacterParameters _Parameters;
        public CharacterParameters Parameters => _Parameters;

        /************************************************************************************************************************/
        // Used in the Weapons sample.
        /************************************************************************************************************************/

        [SerializeField]
        private Equipment _Equipment;
        public Equipment Equipment => _Equipment;

        /************************************************************************************************************************/
    }
}
