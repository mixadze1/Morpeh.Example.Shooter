// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.FineControl
{
    /// <summary>
    /// Demonstrates how to save some performance by updating Animancer at a lower frequency
    /// when the character is far away from the camera.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fine-control/update-rate">
    /// Update Rate</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.FineControl/DynamicUpdateRate
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Fine Control - Dynamic Update Rate")]
    [AnimancerHelpUrl(typeof(DynamicUpdateRate))]
    public class DynamicUpdateRate : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private LowUpdateRate _LowUpdateRate;
        [SerializeField] private TextMesh _TextMesh;
        [SerializeField, Meters] private float _SlowUpdateDistance = 5;

        private Transform _Camera;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            // Finding the Camera.main is a slow operation so we don't want to repeat it every update.
            _Camera = Camera.main.transform;
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            // Compare the squared distance to the camera with the squared threshold.
            // This is more efficient than calculating the distance because it avoids the square root calculation.
            Vector3 offset = _Camera.position - transform.position;
            float squaredDistance = offset.sqrMagnitude;

            // Low update rate enabled = true if the distance is further away.
            // Low update rate enabled = false if the distance is closer.
            float squaredSlowUpdateDistance = _SlowUpdateDistance * _SlowUpdateDistance;
            _LowUpdateRate.enabled = squaredDistance > squaredSlowUpdateDistance;

            // For the sake of this sample, use a TextMesh to show the current details.
            float distance = Mathf.Sqrt(squaredDistance);
            string updating = _LowUpdateRate.enabled ? "Slowly" : "Normally";
            _TextMesh.text = $"Distance {distance:0.00}\nUpdating {updating}\n\nDynamic Rate";
        }

        /************************************************************************************************************************/
    }
}
