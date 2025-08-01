using UnityEngine;

namespace _Scripts.Core.Configs
{
    [CreateAssetMenu]
    public class HealthConfig : ScriptableObject
    {
        [SerializeField] private float _durationShow = 0.5f;
        [SerializeField] private float _durationHide = 0.25f;

        public float DurationShow => _durationShow;
        public float DurationHide => _durationHide;
    }
}