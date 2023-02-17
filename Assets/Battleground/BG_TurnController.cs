using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BG_TurnController : MonoBehaviour
{
    public int initiativeOrder;
    public BG_StateManager stateManager;
    public BG_EntityManager entityManager;
    public BG_EntityTags faction;
    void OnEnable()
    {
        stateManager = BG_StateManager.Instance;
        stateManager.Register(this);
        
        entityManager = BG_EntityManager.Instance;
    }
    void OnDisable()
    {
        BG_StateManager.Instance.Unregister(this);
    }
    public abstract void StartTurn();

    public abstract void EndTurn();
}
