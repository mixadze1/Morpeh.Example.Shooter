using Cinemachine;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using UnityEngine;
using IComponent = Scellecs.Morpeh.IComponent;

namespace _Scripts.Core.Providers
{
    public class PlayerCameraProvider : MonoProvider<PlayerCameraComponent>
    {
        
    }

    [System.Serializable]
    public struct PlayerCameraComponent : IComponent
    {
        [SerializeField] private Camera _camera;

        [ReadOnly] public float XRotation;
        public Camera VirtualCamera => _camera;
    }
}