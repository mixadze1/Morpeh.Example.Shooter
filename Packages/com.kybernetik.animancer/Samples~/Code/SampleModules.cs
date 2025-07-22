// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;
using Object = UnityEngine.Object;

namespace Animancer.Samples
{
    /// <summary>Methods for logging errors about missing Unity modules.</summary>
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples/SampleModules
    public static class SampleModules
    {
        /************************************************************************************************************************/

        /// <summary>Returns an error message about a missing Unity module.</summary>
        [HideInCallstack]
        public static void LogMissingModuleError(string name, string url, Object context)
            => Debug.LogError(
                $"{context.GetType().Name} requires Unity's '{name}'" +
                $" module to be enabled in the Package Manager. {url}",
                context);

        /************************************************************************************************************************/

#if !UNITY_AUDIO
        /// <summary>An error message about the 'Audio' module being missing.</summary>
        [HideInCallstack]
        public static void LogMissingAudioModuleError(Object context)
            => LogMissingModuleError(
                "Audio",
                "https://docs.unity3d.com/ScriptReference/UnityEngine.AudioModule.html",
                context);
#endif

        /************************************************************************************************************************/

#if !UNITY_JSON_SERIALIZE
        /// <summary>An error message about the 'JSON Serialize' module being missing.</summary>
        [HideInCallstack]
        public static void LogMissingJsonSerializeModuleError(Object context)
            => LogMissingModuleError(
                "JSON Serialize",
                "https://docs.unity3d.com/ScriptReference/UnityEngine.JSONSerializeModule.html",
                context);
#endif

        /************************************************************************************************************************/

#if !UNITY_PHYSICS_2D
        /// <summary>An error message about the 'Physics 2D' module being missing.</summary>
        [HideInCallstack]
        public static void LogMissingPhysics2DModuleError(Object context)
            => LogMissingModuleError(
                "Physics 2D",
                "https://docs.unity3d.com/ScriptReference/UnityEngine.Physics2DModule.html",
                context);
#endif

        /************************************************************************************************************************/

#if !UNITY_PHYSICS_3D
        /// <summary>An error message about the 'Physics' module being missing.</summary>
        [HideInCallstack]
        public static void LogMissingPhysics3DModuleError(Object context)
            => LogMissingModuleError(
                "Physics",
                "https://docs.unity3d.com/ScriptReference/UnityEngine.PhysicsModule.html",
                context);
#endif

        /************************************************************************************************************************/

#if !UNITY_UGUI
        /// <summary>An error message about the 'Unity UI' module being missing.</summary>
        [HideInCallstack]
        public static void LogMissingUnityUIModuleError(Object context)
            => LogMissingModuleError(
                "Unity UGUI",
                "https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/index.html",
                context);
#endif

        /************************************************************************************************************************/
    }
}

