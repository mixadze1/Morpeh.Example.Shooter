// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Samples
{
    /// <summary>Simple mouse controls for orbiting the camera around a focal point.</summary>
    /// <remarks>
    /// <strong>Documentation:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/basics/scene-setup#orbit-controls">
    /// Orbit Controls</see>
    /// </remarks>
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples/OrbitControls
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Orbit Controls")]
    [AnimancerHelpUrl(typeof(OrbitControls))]
    [ExecuteAlways]
    public class OrbitControls : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private Transform _LookAt;
        [SerializeField] private Vector3 _FocalPoint = new(0, 1, 0);
        [SerializeField] private Vector3 _Sensitivity = new(1, -0.75f, -0.1f);
        [SerializeField, Meters] private float _MinZoom = 0.5f;
        [SerializeField, Seconds] private float _ZoomSmoothTime = 0.2f;

        private float _Distance;
        private float _TargetDistance;
        private float _ZoomSpeed;

        /************************************************************************************************************************/

        private Vector3 TargetPosition
            => _LookAt != null
            ? _LookAt.TransformPoint(_FocalPoint)
            : _FocalPoint;

        /************************************************************************************************************************/

        protected virtual void OnEnable()
        {
            Vector3 targetPosition = TargetPosition;

            _Distance = _TargetDistance = Vector3.Distance(targetPosition, transform.position);

            transform.LookAt(targetPosition);
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                transform.LookAt(TargetPosition);
                return;
            }
#endif

            if (SampleInput.RightMouseHold)
            {
                Vector2 movement = SampleInput.MousePositionDelta;
                if (!movement.Equals(Vector2.zero))
                {
                    Vector3 euler = transform.localEulerAngles;
                    euler.y += movement.x * _Sensitivity.x;
                    euler.x += movement.y * _Sensitivity.y;
                    if (euler.x > 180)
                        euler.x -= 360;
                    euler.x = Mathf.Clamp(euler.x, -80, 80);
                    transform.localEulerAngles = euler;
                }
            }

            // Scroll to zoom if the mouse is currently inside the game window.
            float zoom = SampleInput.MouseScrollDelta.y * _Sensitivity.z;
            Vector2 mousePosition = SampleInput.MousePosition;
            if (zoom != 0 &&
                mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
                mousePosition.y >= 0 && mousePosition.y <= Screen.height)
            {
                if (zoom > 0)
                    _TargetDistance *= 1 + zoom;
                else
                    _TargetDistance /= 1 - zoom;

                if (_TargetDistance < _MinZoom)
                    _TargetDistance = _MinZoom;
            }

            // Always update position even with no input in case the target is moving.
            UpdatePosition();
        }

        /************************************************************************************************************************/

        private void UpdatePosition()
        {
            _Distance = Mathf.SmoothDamp(_Distance, _TargetDistance, ref _ZoomSpeed, _ZoomSmoothTime);
            transform.position = TargetPosition - transform.forward * _Distance;
        }

        /************************************************************************************************************************/

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = new(0.5f, 1, 0.5f, 1);
            Gizmos.DrawLine(transform.position, TargetPosition);
        }

        /************************************************************************************************************************/
    }
}
