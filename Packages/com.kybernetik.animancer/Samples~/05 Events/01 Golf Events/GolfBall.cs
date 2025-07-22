// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.Events
{
    /// <summary>Manages the physics of a golf ball.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/events/golf">
    /// Golf Events</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Events/GolfBall
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Golf Events - Golf Ball")]
    [AnimancerHelpUrl(typeof(GolfBall))]
    public class GolfBall : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        [SerializeField] private Rigidbody _Rigidbody;
        [SerializeField] private Vector3 _HitVelocity = new(0, 10, 10);
        [SerializeField, Meters] private float _ReturnHeight = -10;

        private Vector3 _StartPosition;

        /************************************************************************************************************************/

        public bool ReadyToHit
            => _Rigidbody.isKinematic;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _Rigidbody.isKinematic = true;
            _StartPosition = _Rigidbody.position;
        }

        /************************************************************************************************************************/

        protected virtual void FixedUpdate()
        {
            if (_Rigidbody.position.y < _ReturnHeight)
            {
                _Rigidbody.isKinematic = true;
                _Rigidbody.position = _StartPosition;
            }
        }

        /************************************************************************************************************************/

        public void Hit()
        {
            _Rigidbody.isKinematic = false;

#if UNITY_6000_0_OR_NEWER
            _Rigidbody.linearVelocity = _HitVelocity;
#else
            _Rigidbody.velocity = _HitVelocity;
#endif
        }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/
        
        protected virtual void Awake()
        {
            SampleModules.LogMissingPhysics3DModuleError(this);
        }

        public bool ReadyToHit
            => false;

        public void Hit() { }
        
        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
