// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#if UNITY_EDITOR

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEditor;
using UnityEngine;

namespace Animancer.Samples.AnimatorControllers.GameKit
{
    /// <summary>Warns the user if the 3D Game Kit is missing.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/animator-controllers/3d-game-kit">
    /// 3D Game Kit</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.AnimatorControllers.GameKit/DependencyWarning3DGameKit
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Game Kit - Dependency Warning")]
    [AnimancerHelpUrl(typeof(DependencyWarning3DGameKit))]
    public class DependencyWarning3DGameKit : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private GameObject _Reference;

        /************************************************************************************************************************/

        protected virtual void OnValidate()
        {
            if (_Reference == null)
                return;

            if (PrefabUtility.GetPrefabInstanceStatus(_Reference) != PrefabInstanceStatus.MissingAsset)
                return;

            if (EditorUtility.DisplayDialog(
                "3D Game Kit Lite Required",
                "The 3D Game Kit Lite is required for this sample",
                "Open Asset Store",
                "OK"))
                Application.OpenURL("https://assetstore.unity.com/packages/templates/tutorials/3d-game-kit-lite-135162");
        }

        /************************************************************************************************************************/
    }
}

#endif
