using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelType
{
    [SerializeField] private string _label;
    [SerializeField] private List<Enemy> _enemies = new List<Enemy>();
    [SerializeField] private Decoration _decoration;
    [SerializeField] private Spawner _spawnerPoint;

    private bool _isFillEnemy;

    public string Label => _label;
    public Spawner SpawnerPoint => _spawnerPoint;

    public void FillSpawner()
    {
        if (_isFillEnemy == false)
        {
            foreach (var enemy in _enemies)
            {
                _spawnerPoint.CreateEnemy(enemy);
            }
            _isFillEnemy = true;
        }
    }

    public Enemy GetRandomEnemy()
    {
        int index = UnityEngine.Random.Range(0, _enemies.Count);

        return _enemies[index];
    }

    public void SetActiveDecoration(bool isEnable)
    {
        if (_decoration != null)
            _decoration.gameObject.SetActive(isEnable);
    }
}
