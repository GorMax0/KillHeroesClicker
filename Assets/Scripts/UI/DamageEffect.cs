using System.Collections;
using UnityEngine;

public class DamageEffect : Effect
{
    [SerializeField] private AudioClip[] _audioEffects;

    protected override IEnumerator Play(double value)
    {
        float step = 0.03f;
        float speed = 350f;
        float deviation = 0.22f;
        float deviationX = transform.position.x * Random.Range(-deviation, deviation);
        float deviationY = transform.position.x * Random.Range(-deviation, deviation);
        Vector3 vectorDeviatino = new Vector3(deviationX, deviationY, 0);
        int indexAudioClip;
        
        transform.position += vectorDeviatino;
        Text.text = "-" + NumericalFormatter.Format(value);

        if (_audioEffects != null)
        {
            indexAudioClip = Random.Range(0, _audioEffects.Length);
            AudioSource.clip = _audioEffects[indexAudioClip];
            AudioSource.Play();
        }

        while (CanvasGroup.alpha >= MinimumVisibility)
        {
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 0, step);
            transform.position += Vector3.up * speed * Time.deltaTime;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
