using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class WrapButton : MonoBehaviour
{    
    [SerializeField] private Button _button;
    [SerializeField] private Sprite _wrap;
    [SerializeField] private float _duration;
    [SerializeField] private float _wrappingPositionY;
    [SerializeField] private float _unwrappingPositionY;

    private Image _imageButton;
    private bool _isWrapped;

    public event UnityAction<bool, float> IsWrapped;

    private void OnEnable()
    {
        _button.onClick.AddListener(Wrap);
    }

    private void OnDisable()
    {
        _button.onClick?.RemoveListener(Wrap);
    }

    private void Start()
    {
        _imageButton = _button.GetComponent<Image>();
    }

    public void Wrap()
    {
        Vector3 mirrorY= _isWrapped == true ? new Vector3(1, 1, 1) : new Vector3(1, -1, 1);

        if (_isWrapped == true)
        {
            transform.DOMoveY(_unwrappingPositionY, _duration);
            _imageButton.rectTransform.localScale = mirrorY;
            _isWrapped = false;
        }
        else
        {
            transform.DOMoveY(_wrappingPositionY, _duration);
            _imageButton.rectTransform.localScale = mirrorY;
            _isWrapped = true;
        }

        IsWrapped?.Invoke(_isWrapped, _duration);
    }
}
