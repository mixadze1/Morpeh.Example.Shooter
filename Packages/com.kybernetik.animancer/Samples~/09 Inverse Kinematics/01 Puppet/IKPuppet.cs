// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.InverseKinematics
{
    /// <summary>Demonstrates how to use Unity's Inverse Kinematics (IK) system to move a character's limbs.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/ik/puppet">
    /// Puppet</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.InverseKinematics/IKPuppet
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Inverse Kinematics - IK Puppet")]
    [AnimancerHelpUrl(typeof(IKPuppet))]
    public class IKPuppet : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private Transform _BodyTarget;
        [SerializeField] private IKPuppetLookTarget _LookTarget;
        [SerializeField] private IKPuppetTarget[] _IKTargets;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            // Tell Unity that we want it to call OnAnimatorIK for states on this layer:
            _Animancer.Layers[0].ApplyAnimatorIK = true;
        }

        /************************************************************************************************************************/

        protected virtual void OnAnimatorIK(int layerIndex)
        {
            _Animancer.Animator.bodyPosition = _BodyTarget.position;
            _Animancer.Animator.bodyRotation = _BodyTarget.rotation;

            _LookTarget.UpdateAnimatorIK(_Animancer.Animator);

            for (int i = 0; i < _IKTargets.Length; i++)
            {
                _IKTargets[i].UpdateAnimatorIK(_Animancer.Animator);
            }
        }

        /************************************************************************************************************************/
    }
}
