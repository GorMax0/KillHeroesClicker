using TMPro;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private bool _isDamagePerClick;

    private void OnEnable()
    {
        _player.DamageChanged += OnDamageChanged;
    }

    private void OnDisable()
    {
        _player.DamageChanged -= OnDamageChanged;
    }

    private void Start()
    {
        OnDamageChanged();
    }

    private void OnDamageChanged()
    {
        if (_isDamagePerClick == true)
            _damageText.text = NumericalFormatter.Format(_player.DamagePerClick);
        else
            _damageText.text = NumericalFormatter.Format(_player.DamagePerSecond);
    }
}
