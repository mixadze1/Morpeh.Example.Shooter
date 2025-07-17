using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace _Scripts.Core.Providers.PlayerProviders
{
    public class TransformProvider : MonoProvider<TransformComponent>
    {
        private void Reset()
        {
            ref TransformComponent unityTransform = ref GetData();
            unityTransform.Transform = this.transform;
        }
    }
    
    [Serializable]
    public struct TransformComponent : IComponent
    {
        public Transform Transform;    
    }
}