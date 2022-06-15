using UnityEngine;
using UnityEngine.EventSystems;

public class Hit : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Player _player;
    [SerializeField] private LevelHandler _levelHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_levelHandler.CurrentEnemy != null)
            _levelHandler.CurrentEnemy.ReceiveDamage(_player.DamagePerClick);
    }
}