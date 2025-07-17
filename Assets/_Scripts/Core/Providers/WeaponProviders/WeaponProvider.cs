using System;
using _Scripts.Core.Providers.PlayerProviders;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Core.Providers.WeaponProviders
{
    [RequireComponent(typeof(TransformProvider))]
    [RequireComponent(typeof(InteractProvider))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class WeaponProvider : MonoProvider<WeaponComponent>
    {
        public void Reset()
        {
            this.GetComponent<MeshCollider>().convex = true;
            gameObject.layer = 7; 
        }
    }

    [Serializable]
    public struct WeaponComponent : IComponent
    {
        [SerializeField, ValidateInput(nameof(ValidateShootPoint))] private Transform _shootPoint;
        [SerializeField, ValidateInput(nameof(ValidateTypeWeapon))] private TypeWeapon _typeWeapon;

        public Transform ShootPoint => _shootPoint;
        public TypeWeapon Type => _typeWeapon;
        
        private bool ValidateShootPoint(Transform t) => 
            t != null;

        private bool ValidateTypeWeapon(TypeWeapon t) => 
            t != TypeWeapon.Default;
    }
}