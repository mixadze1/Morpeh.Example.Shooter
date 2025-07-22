// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Basics
{
    /// <summary>
    /// Plays a movement animation while the user holds W or Up Arrow.
    /// Otherwise plays an idle animation.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/basics/movement">
    /// Basic Movement</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Basics/BasicMovementAnimations
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Basics - Basic Movement Animations")]
    [AnimancerHelpUrl(typeof(BasicMovementAnimations))]
    public class BasicMovementAnimations : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private AnimationClip _Idle;
        [SerializeField] private AnimationClip _Move;

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            float forward = SampleInput.WASD.y;
            if (forward > 0)
            {
                _Animancer.Play(_Move);
            }
            else
            {
                _Animancer.Play(_Idle);
            }
        }

        /************************************************************************************************************************/
    }
}
