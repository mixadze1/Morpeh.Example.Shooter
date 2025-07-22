// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Samples.Jobs
{
    /// <summary>
    /// An sample component that calls <see cref="HitReceiver.Hit"/>
    /// when the user clicks on the ground.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/jobs/hit-impacts">
    /// Hit Impacts</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Jobs/ClickToHit
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Jobs - Click to Hit")]
    [AnimancerHelpUrl(typeof(ClickToHit))]
    public class ClickToHit : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        [SerializeField] private HitReceiver _HitReceiver;
        [SerializeField] private float _LeftClickForce = 500;
        [SerializeField] private float _RightClickForce = 300;

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            if (SampleInput.LeftMouseDown)
                HitTarget(_LeftClickForce);
            else if (SampleInput.RightMouseDown)
                HitTarget(_RightClickForce);
        }

        /************************************************************************************************************************/

        private void HitTarget(float force)
        {
            // Get a ray from the main camera in the direction of the mouse cursor.
            Ray ray = Camera.main.ScreenPointToRay(SampleInput.MousePosition);

            // Raycast with it and stop trying to move it it doesn't hit anything.
            if (!Physics.Raycast(ray, out RaycastHit raycastHit))// Note the exclamation mark !
                return;

            Vector3 direction = _HitReceiver.transform.position - raycastHit.point;
            _HitReceiver.Hit(direction, force);
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
