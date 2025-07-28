using System;
using _Scripts.Core.Configs.WeaponConfigs;
using Animancer;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Core.Providers.WeaponProviders
{
    [RequireComponent(typeof(AnimancerComponent))]
    [RequireComponent(typeof(AnimancerProvider))]
    public class PlayerWeaponProvider : MonoProvider<WeaponComponent>
    {
        
    }

    [Serializable]
    public struct WeaponComponent : IComponent
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private TypeWeapon _typeWeapon;
        
        [Header("Weapon Model")]
        [SerializeField] private int _amountAmmoInMagazine;
        [SerializeField] private int _maxAmmoInMagazine;
        [SerializeField] private int _amountMagazines;

        public int AmountAmmo => _amountAmmoInMagazine;
        public int MaxAmmoInMagazine => _maxAmmoInMagazine;
        public int AmountMagazines => _amountMagazines;

        public void InitializeAmmo(WeaponMagazineConfig config)
        {
            if (config == null)
            {
                Debug.LogError($"You are try setup null config in weapon {_typeWeapon}!");
                return;
            }
            _amountAmmoInMagazine = config.MaxAmmoInMagazine;
            _maxAmmoInMagazine = config.MaxAmmoInMagazine;
            _amountMagazines = config.AmountMagazines;;
        }

        public bool TryShoot() => 
            _amountAmmoInMagazine > 0;

        public bool TryReload() => 
            _amountMagazines > 0;

        public void Reload()
        {
            _amountMagazines--;
            _amountAmmoInMagazine = _maxAmmoInMagazine;
            CustomDebug.Log($"[Player] Reload Magazines: {_amountMagazines}, ammo: {_amountAmmoInMagazine}", new Color(1f, 0.69f, 0.51f));
        }

        public void OnShoot()
        {
            _amountAmmoInMagazine--;
            CustomDebug.Log($"[Player] Shoot! Left: {AmountAmmo}/{MaxAmmoInMagazine}", Color.white);
        }
        
        public Transform ShootPoint => _shootPoint;
        public TypeWeapon TypeWeapon => _typeWeapon;
    }

    [Serializable]
    public class WeaponMagazineConfig
    {
        public int MaxAmmoInMagazine = 30;
        public int AmountMagazines = 6;
    }
}