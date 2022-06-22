using System.Collections.Generic;
using UnityEngine;

public class TextEffectPool : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private TextEffect _template;

    private List<TextEffect> _effects = new List<TextEffect>();

    public void InvokeEffect(Color textColor, double value)
    {
        foreach (TextEffect _effect in _effects)
        {
            if (_effect.gameObject.activeSelf == false)
            {
                _effect.Initiate(_parent.position, textColor, value);
                return;
            }
        }

        CreateEffect(textColor, value);
    }

    private void CreateEffect(Color textColor, double value)
    {
        TextEffect effectText = Instantiate(_template, _parent.transform, false);
        effectText.Initiate(_parent.position, textColor, value);
        _effects.Add(effectText);
    }
}
