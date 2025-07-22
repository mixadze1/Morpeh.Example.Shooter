// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace Animancer.Samples.Jobs
{
    /// <summary>
    /// An sample component that demonstrates how <see cref="SimpleLean"/>
    /// can be used as a dynamic response to getting hit.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/jobs/hit-impacts">
    /// Hit Impacts</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Jobs/HitReceiver
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Jobs - Hit Receiver")]
    [AnimancerHelpUrl(typeof(HitReceiver))]
    public class HitReceiver : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private Transform[] _Bones;
        [SerializeField, Degrees] private float _MaximumAngle = 45;
        [SerializeField, Seconds] private float _SmoothingTime = 0.25f;

        private SimpleLean _Lean;
        private float _Speed;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            Debug.Assert(_Bones.Length > 0, "No bones are assigned.", this);
            NativeArray<TransformStreamHandle> boneHandles =
                AnimancerUtilities.ConvertToTransformStreamHandles(_Bones, _Animancer.Animator);
            _Lean = new(_Animancer.Graph, Vector3.right, boneHandles);
        }

        /************************************************************************************************************************/

        public void Hit(Vector3 direction, float force)
        {
            _Lean.Axis = Vector3.Cross(Vector3.up, direction).normalized;

            _Speed = force;

            enabled = true;
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            float angle = Mathf.SmoothDamp(_Lean.Angle, 0, ref _Speed, _SmoothingTime);
            angle = Math.Min(angle, _MaximumAngle);
            _Lean.Angle = angle;

            if (angle == 0 && _Speed == 0)
                enabled = false;
        }

        /************************************************************************************************************************/
    }
}
