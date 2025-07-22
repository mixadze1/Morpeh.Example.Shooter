// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Layers
{
    /// <summary>
    /// Demonstrates how to use layers to play multiple
    /// independent animations at the same time on different body parts.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/layers/dynamic">
    /// Dynamic Layers</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Layers/DynamicLayeredCharacterAnimations
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Layers - Dynamic Layered Character Animations")]
    [AnimancerHelpUrl(typeof(DynamicLayeredCharacterAnimations))]
    public class DynamicLayeredCharacterAnimations : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private LayeredAnimationManager _AnimationManager;
        [SerializeField] private ClipTransition _Idle;
        [SerializeField] private ClipTransition _Move;
        [SerializeField] private ClipTransition _Action;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _Action.Events.OnEnd = _AnimationManager.FadeOutAction;
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            UpdateMovement();
            UpdateAction();
        }

        /************************************************************************************************************************/

        private void UpdateMovement()
        {
            float forward = SampleInput.WASD.y;
            if (forward > 0)
            {
                _AnimationManager.PlayBase(_Move, false);
            }
            else
            {
                _AnimationManager.PlayBase(_Idle, true);
            }
        }

        /************************************************************************************************************************/

        private void UpdateAction()
        {
            if (SampleInput.LeftMouseUp)
            {
                _AnimationManager.PlayAction(_Action);
            }
        }

        /************************************************************************************************************************/
    }
}
