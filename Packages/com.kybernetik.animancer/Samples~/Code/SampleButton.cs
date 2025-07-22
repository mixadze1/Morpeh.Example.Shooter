// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

using UnityEngine;
using UnityEngine.Events;

#if UNITY_UGUI
using UnityEngine.UI;
#endif

namespace Animancer.Samples
{
    /// <summary>A simple system for dynamically creating a list of buttons based on a template.</summary>
    /// <remarks>
    /// <strong>Documentation:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/samples/basics/scene-setup#buttons">
    /// Basic Scene Setup - Buttons</see>
    /// </remarks>
    /// https://kybernetik.com.au/animancer/api/Animancer.Samples/SampleButton
    [AddComponentMenu(Strings.SamplesMenuPrefix + "Sample Button")]
    [AnimancerHelpUrl(typeof(SampleButton))]
    public class SampleButton : MonoBehaviour
    {
        /************************************************************************************************************************/
#if UNITY_UGUI
        /************************************************************************************************************************/

        [SerializeField] private RectTransform _Transform;
        [SerializeField] private Button _Button;
        [SerializeField] private Text _Text;
        [SerializeField] private float _Spacing = 10;

        /************************************************************************************************************************/

        /// <summary>Automatically gathers the serialized fields.</summary>
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Transform);
            gameObject.GetComponentInParentOrChildren(ref _Button);
            gameObject.GetComponentInParentOrChildren(ref _Text);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Initializes this button when called with `index` 0.
        /// Otherwise, creates a copy of this button and initializes it instead.
        /// </summary>
        public SampleButton AddButton(
            int index,
            string text,
            UnityAction onClick)
        {
            SampleButton button = this;

            if (index != 0)
            {
                button = Instantiate(button, button._Transform.parent);

                Vector2 position = button._Transform.anchoredPosition;
                position.y -= index * (button._Transform.sizeDelta.y + button._Spacing);
                button._Transform.anchoredPosition = position;
            }

            button._Button.onClick.AddListener(onClick);
            button._Text.text = text;
            button.name = text;

            return button;
        }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        protected virtual void Awake()
        {
            SampleModules.LogMissingUnityUIModuleError(this);
        }

        /************************************************************************************************************************/

        public SampleButton AddButton(
            int index,
            string text,
            UnityAction onClick)
            => this;

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
