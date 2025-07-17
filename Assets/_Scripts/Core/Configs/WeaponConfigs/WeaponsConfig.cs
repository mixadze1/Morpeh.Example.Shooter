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
        [SerializeField] private SerializedDictionary<TypeWeapon, WeaponConfig> _weapons;

        public IReadOnlyDictionary<TypeWeapon, WeaponConfig> Weapons => _weapons;
    }

    [Serializable]
    public class WeaponConfig
    {
        [SerializeField] private float _speedBullet;
        [SerializeField] private BulletProvider _bulletPrefab;

        public float SpeedBullet => _speedBullet;
        public BulletProvider BulletPrefab => _bulletPrefab;
    }
}

public enum TypeWeapon
{
    Default,
    Ak,
    M4
}