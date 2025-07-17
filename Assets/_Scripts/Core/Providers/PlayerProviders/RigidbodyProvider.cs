using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace _Scripts.Core.Providers.PlayerProviders
{
    public class RigidbodyProvider : MonoProvider<RigidbodyComponent>
    {
        
    }

    [Serializable]
    public struct RigidbodyComponent : IComponent
    {
        public Rigidbody Rigidbody;
        public Vector3 Direction;
    }
}