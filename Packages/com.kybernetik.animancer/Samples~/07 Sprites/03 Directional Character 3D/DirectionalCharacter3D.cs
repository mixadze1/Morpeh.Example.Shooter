// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.Sprites
{
    /// <summary>A 3D version of the <see cref="DirectionalCharacter"/>.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/sprites/character-3d">
    /// Directional Character 3D</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Sprites/DirectionalCharacter3D
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Sprites - Directional Character 3D")]
    [AnimancerHelpUrl(typeof(DirectionalCharacter3D))]
    public class DirectionalCharacter3D : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        [Header("Physics")]
        [SerializeField] private Rigidbody _Rigidbody;
        [SerializeField, MetersPerSecond] private float _WalkSpeed = 1;
        [SerializeField, MetersPerSecond] private float _RunSpeed = 2;

        [Header("Animations")]
        [SerializeField] private DirectionalAnimations3D _Animancer;
        [SerializeField] private DirectionalSet<AnimationClip> _Idle;
        [SerializeField] private DirectionalSet<AnimationClip> _Walk;
        [SerializeField] private DirectionalSet<AnimationClip> _Run;
        [SerializeField] private DirectionalSet<AnimationClip> _Push;

        private Vector3 _Movement;
        private bool _IsPushing;

        public enum AnimationGroup
        {
            Other,
            Movement,
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            Vector2 input = SampleInput.WASD;
            if (input != Vector2.zero)
            {
                input = _Animancer.Animations.Snap(input);

                // Convert the input to 3D in the XZ plane.
                _Movement = new Vector3(input.x, 0, input.y);

                // Apply the camera's rotation and set the forward direction.
                Transform camera = _Animancer.Camera;
                _Movement = camera.TransformDirection(_Movement);
                _Movement.y = 0;
                _Movement.Normalize();
                _Animancer.Forward = _Movement;

                Play(GetMovementAnimations(), AnimationGroup.Movement);
            }
            else
            {
                _Movement = Vector3.zero;

                Play(_Idle, AnimationGroup.Other);
            }
        }

        /************************************************************************************************************************/

        private void Play(DirectionalSet<AnimationClip> animations, AnimationGroup group)
            => _Animancer.SetAnimations(animations, (int)group);

        /************************************************************************************************************************/

        private DirectionalSet<AnimationClip> GetMovementAnimations()
        {
            if (_IsPushing)
                return _Push;
            else if (SampleInput.LeftShiftHold)
                return _Run;
            else
                return _Walk;
        }

        /************************************************************************************************************************/

        protected virtual void OnCollisionEnter(Collision collision)
            => OnCollision(collision);

        protected virtual void OnCollisionStay(Collision collision)
            => OnCollision(collision);

        private void OnCollision(Collision collision)
        {
            if (_IsPushing)
                return;

            int contactCount = collision.contactCount;
            for (int i = 0; i < contactCount; i++)
            {
                ContactPoint contact = collision.GetContact(i);

                // If we're moving directly towards an object (or within 30 degrees of it), we are pushing it.
                if (Vector3.Angle(contact.normal, _Movement) > 180 - 30)
                {
                    _IsPushing = true;
                    return;
                }
            }
        }

        /************************************************************************************************************************/

        protected virtual void FixedUpdate()
        {
            _IsPushing = false;

            // Determine the desired speed based on the current animation.
            float speed = _Animancer.Animations == _Run ? _RunSpeed : _WalkSpeed;

#if UNITY_6000_0_OR_NEWER
            _Rigidbody.linearVelocity = _Movement * speed;
#else
            _Rigidbody.velocity = _Movement * speed;
#endif
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        protected virtual void OnValidate()
        {
            if (_Animancer != null)
                _Animancer.Animations = _Idle;
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingPhysics3DModuleError(this);
        }
        
        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
