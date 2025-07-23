using UnityEngine;

namespace _Scripts.Core
{
    public static class CustomDebug
    {
        public static void Log(string message)
        {
#if UNITY_EDITOR || DEV_VERSION
            Debug.Log(message);
#endif
        }

        public static void Log(string message, Color color, bool isLogDevice = false)
        {
#if UNITY_EDITOR || DEV_VERSION
            if (!isLogDevice)
            {
                string colorHex = ColorUtility.ToHtmlStringRGB(color);
                Debug.Log($"<color=#{colorHex}> {message}</color>"); 
            }
#endif
            if (isLogDevice)
            {
                string colorHex = ColorUtility.ToHtmlStringRGB(color);
                Debug.Log($"<color=#{colorHex}> {message}</color>");
            }
        }
    }
}