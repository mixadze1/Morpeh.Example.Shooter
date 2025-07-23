using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Core.Configs
{
    [CreateAssetMenu(fileName = "Configs/" + nameof(HealthConfig))]
    public class HealthConfig : ScriptableObject
    {
        [SerializeField] private int _health = 100;
        
        public int StartHealth => _health;
    }
}