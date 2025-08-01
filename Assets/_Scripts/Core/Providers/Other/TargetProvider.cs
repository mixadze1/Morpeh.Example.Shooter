using System;
using _Scripts.Core.Providers.PlayerProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace _Scripts.Core.Providers.Other
{
    [RequireComponent(typeof(HealthProvider))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(TransformComponent))]
    public class TargetProvider : MonoProvider<TargetComponent>
    {
        public void Reset()
        {
            this.gameObject.layer = 6;
        }
    }

    [Serializable]
    public struct TargetComponent : IComponent
    {
        
    }
}