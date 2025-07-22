// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace Animancer.Samples.Jobs
{
    /// <summary>An sample of how to use Animation Jobs in Animancer to apply physics based damping to certain bones.</summary>
    /// 
    /// <remarks>
    /// This sample is based on Unity's
    /// <see href="https://github.com/Unity-Technologies/animation-jobs-samples">Animation Jobs Samples</see>.
    /// <para></para>
    /// This script sets up the job in place of
    /// <see href="https://github.com/Unity-Technologies/animation-jobs-samples/blob/master/Assets/animation-jobs-samples/Samples/Scripts/Damping/Damping.cs">
    /// Damping.cs</see>.
    /// <para></para>
    /// The <see cref="DampingJob"/> script is almost identical to the original 
    /// <see href="https://github.com/Unity-Technologies/animation-jobs-samples/blob/master/Assets/animation-jobs-samples/Runtime/AnimationJobs/DampingJob.cs">
    /// DampingJob.cs</see>.
    /// <para></para>
    /// The <see href="https://learn.unity.com/tutorial/working-with-animation-rigging">Animation Rigging</see> package
    /// has a damping system which is likely better than this sample.
    /// <para></para>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/jobs/damping">
    /// Damping</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Jobs/Damping
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Jobs - Damping")]
    [AnimancerHelpUrl(typeof(Damping))]
    public class Damping : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private Transform _EndBone;
        [SerializeField] private int _BoneCount = 3;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            // Create the job and initialize all its arrays.
            // They are all Persistent because we want them to last for the full lifetime of the job.
            // Most of them can use UninitializedMemory which is faster because we will be immediately filling them.
            // But the velocities will use the default ClearMemory to make sure all the values start at zero.

            // Since we are about to use these values several times, we can shorten the following lines a bit by using constants:
            const Allocator Persistent = Allocator.Persistent;
            const NativeArrayOptions UninitializedMemory = NativeArrayOptions.UninitializedMemory;

            DampingJob job = new()
            {
                jointHandles = new(_BoneCount, Persistent, UninitializedMemory),
                localPositions = new(_BoneCount, Persistent, UninitializedMemory),
                localRotations = new(_BoneCount, Persistent, UninitializedMemory),
                positions = new(_BoneCount, Persistent, UninitializedMemory),
                velocities = new(_BoneCount, Persistent),
            };

            // Initialize the contents of the arrays for each bone.
            Animator animator = _Animancer.Animator;
            Transform bone = _EndBone;
            for (int i = _BoneCount - 1; i >= 0; i--)
            {
                job.jointHandles[i] = animator.BindStreamTransform(bone);
                job.localPositions[i] = bone.localPosition;
                job.localRotations[i] = bone.localRotation;
                job.positions[i] = bone.position;

                bone = bone.parent;
            }

            job.rootHandle = animator.BindStreamTransform(bone);

            // Add the job to Animancer's output.
            _Animancer.Graph.InsertOutputJob(job);

            // Make sure Animancer disposes the Native Arrays when it is destroyed so we don't leak memory.
            // If we were writing our own job rather than just using the sample, we could have it implement the
            // IDisposable interface to dispose its arrays so that we would only have to call ...Add(_Job); here.
            _Animancer.Graph.Disposables.Add(job.jointHandles);
            _Animancer.Graph.Disposables.Add(job.localPositions);
            _Animancer.Graph.Disposables.Add(job.localRotations);
            _Animancer.Graph.Disposables.Add(job.positions);
            _Animancer.Graph.Disposables.Add(job.velocities);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Ensures that the <see cref="_BoneCount"/> is positive and not larger than the number of bones between the
        /// <see cref="_EndBone"/> and the <see cref="Animator"/>.
        /// </summary>
        /// <remarks>Called in Edit Mode whenever this script is loaded or a value is changed in the Inspector.</remarks>
        protected virtual void OnValidate()
        {
            if (_BoneCount < 1)
            {
                _BoneCount = 1;
            }
            else if (_EndBone != null && _Animancer != null && _Animancer.Animator != null)
            {
                Transform root = _Animancer.Animator.transform;

                Transform bone = _EndBone;
                for (int i = 0; i < _BoneCount; i++)
                {
                    bone = bone.parent;
                    if (bone == root)
                    {
                        _BoneCount = i + 1;
                        break;
                    }
                    else if (bone == null)
                    {
                        _EndBone = null;
                        Debug.LogWarning("The End Bone must be a child of the Animator.");
                        break;
                    }
                }
            }
        }

        /************************************************************************************************************************/
    }
}
