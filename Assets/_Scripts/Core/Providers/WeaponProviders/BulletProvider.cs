using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Core.Providers.WeaponProviders
{
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

    public struct BulletComponent : IComponent
    {
        [ReadOnly] public Vector3 Direction;
    }
}