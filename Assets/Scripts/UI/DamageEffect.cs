using System.Collections;
using UnityEngine;

public class DamageEffect : Effect
{
    [SerializeField] private AudioClip[] _audioEffects;

    protected override IEnumerator Play(double value)
    {
        float speed = 350f;
        int indexAudioClip;

        Text.text = "-" + NumericalFormatter.Format(value);

        if (_audioEffects != null)
        {
            indexAudioClip = Random.Range(0, _audioEffects.Length);
            AudioSource.clip = _audioEffects[indexAudioClip];
            AudioSource.Play();
        }

        while (EffectIsOver != true)
        {
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 0, Step);
            transform.position += Vector3.up * speed * Time.deltaTime;

            if (CanvasGroup.alpha <= MinimumVisibility)
                EffectIsOver = true;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
