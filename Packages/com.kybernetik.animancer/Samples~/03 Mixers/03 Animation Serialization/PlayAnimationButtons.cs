// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.TransitionLibraries;
using UnityEngine;

namespace Animancer.Samples.Mixers
{
    /// <summary>Creates buttons for playing each transition in a library.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/mixers/serialization">
    /// Animation Serialization</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Mixers/PlayAnimationButtons
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Mixers - Play Animation Buttons")]
    [AnimancerHelpUrl(typeof(PlayAnimationButtons))]
    public class PlayAnimationButtons : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private SampleButton _Button;
        [SerializeField] private AnimancerComponent _Animancer;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            TransitionLibraryDefinition library = _Animancer.Transitions.Definition;

            TransitionAssetBase idle = library.Transitions[0];

            _Animancer.Play(idle);

            for (int i = 0; i < library.Transitions.Length; i++)
            {
                TransitionAssetBase transition = library.Transitions[i];

                // If the animation isn't looping, give it an End Event to play the default animation.
                // We need to initialize all the events on startup so that they're ready if one gets
                // played from a loaded file since the loading script doesn't know about the events.

                AnimancerState state = _Animancer.States.GetOrCreate(transition);
                if (!state.IsLooping)
                    state.Events(this).OnEnd ??= () => _Animancer.Play(idle);

                // Create a button to play it.
                // Don't use Play(state) because that won't include the Fade Duration from the transition.

                _Button.AddButton(
                    i,
                    transition.name,
                    () => _Animancer.Play(transition));
            }
        }

        /************************************************************************************************************************/
    }
}
