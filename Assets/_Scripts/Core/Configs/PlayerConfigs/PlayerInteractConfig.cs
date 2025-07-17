using System;
using UnityEngine;

namespace _Scripts.Core.Configs
{
    [CreateAssetMenu(fileName = "Configs/" + nameof(PlayerInteractConfig))]
    public class PlayerInteractConfig : ScriptableObject
    {
        [SerializeField] private float _interactDistance = 10;
        [SerializeField] private LayerMask _interactLayerMask;

        public float InteractDistance => _interactDistance;
        public LayerMask InteractLayerMask => _interactLayerMask;
    }
}