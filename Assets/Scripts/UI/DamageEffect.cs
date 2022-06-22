using System.Collections;
using UnityEngine;

public class DamageEffect : TextEffect
{
    protected override IEnumerator Play(double value)
    {
        Text.text = "-" + NumericalFormatter.Format(value);

        while (EffectIsOver != true)
        {
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 0, Step);
            transform.position += Vector3.up * 150f * Time.deltaTime;

            if (CanvasGroup.alpha <= MinimumVisibility)
                EffectIsOver = true;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
