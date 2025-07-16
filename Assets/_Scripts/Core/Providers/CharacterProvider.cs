using System;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using UnityEngine;
using IComponent = Scellecs.Morpeh.IComponent;

namespace _Scripts.Core.Providers
{
    public class CharacterProvider : MonoProvider<CharacterComponent>
    {
        
    }

    [Serializable]
    public struct CharacterComponent : IComponent
    {
        [SerializeField] private CharacterController _characterController;

        public CharacterController CharacterController => _characterController;
       [ReadOnly] public Vector3 PlayerVelocity;
        public bool IsGround => _characterController.isGrounded;
    }
}