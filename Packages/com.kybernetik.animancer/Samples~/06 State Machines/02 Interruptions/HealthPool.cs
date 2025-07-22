// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Samples.FineControl;
using System;
using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>Manages a character's health and damage received.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/interruptions">
    /// Interruptions</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/HealthPool
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Interruptions - Health Pool")]
    [AnimancerHelpUrl(typeof(HealthPool))]
    public class HealthPool : MonoBehaviour, IInteractable
    {
        /************************************************************************************************************************/

        // Normally, this class would have fields like maximum health and
        // current health to keep track of how much damage the character takes,
        // but for this sample we're just pretending the character was hit
        // whenever something interacts with it.

        public event Action OnHitReceived;

        /************************************************************************************************************************/

        public void Interact()
        {
            OnHitReceived?.Invoke();
        }

        /************************************************************************************************************************/
    }
}
