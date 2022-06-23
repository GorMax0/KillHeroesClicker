using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Effect _template;

    private List<Effect> _effects = new List<Effect>();

    public void InvokeEffect(Color textColor, double value)
    {
        foreach (Effect _effect in _effects)
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
        Effect effectText = Instantiate(_template, _parent.transform, false);
        effectText.Initiate(_parent.position, textColor, value);
        _effects.Add(effectText);
    }
}
