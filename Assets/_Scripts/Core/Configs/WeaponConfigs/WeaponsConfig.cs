using System;
using System.Collections.Generic;
using _Scripts.Core.Providers.WeaponProviders;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Scripts.Core.Configs.WeaponConfigs
{
    [CreateAssetMenu]
    public class WeaponsConfig : ScriptableObject
    {
        [SerializeField] private float _lifetimeBullet = 5;
        [SerializeField] private float _lifetimeDecal = 15;
        [SerializeField] private SerializedDictionary<TypeWeapon, WeaponConfig> _weapons;

        public IReadOnlyDictionary<TypeWeapon, WeaponConfig> Weapons => _weapons;
        public float LifetimeBullet => _lifetimeBullet;
        public float LifetimeDecal => _lifetimeDecal;
    }


    public enum TypeWeapon
    {
        Default,
        Ak,
        Mk16,
        Ar25,
        HandGun
    }
}