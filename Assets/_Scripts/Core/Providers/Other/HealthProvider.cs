using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace _Scripts.Core.Providers.Other
{
    public class HealthProvider : MonoProvider<HealthComponent>
    {
        public void Reset()
        {
            ref var component = ref GetData();
            component.SetupHealth(100);
        }
    }

    [Serializable]
    public struct HealthComponent : IComponent
    {
        [SerializeField] private int _health;

        public bool IsDeath() => 
            _health <= 0;

        public void SetupHealth(int value) => 
            _health = value;

        public void GetDamage(int value)
        {
            _health -= value;
        }
    }
}