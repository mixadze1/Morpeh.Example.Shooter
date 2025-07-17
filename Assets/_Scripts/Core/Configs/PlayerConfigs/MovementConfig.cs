using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Core.Configs
{
    [CreateAssetMenu(fileName = "Configs/" + nameof(MovementConfig))]
    public class MovementConfig : ScriptableObject
    {
        [SerializeField] private float _speedValue;
        [SerializeField, Range(-100, 0)] private float _gravity = -9.8f;
        [SerializeField] private float _jumpHeight = 5;

        [SerializeField] private float _groundDefaultGravity = -2f;
        public float SpeedValue => _speedValue;
        public float Gravity => _gravity;
        public float JumpHeight => _jumpHeight;
        public float OnGroundDefaultGravity => _groundDefaultGravity;
    }
}