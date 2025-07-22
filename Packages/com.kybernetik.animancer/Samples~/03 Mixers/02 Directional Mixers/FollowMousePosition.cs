// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples.Mixers
{
    /// <summary>
    /// Controls Animancer parameters to make the character move
    /// towards the mouse position using Root Motion in a 2D Mixer.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/mixers/directional">
    /// Directional Mixers</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Mixers/FollowMousePosition
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Mixers - Follow Mouse Position")]
    [AnimancerHelpUrl(typeof(FollowMousePosition))]
    public class FollowMousePosition : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        // We could hard code the parameter names like this:
        // public static readonly StringReference ParameterX = "Movement X";
        // public static readonly StringReference ParameterY = "Movement Y";
        // But that would make this script less flexible
        // and you wouldn't be able to see what it's using in the Inspector.

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private StringAsset _ParameterX;
        [SerializeField] private StringAsset _ParameterY;
        [SerializeField, Seconds] private float _ParameterSmoothTime = 0.15f;
        [SerializeField, Meters] private float _StopProximity = 0.1f;

        /************************************************************************************************************************/

        private SmoothedVector2Parameter _SmoothedParameters;

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _SmoothedParameters = new SmoothedVector2Parameter(
                _Animancer,
                _ParameterX,
                _ParameterY,
                _ParameterSmoothTime);
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            // Calculate the movement direction.
            Vector3 movementDirection = GetMovementDirection();

            // The movement direction is in world space,
            // so we need to convert it to the character's local space
            // to be appropriate for their current rotation.
            Vector3 localDirection = transform.InverseTransformDirection(movementDirection);

            // Then set the target value for the parameters to move towards:
            // - Parameter X towards Direction X (right/left).
            // - Parameter Y towards Direction Z (forwards/backwards).
            // - Ignore Direction Y because the Mixer is only 2D.
            _SmoothedParameters.TargetValue = new Vector2(localDirection.x, localDirection.z);
        }

        /************************************************************************************************************************/

        private Vector3 GetMovementDirection()
        {
            // Get a ray from the main camera in the direction of the mouse cursor.
            Ray ray = Camera.main.ScreenPointToRay(SampleInput.MousePosition);

            // Raycast with it and stop trying to move it it doesn't hit anything.
            if (!Physics.Raycast(ray, out RaycastHit raycastHit))// Note the exclamation mark !
                return Vector3.zero;

            // If the ray hit something, calculate the direction from this object to that point.
            Vector3 direction = raycastHit.point - transform.position;

            // If we are close to the destination, stop moving.
            float squaredDistance = direction.sqrMagnitude;
            if (squaredDistance <= _StopProximity * _StopProximity)
            {
                return Vector3.zero;
            }
            else
            {
                // Otherwise normalize the direction so that we don't change speed based on distance.
                // Calling direction.Normalize() would do the same thing, but would calculate the magnitude again.
                return direction / Mathf.Sqrt(squaredDistance);
            }
        }

        /************************************************************************************************************************/

        protected virtual void OnDestroy()
        {
            // It's not necessary for this sample,
            // but if this component could be destroyed before the rest of the character
            // then we need to Dispose the smoother to remove it from the target parameters.
            _SmoothedParameters.Dispose();
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
