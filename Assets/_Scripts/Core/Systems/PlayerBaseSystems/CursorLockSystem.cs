using Scellecs.Morpeh;
using UnityEngine;

namespace _Scripts.Core.Systems.PlayerBaseSystems
{
    public class CursorLockSystem : ISystem
    {
        public World World { get; set; }

        public void OnAwake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnUpdate(float deltaTime)
        {
        }

        public void Dispose()
        {
        }
    }
}