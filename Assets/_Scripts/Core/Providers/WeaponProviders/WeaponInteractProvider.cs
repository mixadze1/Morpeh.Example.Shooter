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
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class WeaponInteractProvider : MonoProvider<WeaponInteractComponent>
    {
        public void Reset()
        {
            this.GetComponent<MeshCollider>().convex = true;
            gameObject.layer = 7; 
        }
    }
    
    [Serializable]
    public struct WeaponInteractComponent : IComponent
    {
        [SerializeField, ValidateInput(nameof(ValidateTypeWeapon))] private TypeWeapon _typeWeapon;

        public TypeWeapon Type => _typeWeapon;
        
        private bool ValidateShootPoint(Transform t) => 
            t != null;

        private bool ValidateTypeWeapon(TypeWeapon t) => 
            t != TypeWeapon.Default;
    }
}