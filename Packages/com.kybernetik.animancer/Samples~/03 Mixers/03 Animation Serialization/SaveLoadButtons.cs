// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System.IO;
using UnityEngine;

#if UNITY_UGUI
using UnityEngine.UI;
#endif

namespace Animancer.Samples.Mixers
{
    /// <summary>Uses buttons to save and load the current animation details.</summary>
    /// 
    /// <remarks>
    /// <strong>Sample:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/mixers/serialization">
    /// Animation Serialization</see>
    /// </remarks>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples.Mixers/SaveLoadButtons
    /// 
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Mixers - Save Load Buttons")]
    [AnimancerHelpUrl(typeof(SaveLoadButtons))]
    public class SaveLoadButtons : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_UGUI && UNITY_JSON_SERIALIZE
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private StringAsset _SpeedParameter;
        [SerializeField] private Button _SaveButton;
        [SerializeField] private Button _LoadButton;
        [SerializeField] private string _FilePath = "Temp/AnimancerSerializedPose.json";

        private readonly SerializablePose SerializablePose = new();

        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            _SaveButton.onClick.AddListener(Save);
            _LoadButton.onClick.AddListener(Load);
        }

        /************************************************************************************************************************/

        private void Save()
        {
            SerializablePose.GatherFrom(_Animancer, _SpeedParameter);

            string json = JsonUtility.ToJson(SerializablePose);

            File.WriteAllText(_FilePath, json);
        }

        /************************************************************************************************************************/

        private void Load()
        {
            string json = File.ReadAllText(_FilePath);

            JsonUtility.FromJsonOverwrite(json, SerializablePose);

            SerializablePose.ApplyTo(_Animancer, _SpeedParameter);
        }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
#if !UNITY_JSON_SERIALIZE
            SampleModules.LogMissingJsonSerializeModuleError(this);
#endif

#if !UNITY_UGUI
            SampleModules.LogMissingUnityUIModuleError(this);
#endif
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
