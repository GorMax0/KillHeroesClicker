using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text), typeof(CanvasGroup), typeof(AudioSource))]
public abstract class Effect : MonoBehaviour
{
    protected readonly float Step = 0.02f;
    protected readonly float MinimumVisibility = 0.1f;
    protected bool EffectIsOver = false;
    protected TMP_Text Text;
    protected CanvasGroup CanvasGroup;
    protected AudioSource AudioSource;

    private void Awake()
    {
        Text = GetComponent<TMP_Text>();
        CanvasGroup = GetComponent<CanvasGroup>();
        AudioSource = GetComponent<AudioSource>();
    }

    public void Initiate(Vector3 position, Color textColor, double value)
    {
        float fullVisibility = 1f;

        EffectIsOver = false;
        transform.position = position;
        CanvasGroup.alpha = fullVisibility;
        Text.outlineColor = textColor;
        gameObject.SetActive(true);
        StartCoroutine(Play(value));
    }

    protected abstract IEnumerator Play(double value);
}
