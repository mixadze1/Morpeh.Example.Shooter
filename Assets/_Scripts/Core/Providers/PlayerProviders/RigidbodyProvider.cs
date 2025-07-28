using System;
using Animancer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace _Scripts.Core.Providers.PlayerProviders
{
    public class RigidbodyProvider : MonoProvider<RigidbodyComponent>
    {
        public void Reset()
        {
            ref var data = ref GetData();
            data.SetRigidbody(GetComponent<Rigidbody>());
        }
    }

    [Serializable]
    public struct RigidbodyComponent : IComponent
    {
        [SerializeField] private Rigidbody _rigidbody;

        public Rigidbody Rigidbody => _rigidbody;

        public void SetRigidbody(Rigidbody rb)
        {
            if (rb == null)
            {
                Debug.LogError($"You are try setup null Rigidbody in RigidbodyComponent!");
                return;
            }
            
            _rigidbody = rb;
        }
    }
}