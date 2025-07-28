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
            unityTransform.SetupTransform(this.transform);
        }
    }
    
    [Serializable]
    public struct TransformComponent : IComponent
    {
        [SerializeField] private Transform _transform;
        public Transform Transform => _transform;

        public void SetupTransform(Transform transform)
        {
            if (transform == null)
            {
                Debug.LogError($"In Transform Provider transform is Null. Check GameObject!");
                return;
            }
            
            _transform = transform;
        }
    }
}