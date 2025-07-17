using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Core.Configs
{
    [CreateAssetMenu(fileName = "Configs/" + nameof(HealthConfig))]
    public class HealthConfig : ScriptableObject
    {
        [FormerlySerializedAs("Health")] [SerializeField] public int StartHealth;
    }
}