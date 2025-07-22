// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.TransitionLibraries;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer.Samples.Mixers
{
    /// <summary>
    /// Gathers the animation details from a character to save
    /// and applies them back after loading.
    /// </summary>
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/mixers/serialization">
    /// Animation Serialization</see>
    /// </remarks>
    [Serializable]
    public class SerializablePose
    {
        /************************************************************************************************************************/

        [SerializeField] private float _RemainingFadeDuration;
        [SerializeField] private float _SpeedParameter;
        [SerializeField] private List<StateData> _States = new();

        /************************************************************************************************************************/

        [Serializable]
        public struct StateData
        {
            /************************************************************************************************************************/

            /// <summary>The index of the state in the <see cref="TransitionLibrary"/>.</summary>
            /// <remarks>
            /// This is a <c>byte</c> because a library probably won't have more than 256 transitions.
            /// If it does, you would need a <c>ushort</c> instead.
            /// </remarks>
            public byte index;

            /// <summary><see cref="AnimancerState.Time"/></summary>
            public float time;

            /// <summary><see cref="AnimancerNode.Weight"/></summary>
            public float weight;

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/

        public void GatherFrom(AnimancerComponent animancer, StringReference speedParameter)
        {
            _States.Clear();
            _RemainingFadeDuration = 0;

            IReadOnlyIndexedList<AnimancerState> activeStates = animancer.Layers[0].ActiveStates;
            for (int i = 0; i < activeStates.Count; i++)
            {
                AnimancerState state = activeStates[i];

                _States.Add(new StateData()
                {
                    index = (byte)animancer.Graph.Transitions.IndexOf(state.Key),
                    time = state.Time,
                    weight = state.Weight,
                });

                if (state.FadeGroup != null &&
                    state.TargetWeight == 1)
                {
                    _RemainingFadeDuration = state.FadeGroup.RemainingFadeDuration;

                    // If this state is fading in, swap it with the first state
                    // so we know which one it is after deserialization.
                    if (i > 0)
                        (_States[0], _States[i]) = (_States[i], _States[0]);
                }
            }

            _SpeedParameter = animancer.Parameters.GetFloat(speedParameter);
        }

        /************************************************************************************************************************/

        public void ApplyTo(AnimancerComponent animancer, StringReference speedParameter)
        {
            float weightlessThreshold = AnimancerLayer.WeightlessThreshold;
            try
            {
                AnimancerLayer.WeightlessThreshold = 0;

                AnimancerLayer layer = animancer.Layers[0];
                layer.Stop();
                layer.Weight = 1;

                AnimancerState firstState = null;

                for (int i = _States.Count - 1; i >= 0; i--)
                {
                    StateData stateData = _States[i];
                    if (!animancer.Graph.Transitions.TryGetTransition(
                        stateData.index,
                        out TransitionModifierGroup transition))
                    {
                        Debug.LogError(
                            $"Transition Library '{animancer.Transitions}'" +
                            $" doesn't contain transition index {stateData.index}.",
                            animancer);
                        continue;
                    }

                    AnimancerState state = layer.GetOrCreateState(transition.Transition);

                    if (state.Weight != 0)
                        state = layer.GetOrCreateWeightlessState(state);

                    state.IsPlaying = true;
                    state.Time = stateData.time;
                    state.SetWeight(stateData.weight);

                    if (i == 0)
                        firstState = state;
                }

                layer.Play(firstState, _RemainingFadeDuration);

                animancer.Parameters.SetValue(speedParameter, _SpeedParameter);
            }
            finally
            {
                AnimancerLayer.WeightlessThreshold = weightlessThreshold;
            }
        }

        /************************************************************************************************************************/
    }
}
