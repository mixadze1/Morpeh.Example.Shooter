using System;
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
        public Transform PointForSpawnWeapon;
        public Entity WeaponEntity;
    }
}