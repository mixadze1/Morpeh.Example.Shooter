// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using Animancer.Units;
using System;
using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>Uses player input to control a <see cref="Character"/>.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/weapons">
    /// Weapons</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/WeaponsCharacterBrain
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Weapons - Weapons Character Brain")]
    [AnimancerHelpUrl(typeof(WeaponsCharacterBrain))]
    public class WeaponsCharacterBrain : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private Character _Character;
        [SerializeField] private CharacterState _Move;
        [SerializeField] private CharacterState _Attack;
        [SerializeField, Seconds] private float _InputTimeOut = 0.5f;
        [SerializeField] private EquipState _Equip;
        [SerializeField] private Weapon[] _Weapons;

        private StateMachine<CharacterState>.InputBuffer _InputBuffer;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _InputBuffer = new StateMachine<CharacterState>.InputBuffer(_Character.StateMachine);
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            UpdateMovement();
            UpdateEquip();
            UpdateAction();

            _InputBuffer.Update();
        }

        /************************************************************************************************************************/

        private void UpdateMovement()// This method is identical to the one in MovingCharacterBrain.
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

                // Enter the locomotion state if we aren't already in it.
                _Character.StateMachine.TrySetState(_Move);
            }
            else
            {
                _Character.Parameters.MovementDirection = Vector3.zero;
                _Character.StateMachine.TrySetDefaultState();
            }

            // Indicate whether the character wants to run or not.
            _Character.Parameters.WantsToRun = SampleInput.LeftShiftHold;
        }

        /************************************************************************************************************************/

        private void UpdateEquip()
        {
            if (SampleInput.RightMouseDown)
            {
                int equippedWeaponIndex = Array.IndexOf(_Weapons, _Character.Equipment.Weapon);

                equippedWeaponIndex++;
                if (equippedWeaponIndex >= _Weapons.Length)
                    equippedWeaponIndex = 0;

                _Equip.NextWeapon = _Weapons[equippedWeaponIndex];
                _InputBuffer.Buffer(_Equip, _InputTimeOut);
            }
        }

        /************************************************************************************************************************/

        private void UpdateAction()
        {
            if (SampleInput.LeftMouseDown)
            {
                _InputBuffer.Buffer(_Attack, _InputTimeOut);
            }
        }

        /************************************************************************************************************************/
    }
}
