using System;
using _Scripts.Core.Systems.PlayerBaseSystems;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Core.Providers.PlayerProviders
{
    public class ShowInteractViewProvider : MonoProvider<ShowInteractViewComponent>
    {
        private void Reset()
        {
            ref ShowInteractViewComponent unityTransform = ref GetData();
            unityTransform.SetText(GetComponent<TextMeshProUGUI>());
        }
    }

    [Serializable]
    public struct ShowInteractViewComponent : IComponent
    {
        [SerializeField] private TextMeshProUGUI _interactText;

        public void SetText(TextMeshProUGUI view)
        {
            _interactText = view;
        }

        public void Show(string message)
        {
            _interactText.text = message;
            _interactText.enabled = true;
        }

        public void Hide()
        {
            _interactText.enabled = false;
        }
    }
}