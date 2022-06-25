using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CoinEffect : Effect
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private MoneyBalance _targe;

    protected override IEnumerator Play(double value)
    {
        bool EffectIsOver = false;
        float step = 0.01f;

        transform.position = _startPosition.position;
        Text.text = "+" + NumericalFormatter.Format(value);
        AudioSource.Play();

        while (EffectIsOver != true)
        {
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 0, step);
            transform.DOMove(_targe.transform.position, 2f);
            
            if (CanvasGroup.alpha <= MinimumVisibility)
                EffectIsOver = true;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
