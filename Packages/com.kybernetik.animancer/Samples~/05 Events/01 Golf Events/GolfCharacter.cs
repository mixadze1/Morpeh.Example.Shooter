// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Events
{
    /// <summary>Manages a character with the ability to hit a golf ball.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/events/golf">
    /// Golf Events</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Events/GolfCharacter
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Golf Events - Golf Character")]
    [AnimancerHelpUrl(typeof(GolfCharacter))]
    public class GolfCharacter : MonoBehaviour
    {
        /************************************************************************************************************************/

        private static readonly StringReference HitEventName = "Hit";

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private ClipTransition _Ready;
        [SerializeField, EventNames] private ClipTransition _Swing;
        [SerializeField] private GolfBall _Ball;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _Swing.Events.SetCallback(HitEventName, _Ball.Hit);
            _Swing.Events.OnEnd = PlayReady;

            _Animancer.Play(_Ready);
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            if (_Ball.ReadyToHit && SampleInput.LeftMouseDown)
                _Animancer.Play(_Swing);
        }

        /************************************************************************************************************************/

        private void PlayReady()
        {
            _Animancer.Play(_Ready);
        }

        /************************************************************************************************************************/
    }
}
