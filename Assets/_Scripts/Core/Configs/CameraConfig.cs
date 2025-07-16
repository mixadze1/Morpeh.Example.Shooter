using UnityEngine;

namespace _Scripts.Core.Configs
{
    [CreateAssetMenu]
    public class CameraConfig : ScriptableObject
    {
        [SerializeField] private float _mouseSens = 100;
        [SerializeField] private float _minX = -80;
        [SerializeField] private float _maxX = 80;
        
        public float MouseSensitivity => _mouseSens;
        public float MaxX => _maxX;
        public float MinX => _minX;
    }
}