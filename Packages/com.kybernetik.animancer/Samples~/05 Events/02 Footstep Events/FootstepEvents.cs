// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Events
{
    /// <summary>Uses Animancer Events to play a sound randomly selected from an array.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/events/footsteps">
    /// Footstep Events</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Events/FootstepEvents
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Footstep Events - Footstep Events")]
    [AnimancerHelpUrl(typeof(FootstepEvents))]
    public class FootstepEvents : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_AUDIO
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private StringAsset _EventName;
        [SerializeField, Range(0, 1)] private float _PitchRandomization = 0.2f;
        [SerializeField] private AudioClip[] _Sounds;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _Animancer.Events.AddTo<AudioSource>(_EventName, PlaySound);
        }

        /************************************************************************************************************************/

        private void PlaySound(AudioSource source)
        {
            source.clip = _Sounds[Random.Range(0, _Sounds.Length)];
            source.pitch = Random.Range(1 - _PitchRandomization, 1 + _PitchRandomization);
            source.volume = AnimancerEvent.Current.State.Weight;
            source.Play();

            // Create a sphere on the foot to show where the sound is coming from.
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Transform transform = sphere.transform;
            transform.parent = source.transform;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one * 0.2f;
            Destroy(sphere, 0.1f);// Destroy after 0.1 seconds.
        }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingAudioModuleError(this);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
