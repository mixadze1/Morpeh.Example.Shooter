// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

using UnityEngine;

namespace Animancer.Samples.InverseKinematics
{
    /// <summary>Allows the user to drag any object with a collider around on screen with the mouse.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/ik/puppet">
    /// Puppet</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.InverseKinematics/MouseDrag
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Inverse Kinematics - Mouse Drag")]
    [AnimancerHelpUrl(typeof(MouseDrag))]
    public class MouseDrag : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        private Transform _Dragging;
        private float _Distance;

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            // On click, do a raycast from the mouse, grab whatever it hits, and calculate how far away it is.
            if (SampleInput.LeftMouseDown)
            {
                Ray ray = Camera.main.ScreenPointToRay(SampleInput.MousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.rigidbody != null)
                {
                    _Dragging = hit.transform;

                    Transform cameraTransform = Camera.main.transform;
                    _Distance = Vector3.Dot(_Dragging.position - cameraTransform.position, cameraTransform.forward);
                }

                return;
            }
            // While holding the button, move the object in line with the mouse ray.
            else if (_Dragging != null && SampleInput.LeftMouseHold)
            {
                Ray ray = Camera.main.ScreenPointToRay(SampleInput.MousePosition);

                Transform cameraTransform = Camera.main.transform;
                Vector3 forward = cameraTransform.forward;

                float dot = Vector3.Dot(ray.direction, forward);
                if (dot > 0)
                {
                    Vector3 planeCenter = cameraTransform.position + forward * _Distance;
                    Vector3 intersection = ray.origin + ray.direction * Vector3.Dot(planeCenter - ray.origin, forward) / dot;
                    _Dragging.position = intersection;
                    return;
                }
            }

            _Dragging = null;
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
