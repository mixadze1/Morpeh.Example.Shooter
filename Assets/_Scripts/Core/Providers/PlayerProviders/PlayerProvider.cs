using System;
using Animancer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace _Scripts.Core.Providers.PlayerProviders
{
    public class PlayerProvider : MonoProvider<PlayerComponent>
    {
        
    }

    [Serializable]
    public struct PlayerComponent : IComponent
    {
        [SerializeField] private Transform _pointForSpawn;
        public Transform PointForSpawnWeapon => _pointForSpawn;
        public Entity WeaponEntity { get; private set; }

        public void SetupWeaponEntity(Entity entity) => 
            WeaponEntity = entity;
    }
}