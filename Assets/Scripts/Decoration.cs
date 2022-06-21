using UnityEngine;
using DG.Tweening;

public class Decoration : MonoBehaviour
{
    [SerializeField] private WrapButton _wrap;
    [SerializeField] private float _moveToY;

    private void OnEnable()
    {
        _wrap.IsWrapped += OnIsWrapped;
    }

    private void OnDisable()
    {
        _wrap.IsWrapped -= OnIsWrapped;
    }

    private void OnIsWrapped(bool isWrapped, float duration)
    {
        float startPositionY = 0;
        float positionY = isWrapped == true ? _moveToY : startPositionY;

        transform.DOMoveY(positionY, duration);
    }
}
