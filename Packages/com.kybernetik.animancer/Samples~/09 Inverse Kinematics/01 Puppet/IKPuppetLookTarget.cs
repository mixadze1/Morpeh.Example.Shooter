// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

using UnityEngine;

namespace Animancer.Samples.InverseKinematics
{
    /// <summary>An object for a character to look at using Inverse Kinematics (IK).</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/ik/puppet">
    /// Puppet</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.InverseKinematics/IKPuppetLookTarget
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Inverse Kinematics - IK Puppet Look Target")]
    [AnimancerHelpUrl(typeof(IKPuppetLookTarget))]
    public class IKPuppetLookTarget : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField, Range(0, 1)] private float _Weight = 1;
        [SerializeField, Range(0, 1)] private float _BodyWeight = 0.3f;
        [SerializeField, Range(0, 1)] private float _HeadWeight = 0.6f;
        [SerializeField, Range(0, 1)] private float _EyesWeight = 1;
        [SerializeField, Range(0, 1)] private float _ClampWeight = 0.5f;

        /************************************************************************************************************************/

        public void UpdateAnimatorIK(Animator animator)
        {
            animator.SetLookAtWeight(_Weight, _BodyWeight, _HeadWeight, _EyesWeight, _ClampWeight);
            animator.SetLookAtPosition(transform.position);
        }

        /************************************************************************************************************************/
    }
}
