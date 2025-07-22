// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //
// Compare to the original script: https://github.com/Unity-Technologies/animation-jobs-samples/blob/master/Assets/animation-jobs-samples/Samples/Scripts/TwoBoneIK/TwoBoneIK.cs

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Jobs
{
    /// <summary>
    /// An sample of how to use Animation Jobs in Animancer to apply simple two bone Inverse Kinematics,
    /// even to Generic Rigs which are not supported by Unity's inbuilt IK system.
    /// </summary>
    /// 
    /// <remarks>
    /// This sample is based on Unity's
    /// <see href="https://github.com/Unity-Technologies/animation-jobs-samples">Animation Jobs Samples</see>.
    /// <para></para>
    /// This script sets up the job in place of
    /// <see href="https://github.com/Unity-Technologies/animation-jobs-samples/blob/master/Assets/animation-jobs-samples/Samples/Scripts/TwoBoneIK/TwoBoneIK.cs">
    /// TwoBoneIK.cs</see>.
    /// <para></para>
    /// The <see cref="TwoBoneIKJob"/> script is almost identical to the original 
    /// <see href="https://github.com/Unity-Technologies/animation-jobs-samples/blob/master/Assets/animation-jobs-samples/Runtime/AnimationJobs/TwoBoneIKJob.cs">
    /// TwoBoneIKJob.cs</see>.
    /// <para></para>
    /// The <see href="https://learn.unity.com/tutorial/working-with-animation-rigging">Animation Rigging</see> package
    /// has an IK system which is much better than this sample.
    /// <para></para>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/jobs/two-bone-ik">
    /// Two Bone IK</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Jobs/TwoBoneIK
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Jobs - Two Bone IK")]
    [AnimancerHelpUrl(typeof(TwoBoneIK))]
    public class TwoBoneIK : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private Transform _EndBone;
        [SerializeField] private Transform _Target;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            // Get the bones we want to affect.
            Transform midBone = _EndBone.parent;
            Transform topBone = midBone.parent;

            // Create the job and setup its details.
            TwoBoneIKJob twoBoneIKJob = new();
            twoBoneIKJob.Setup(_Animancer.Animator, topBone, midBone, _EndBone, _Target);

            // Add it to Animancer's output.
            _Animancer.Graph.InsertOutputJob(twoBoneIKJob);
        }

        /************************************************************************************************************************/
    }
}
