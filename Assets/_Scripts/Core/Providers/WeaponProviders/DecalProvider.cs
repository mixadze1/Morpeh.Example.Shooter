
using System;
using _Scripts.Core.Providers.PlayerProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Core.Providers.WeaponProviders
{
    [RequireComponent(typeof(TransformProvider))]
    public class DecalProvider : MonoProvider<DecalComponent>
    {
        
    }

    [Serializable]
    public struct DecalComponent : IComponent
    {
        [SerializeField, ReadOnly] private float _lifeTime;
        [SerializeField, ReadOnly] private float _timeAlive;

        public bool IsEndLife()
        {
            return _timeAlive >= _lifeTime;
        }

        public void SetupLifeTime(float lifeTime)
        {
            if (lifeTime <= 0)
            {
                Debug.LogError($"Life time decal {lifeTime} <= 0. Strange Value!");
                return;
            }

            _lifeTime = lifeTime;
        }

        public void AddTimeAlive(float value)
        {
            if (value <= 0)
            {
                Debug.LogError($"You are try setup negative values {value} <= 0!");
                return;
            }

            _timeAlive += value;
        }
    }
}