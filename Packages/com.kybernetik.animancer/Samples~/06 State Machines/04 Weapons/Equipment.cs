// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>Manages the items equipped by a <see cref="Character"/>.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/weapons">
    /// Weapons</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/Equipment
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Weapons - Equipment")]
    [AnimancerHelpUrl(typeof(Equipment))]
    public class Equipment : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private Transform _WeaponHolder;
        [SerializeField] private Weapon _Weapon;

        /************************************************************************************************************************/

        public Weapon Weapon
        {
            get => _Weapon;
            set
            {
                DetachWeapon();
                _Weapon = value;
                AttachWeapon();
            }
        }

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            AttachWeapon();
        }

        /************************************************************************************************************************/

        private void AttachWeapon()
        {
            if (_Weapon == null)
                return;

            Transform transform = _Weapon.transform;
            transform.parent = _WeaponHolder;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.localScale = Vector3.one;

            _Weapon.gameObject.SetActive(true);
        }

        /************************************************************************************************************************/

        private void DetachWeapon()
        {
            if (_Weapon == null)
                return;

            _Weapon.transform.parent = transform;
            _Weapon.gameObject.SetActive(false);
        }

        /************************************************************************************************************************/
    }
}
