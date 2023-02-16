using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BG_EnemyTurnController : BG_TurnController
{
    private BG_UIDocManager uiDocManager;
    // Start is called before the first frame update
    void Start()
    {
        uiDocManager = BG_UIDocManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartTurn()
    {
        uiDocManager.hudOverlay.SetStateLabel("Enemy");
        UniTask.Action(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            EndTurn();

        }).Invoke();
    }

    public override void EndTurn()
    {
        stateManager.PassTurnOwner();
    }
}
