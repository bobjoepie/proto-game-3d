using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public List<BossController> bosses = new List<BossController>();
    public List<EnemyController> enemies = new List<EnemyController>();
    private UIDocManager uiDocManager;

    private EnemyManager()
    {
        Instance = this;
    }

    private void Awake()
    {
        if (UIDocManager.Instance != null)
        {
            uiDocManager = UIDocManager.Instance;
        }
    }

    public void Register<T>(T entity)
    {
        switch (entity)
        {
            case EnemyController e:
                enemies.Add(e);
                break;
            case BossController e:
                bosses.Add(e);
                break;
        }
    }

    public void Unregister<T>(T entity)
    {
        switch (entity)
        {
            case EnemyController e:
                enemies.Remove(e);
                break;
            case BossController e:
                bosses.Remove(e);
                break;
        }
    }
}