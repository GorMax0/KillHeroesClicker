using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Effect _template;

    private List<Effect> _effects = new List<Effect>();

    public void InvokeEffect(double value)
    {
        foreach (Effect _effect in _effects)
        {
            if (_effect.gameObject.activeSelf == false)
            {
                _effect.Initiate(_parent.position, value);
                return;
            }
        }

        CreateEffect(value);
    }

    private void CreateEffect(double value)
    {
        Effect effectText = Instantiate(_template, _parent.transform, false);
        effectText.Initiate(_parent.position, value);
        _effects.Add(effectText);
    }
}
