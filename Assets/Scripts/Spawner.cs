using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Spawner _spawnPoint;
    [SerializeField] private HealthBar _healthBar;

    private List<Enemy> _enemies = new List<Enemy>();

    public Enemy CreateEnemy(Enemy enemy)
    {
        Enemy currentEnemy = Instantiate(enemy, _spawnPoint.transform.position, _spawnPoint.transform.rotation, _spawnPoint.transform);
        currentEnemy.gameObject.SetActive(false);
        _enemies.Add(currentEnemy);

        return currentEnemy;
    }

    public Enemy InvokeEnemy(Enemy enemyPrefab, double healthMultiplier, Player player)
    {
        foreach (var enemy in _enemies)
        {
            if (enemyPrefab.Name == enemy.Name && enemy.gameObject.activeSelf == false)
            {
                enemy.gameObject.SetActive(true);
                _healthBar.SetSubscription(enemy);
                enemy.Init(healthMultiplier, player);

                return enemy;
            }
        }
        
        return null;
    }
}
