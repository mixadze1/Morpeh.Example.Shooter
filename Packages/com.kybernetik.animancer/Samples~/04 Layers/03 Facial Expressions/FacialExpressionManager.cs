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
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/layers/face">
    /// Facial Expressions</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Layers/FacialExpressionManager
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Layers - Facial Expression Manager")]
    [AnimancerHelpUrl(typeof(FacialExpressionManager))]
    [DefaultExecutionOrder(1000)]// Initialize after other scripts have initialized their layers.
    public class FacialExpressionManager : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private SampleButton _Button;
        [SerializeField] private NamedClipTransition[] _Expressions;

        private AnimancerLayer _Layer;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _Layer = _Animancer.Layers.Add();
            _Layer.SetDebugName("Facial Expressions");

            _Layer.Play(_Expressions[0]);

            for (int i = 0; i < _Expressions.Length; i++)
            {
                NamedClipTransition expression = _Expressions[i];

                _Button.AddButton(
                    i,
                    expression.Name,
                    () => _Layer.Play(expression));
            }
        }

        /************************************************************************************************************************/
    }
}
