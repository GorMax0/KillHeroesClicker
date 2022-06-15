using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MultiplierSell : MonoBehaviour
{
    [SerializeField] private Button _multiplierButton;
    [SerializeField] private TMP_Text _buttonText;

    private int _multiplier;
    private int[] _multipliers = new int[] { 1, 10, 25, 50 };
    private int _index;    

    public event UnityAction<int> MultiplierChanged;

    private void OnEnable()
    {
        _multiplierButton.onClick.AddListener(SetMultiplier);        
    }

    private void OnDisable()
    {
        _multiplierButton.onClick.RemoveListener(SetMultiplier);
    }

    private void Start()
    {
        _multiplier = _multipliers[_index];
        MultiplierChanged?.Invoke(_multiplier);
    }

    private void SetMultiplier()
    {
        _index++;

        if (_index == _multipliers.Length)
            _index = 0;

        _multiplier = _multipliers[_index];
        _buttonText.text = $"x{_multiplier}";
        MultiplierChanged?.Invoke(_multiplier);
    }
}
