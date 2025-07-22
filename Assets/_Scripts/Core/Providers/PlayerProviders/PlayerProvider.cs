using System;
using _Scripts.Core.Providers.WeaponProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

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