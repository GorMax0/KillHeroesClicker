using UnityEngine;
using UnityEngine.EventSystems;

public class Hit : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Player _player;
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private EffectPool _damageTextEffect;
    [SerializeField] private Color _textColor;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_levelHandler.CurrentEnemy != null)
        {
            _levelHandler.CurrentEnemy.ReceiveDamage(_player.DamagePerClick);
            _damageTextEffect.InvokeEffect( _player.DamagePerClick);
        }
    }
}