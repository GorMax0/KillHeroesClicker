using TMPro;
using UnityEngine;

public class MoneyBalance : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _money;

    private void OnEnable()
    {
        _player.MoneyChanged += OnMoneyChanged;
        _money.text = _player.Money.ToString();
    }

    private void OnDisable()
    {
        _player.MoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(double money)
    {
        _money.text = NumericalFormatter.Format(money);
    }
}
