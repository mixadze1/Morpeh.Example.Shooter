// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer.Samples.Sprites
{
    /// <summary>
    /// A more complex version of the <see cref="DirectionalBasics"/> which adds
    /// running and pushing animations as well as the ability to actually move around.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/sprites/character">
    /// Directional Character</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Sprites/DirectionalCharacter
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Sprites - Directional Character")]
    [AnimancerHelpUrl(typeof(DirectionalCharacter))]
    public class DirectionalCharacter : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_2D
        /************************************************************************************************************************/

        [Header("Physics")]
        [SerializeField] private CapsuleCollider2D _Collider;
        [SerializeField] private Rigidbody2D _Rigidbody;
        [SerializeField, MetersPerSecond] private float _WalkSpeed = 1;
        [SerializeField, MetersPerSecond] private float _RunSpeed = 2;

        [Header("Animations")]
        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private DirectionalSet<AnimationClip> _Idle;
        [SerializeField] private DirectionalSet<AnimationClip> _Walk;
        [SerializeField] private DirectionalSet<AnimationClip> _Run;
        [SerializeField] private DirectionalSet<AnimationClip> _Push;
        [SerializeField] private Vector2 _Facing = Vector2.down;

        private Vector2 _Movement;
        private DirectionalSet<AnimationClip> _CurrentAnimationSet;

        private static readonly TimeSynchronizer<AnimationGroup>
            TimeSynchronizer = new();

        public enum AnimationGroup
        {
            Other,
            Movement,
        }

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _CurrentAnimationSet = _Idle;
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            _Movement = SampleInput.WASD;
            if (_Movement != Vector2.zero)
            {
                // Snap the movement to the exact directions we have animations for.
                // When using DirectionalAnimationSets this means the character will only move up/right/down/left.
                // But DirectionalAnimationSet8s will allow diagonal movement as well.
                _Movement = _CurrentAnimationSet.Snap(_Movement);
                _Movement = Vector2.ClampMagnitude(_Movement, 1);

                _Facing = _Movement;

                UpdateMovementState();
            }
            else
            {
                Play(_Idle, AnimationGroup.Other);
            }
        }

        /************************************************************************************************************************/

        private void Play(DirectionalSet<AnimationClip> animations, AnimationGroup group)
        {
            // Store the current time.
            TimeSynchronizer.StoreTime(_Animancer);

            _CurrentAnimationSet = animations;
            _Animancer.Play(animations.Get(_Facing));

            // If the new animation is in the synchronization group, give it the same time the previous animation had.
            TimeSynchronizer.SyncTime(_Animancer, group);
        }

        /************************************************************************************************************************/

        // Pre-allocate a list of contacts so Unity doesn't need to allocate a new one every time.
        // It's static because every character can use the same list one at a time.
        private static readonly List<ContactPoint2D> Contacts = new();

        private void UpdateMovementState()
        {
            int contactCount = _Collider.GetContacts(Contacts);
            for (int i = 0; i < contactCount; i++)
            {
                // If we're moving directly towards an object (or within 30 degrees of it), we are pushing it.
                if (Vector2.Angle(Contacts[i].normal, _Movement) > 180 - 30)
                {
                    Play(_Push, AnimationGroup.Movement);
                    return;
                }
            }

            DirectionalSet<AnimationClip> animations = SampleInput.LeftShiftHold ? _Run : _Walk;
            Play(animations, AnimationGroup.Movement);
        }

        /************************************************************************************************************************/

        protected virtual void FixedUpdate()
        {
            // Determine the desired speed based on the current animation.
            float speed = _CurrentAnimationSet == _Run
                ? _RunSpeed
                : _WalkSpeed;

#if UNITY_6000_0_OR_NEWER
            _Rigidbody.linearVelocity = _Movement * speed;
#else
            _Rigidbody.velocity = _Movement * speed;
#endif
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Sets the character's starting sprite in Edit Mode so you can see it while working in the scene.
        /// </summary>
        /// <remarks>Called in Edit Mode whenever this script is loaded or a value is changed in the Inspector.</remarks>
        protected virtual void OnValidate()
        {
            if (_Idle != null)
                _Idle.Get(_Facing).EditModePlay(_Animancer);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingPhysics2DModuleError(this);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
