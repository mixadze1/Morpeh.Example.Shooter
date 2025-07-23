using System;
using System.Collections.Generic;
using _Scripts.Core.Providers.WeaponProviders;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

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

    [Serializable]
    public class WeaponConfig
    {
        [SerializeField] private float _speedBullet;
        [SerializeField] private float _shootPerSecond;
        [SerializeField] private BulletProvider _bulletPrefab; 
        [SerializeField] private PlayerWeaponProvider _playerWeaponPreset;
        [SerializeField] private WeaponMagazineConfig _magazineConfig;
        [SerializeField] private DecalProvider _decalWallProvider;
        
        public DecalProvider DecalWallProvider => _decalWallProvider;
        public float SpeedBullet => _speedBullet;   
        public BulletProvider BulletPrefab => _bulletPrefab;
        public PlayerWeaponProvider WeaponPreset => _playerWeaponPreset;
        public WeaponMagazineConfig MagazineConfig => _magazineConfig;
        public SerializedDictionary<Trigger, AnimationData> AnimationConfig;
        public float ShootPerSecond => _shootPerSecond;
    }
}

public enum TypeWeapon
{
    Default,
    Ak,
    Mk16,
    Ar,
    HandGun
}