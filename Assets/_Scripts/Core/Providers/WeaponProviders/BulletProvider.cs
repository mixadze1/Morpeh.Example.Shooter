using System;
using System.Collections.Generic;
using _Scripts.Core.Configs.WeaponConfigs;
using _Scripts.Core.Providers.PlayerProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Core.Providers.WeaponProviders
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(TransformProvider))]
    [RequireComponent(typeof(RigidbodyComponent))]
    public class BulletProvider : MonoProvider<BulletComponent>
    {
        #if UNITY_EDITOR
        private class TrailPoint
        {
            public Vector3 Position;
            public float Timestamp;

            public TrailPoint(Vector3 pos)
            {
                Position = pos;
                Timestamp = Time.time;
            }
        }

        private readonly List<TrailPoint> _trailPoints = new();

        private const float TRAIL_LIFETIME = 1f;

        private void Update()
        {
            _trailPoints.Add(new TrailPoint(transform.position));

            float currentTime = Time.time;
            _trailPoints.RemoveAll(p => currentTime - p.Timestamp > TRAIL_LIFETIME);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            for (int i = 1; i < _trailPoints.Count; i++)
            {
                Gizmos.DrawLine(_trailPoints[i - 1].Position, _trailPoints[i].Position);
            }
        }
        #endif
    }

    [Serializable]
    public struct BulletComponent : IComponent
    {
        [SerializeField, ReadOnly] private float _timeAlive;
        [SerializeField, ReadOnly] private float _lifeTime;

        [ReadOnly] public TypeWeapon FromWeaponType;

       public bool IsEndLife() => 
           _timeAlive >= _lifeTime;

       public void SetupLifeTime(float value) => 
           _lifeTime = value;

       public void AddTimeAlive(float value) => 
           _timeAlive += value;
    }
}