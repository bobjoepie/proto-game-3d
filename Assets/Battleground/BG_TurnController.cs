using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BG_TurnController : MonoBehaviour
{
    public int initiativeOrder;
    public BG_StateManager stateManager;

    void OnEnable()
    {
        BG_StateManager.Instance.Register(this);
    }
    void OnDisable()
    {
        BG_StateManager.Instance.Unregister(this);
    }
    public abstract void StartTurn();

    public abstract void EndTurn();
}
