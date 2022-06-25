using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text), typeof(CanvasGroup), typeof(AudioSource))]
public abstract class Effect : MonoBehaviour
{    
    protected readonly float MinimumVisibility = 0.15f;
    private float _fullVisibility = 1f;
    protected TMP_Text Text;
    protected CanvasGroup CanvasGroup;
    protected AudioSource AudioSource;

    private void Awake()
    {
        Text = GetComponent<TMP_Text>();
        CanvasGroup = GetComponent<CanvasGroup>();
        AudioSource = GetComponent<AudioSource>();
    }

    public void Initiate(Enemy enemy, Vector3 position)
    {
        enemy.Died += OnEnemyDied;
    }

    public void Initiate(Vector3 position, double value)
    {
        transform.position = position;
        CanvasGroup.alpha = _fullVisibility;
        gameObject.SetActive(true);
        StartCoroutine(Play(value));
    }

    protected abstract IEnumerator Play(double value);

    private void OnEnemyDied(Enemy enemy)
    {
        CanvasGroup.alpha = _fullVisibility;
        gameObject.SetActive(true);
        StartCoroutine(Play(enemy.Reward));
        enemy.Died -= OnEnemyDied;
    }
}
