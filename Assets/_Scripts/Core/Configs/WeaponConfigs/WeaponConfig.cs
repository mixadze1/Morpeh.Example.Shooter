using _Scripts.Core.Providers.WeaponProviders;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Core.Configs.WeaponConfigs
{
    [CreateAssetMenu(menuName = "Configs/Weapon", fileName = nameof(WeaponConfig))]
    public class WeaponConfig : ScriptableObject
    {
        [BoxGroup("General")]
        [SerializeField, Range(5, 1500), LabelText("Bullet speed m/s")] private float _speedBullet;

        [BoxGroup("General")]
        [SerializeField, Range(0.005f, 5f)] private float _shootPerSecond;

        [BoxGroup("General"), SerializeField, LabelText("Magazine Settings")] 
        private WeaponMagazineConfig _magazineConfig;

        [Title("References")]
        [LabelText("Bullet Prefab"), Required]
        [SerializeField] private BulletProvider _bulletPrefab;

        [LabelText("Player Weapon Preset"), Required]
        [SerializeField] private PlayerWeaponProvider _playerWeaponPreset;

        [LabelText("Decal Provider (Wall)"), Required]
        [SerializeField] private DecalProvider _decalWallProvider;

        [SerializedDictionary("Trigger", "Animation")]
        public SerializedDictionary<Trigger, AnimationData> AnimationConfig;

        public float SpeedBullet => _speedBullet;
        public float ShootPerSecond => _shootPerSecond;
        public BulletProvider BulletPrefab => _bulletPrefab;
        public PlayerWeaponProvider WeaponPreset => _playerWeaponPreset;
        public DecalProvider DecalWallProvider => _decalWallProvider;
        public WeaponMagazineConfig MagazineConfig => _magazineConfig;
    }
}