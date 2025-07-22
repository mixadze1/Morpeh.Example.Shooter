using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Core.Providers.View
{
    public class AmmoViewProvider : MonoProvider<AmmoViewComponent>
    {
        
    }

    [Serializable]
    public struct AmmoViewComponent : IComponent
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private TextMeshProUGUI _magazines;
        [SerializeField] private Slider _progressBar;

        public void UpdateView(int weaponComponentAmountAmmo, int weaponComponentMaxAmmoInMagazine, int weaponComponentAmountMagazines)
        {
            _amount.text = $"{weaponComponentAmountAmmo}/{weaponComponentMaxAmmoInMagazine}";
            _magazines.text = $"{weaponComponentAmountMagazines}";
            _progressBar.value = (float)weaponComponentAmountAmmo / weaponComponentMaxAmmoInMagazine;
        }

        public void Hide()
        {
            if (_container.gameObject.activeSelf)
                _container.gameObject.SetActive(false);
        }

        public void Show()
        {
            if (!_container.gameObject.activeSelf)
                _container.gameObject.SetActive(true);
        }
    }
}