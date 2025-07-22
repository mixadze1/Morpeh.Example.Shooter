// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0414 // Field is assigned but its value is never used.
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.InverseKinematics
{
    /// <summary>
    /// Demonstrates how to use Unity's Inverse Kinematics (IK) system to
    /// adjust a character's feet according to the terrain they are moving over.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/ik/uneven-ground">
    /// Uneven Ground</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.InverseKinematics/RaycastFootIK
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Inverse Kinematics - Raycast Foot IK")]
    [AnimancerHelpUrl(typeof(RaycastFootIK))]
    public class RaycastFootIK : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField, Meters] private float _RaycastOriginY = 0.5f;
        [SerializeField, Meters] private float _RaycastEndY = -0.2f;

        /************************************************************************************************************************/

        private Transform _LeftFoot;
        private Transform _RightFoot;

        private AnimatedFloat _FootWeights;

        /************************************************************************************************************************/

        /// <summary>Public property for a UI Toggle to enable or disable the IK.</summary>
        public bool ApplyAnimatorIK
        {
            get => _Animancer.Layers[0].ApplyAnimatorIK;
            set => _Animancer.Layers[0].ApplyAnimatorIK = value;
        }

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _LeftFoot = _Animancer.Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            _RightFoot = _Animancer.Animator.GetBoneTransform(HumanBodyBones.RightFoot);

            _FootWeights = new(_Animancer, "LeftFootIK", "RightFootIK");

            // Tell Unity that OnAnimatorIK needs to be called every frame.
            ApplyAnimatorIK = true;
        }

        /************************************************************************************************************************/

        // Note that due to limitations in the Playables API,
        // Unity will always call this method with layerIndex = 0.
        protected virtual void OnAnimatorIK(int layerIndex)
        {
            // _FootWeights[0] is the first property we specified in Awake: "LeftFootIK".
            // _FootWeights[1] is the second property we specified in Awake: "RightFootIK".
            UpdateFootIK(
                _LeftFoot,
                AvatarIKGoal.LeftFoot,
                _FootWeights[0],
                _Animancer.Animator.leftFeetBottomHeight);
            UpdateFootIK(
                _RightFoot,
                AvatarIKGoal.RightFoot,
                _FootWeights[1],
                _Animancer.Animator.rightFeetBottomHeight);
        }

        /************************************************************************************************************************/

        private void UpdateFootIK(
            Transform footTransform,
            AvatarIKGoal goal,
            float weight,
            float footBottomHeight)
        {
            Animator animator = _Animancer.Animator;
            animator.SetIKPositionWeight(goal, weight);
            animator.SetIKRotationWeight(goal, weight);

            if (weight == 0)
                return;

            // Get the local up direction of the foot.
            Quaternion rotation = animator.GetIKRotation(goal);
            Vector3 localUp = rotation * Vector3.up;

            Vector3 position = footTransform.position;
            position += localUp * _RaycastOriginY;

            float distance = _RaycastOriginY - _RaycastEndY;

            if (Physics.Raycast(position, -localUp, out RaycastHit hit, distance))
            {
                // Use the hit point as the desired position.
                position = hit.point;
                position += localUp * footBottomHeight;
                animator.SetIKPosition(goal, position);

                // Use the hit normal to calculate the desired rotation.
                Vector3 rotationAxis = Vector3.Cross(localUp, hit.normal);
                float angle = Vector3.Angle(localUp, hit.normal);
                rotation = Quaternion.AngleAxis(angle, rotationAxis) * rotation;

                animator.SetIKRotation(goal, rotation);
            }
            else// Otherwise simply stretch the leg out to the end of the ray.
            {
                position += localUp * (footBottomHeight - distance);
                animator.SetIKPosition(goal, position);
            }
        }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingPhysics3DModuleError(this);
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
