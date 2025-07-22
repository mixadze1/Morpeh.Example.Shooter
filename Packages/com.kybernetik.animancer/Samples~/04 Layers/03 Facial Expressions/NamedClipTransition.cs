// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

using System;
using UnityEngine;

namespace Animancer.Samples.Layers
{
    /// <summary>A <see cref="ClipTransition"/> with a <see cref="Name"/></summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/layers/face">
    /// Facial Expressions</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Layers/NamedClipTransition
    /// 
    [AnimancerHelpUrl(typeof(NamedClipTransition))]
    [Serializable]
    public class NamedClipTransition : ClipTransition
    {
        /************************************************************************************************************************/

        [SerializeField]
        private string _Name;

        /// <inheritdoc/>
        public override string Name
            => _Name;

        /// <summary>Sets the <see cref="Name"/>.</summary>
        public void SetName(string name)
            => _Name = name;

        /************************************************************************************************************************/
    }
}
