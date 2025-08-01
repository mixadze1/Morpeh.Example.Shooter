using System;
using DG.Tweening;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace _Scripts.Core.Providers.Other
{
    public class ViewTextProvider : MonoProvider<ViewTextComponent>
    {
        
    }

    [Serializable]
    public struct ViewTextComponent : IComponent
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _view;

        public CanvasGroup CanvasGroup => _canvasGroup;
        
        private float _durationShow;
        private float _durationHide;
        private Sequence _sequence;
        private Sequence _sequence2;

        public void SetupDurations(float durationShow, float durationHide)
        {
            _durationShow = durationShow;
            _durationHide = durationHide;
        }

        public void ShowText(string message, Action onComplete)
        {
            _view.text = message;
            if (_sequence == null)
            {
                _sequence = DOTween.Sequence();
                _sequence.SetAutoKill(false);  
            }
            
            _sequence2.Pause();

            _canvasGroup.transform.localScale =Vector3.one;
            _sequence.Join(_canvasGroup.DOFade(1, _durationShow).SetEase(Ease.OutBack).From(0))
                .Join(_canvasGroup.transform.DOScale(Vector3.one * 1.5f, _durationShow).SetEase(Ease.OutBack))
                .OnComplete(onComplete.Invoke);
            _sequence.Restart();
        }

        public void HideImmediate()
        {
            _canvasGroup.alpha = 0;
        }
        
        public void Hide()
        {
            if (_sequence2 == null)
            {
                _sequence2 = DOTween.Sequence();
                _sequence2.SetAutoKill(false);  
            }
            
            _sequence.Pause();
            _sequence2.Join(_canvasGroup.DOFade(0, _durationHide).SetEase(Ease.OutBack).From(1))
                .Join(_canvasGroup.transform.DOScale(Vector3.one, _durationShow).SetEase(Ease.OutBack));
        }
    }
}