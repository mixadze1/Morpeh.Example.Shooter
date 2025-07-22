// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Sprites
{
    /// <summary>
    /// Animates a character to either stand idle or walk using animations
    /// defined in <see cref="DirectionalSet{T}"/>s.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/sprites/basics">
    /// Directional Basics</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Sprites/DirectionalBasics
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Sprites - Directional Basics")]
    [AnimancerHelpUrl(typeof(DirectionalBasics))]
    public class DirectionalBasics : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private DirectionalSet<AnimationClip> _Idles;
        [SerializeField] private DirectionalSet<AnimationClip> _Walks;
        [SerializeField] private Vector2 _Facing = Vector2.down;

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            Vector2 input = SampleInput.WASD;
            if (input != Vector2.zero)
            {
                _Facing = input;

                Play(_Walks);

                // Play could return the AnimancerState it gets from _Animancer.Play,
                // But we can also just access it using _Animancer.States.Current.

                bool isRunning = SampleInput.LeftShiftHold;
                _Animancer.States.Current.Speed = isRunning ? 2 : 1;
            }
            else
            {
                // When we're not moving, we still remember the direction we're facing
                // so we can continue using the correct idle animation for that direction.
                Play(_Idles);
            }
        }

        /************************************************************************************************************************/

        private void Play(DirectionalSet<AnimationClip> animations)
        {
            // Instead of only a single animation, we have a different one for each direction we can face.
            // So we get whichever is appropriate for that direction and play it.

            AnimationClip clip = animations.Get(_Facing);
            _Animancer.Play(clip);

            // Or we could do that in one line:
            // _Animancer.Play(animations.Get(_Facing));
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Sets the character's starting sprite in Edit Mode so you can see it while working in the scene.
        /// </summary>
        protected virtual void OnValidate()
        {
            if (_Idles != null)
                _Idles.Get(_Facing).EditModeSampleAnimation(_Animancer);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
