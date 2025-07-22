// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

using UnityEngine;

namespace Animancer.Samples.FineControl
{
    /// <summary>
    /// Attempts to interact with whatever <see cref="IInteractable"/>
    /// the cursor is pointing at when the user clicks the mouse.
    /// </summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/fine-control/doors">
    /// Doors</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.FineControl/ClickToInteract
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Fine Control - Click To Interact")]
    [AnimancerHelpUrl(typeof(ClickToInteract))]
    public class ClickToInteract : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_PHYSICS_3D
        /************************************************************************************************************************/

        protected virtual void Update()
        {
            if (!SampleInput.LeftMouseUp)
                return;

            Ray ray = Camera.main.ScreenPointToRay(SampleInput.MousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                IInteractable interactable = raycastHit.collider.GetComponentInParent<IInteractable>();
                interactable?.Interact();
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
