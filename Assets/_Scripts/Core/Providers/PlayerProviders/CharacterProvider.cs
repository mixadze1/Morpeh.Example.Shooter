using System;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using UnityEngine;
using IComponent = Scellecs.Morpeh.IComponent;

namespace _Scripts.Core.Providers.PlayerProviders
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterProvider : MonoProvider<CharacterComponent>
    {
        public void Reset()
        {
            ref var component = ref GetData();
            component.SetupCharacter(GetComponent<CharacterController>());
        }
    }

    [Serializable]
    public struct CharacterComponent : IComponent
    {
        [SerializeField] private CharacterController _characterController;

        [SerializeField, ReadOnly] private Vector3 _velocity;
        
        public Vector3 PlayerVelocity => _velocity;
        public CharacterController CharacterController => _characterController;

        public bool IsGround => _characterController.isGrounded;

        public void SetupCharacter(CharacterController character) => 
            _characterController = character;

        public void SetVelocity(Vector3 v) => 
            _velocity = v;

        public void SetVelocityY(float y) 
            => _velocity.y = y;
    }
}