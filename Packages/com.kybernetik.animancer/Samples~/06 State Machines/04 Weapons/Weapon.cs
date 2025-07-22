// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.StateMachines
{
    /// <summary>Holds various animations relating to the use of a weapon.</summary>
    /// 
    /// <remarks>
    /// In a real game, this class might have other details like damage, damage type, weapon category, etc.
    /// It could also inherit from a base Item class for things like weight, cost, and description.
    /// <para></para>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fsm/weapons">
    /// Weapons</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.StateMachines/Weapon
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Weapons - Weapon")]
    [AnimancerHelpUrl(typeof(Weapon))]
    public class Weapon : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private TransitionAsset[] _AttackAnimations;
        public TransitionAsset[] AttackAnimations => _AttackAnimations;

        /************************************************************************************************************************/

        [SerializeField]
        private TransitionAsset _EquipAnimation;
        public TransitionAsset EquipAnimation => _EquipAnimation;

        [SerializeField]
        private TransitionAsset _UnequipAnimation;
        public TransitionAsset UnequipAnimation => _UnequipAnimation;

        /************************************************************************************************************************/
    }
}
