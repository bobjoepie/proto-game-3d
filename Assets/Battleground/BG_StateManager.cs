using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum TurnState
{
    Setup,
    
    Start,
    Current,
    End,
    
    Transition,
    Confirmation
}

public class BG_StateManager : MonoBehaviour, IInputController
{
    public static BG_StateManager Instance { get; private set; }

    private InputManager input;
    private Camera mainCamera;
    private BG_UIDocManager uiDocManager;

    public BG_TurnController currentTurnOwner;
    public TurnState currentTurnState;
    public List<BG_TurnController> turnOwners = new List<BG_TurnController>();
    public int turnIndex;
    public BG_EntityController selectedEntity;

    private BG_CardManager cardManager;
    private Action<Vector3> actionToConfirm;

    public BG_StateManager()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.Instance;
        cardManager = BG_CardManager.Instance;

        input.Register(this, DefaultActionMaps.BattlegroundActions);
        input.Register(this, DefaultActionMaps.NumberKeyActions);

        mainCamera = Camera.main;

        uiDocManager = BG_UIDocManager.Instance;

        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            InitTurnSetup();

        }).Invoke();
    }

    private void InitTurnSetup()
    {
        //currentTurnOwner = TurnOwner.Init;
        currentTurnState = TurnState.Setup;

        // do setup

        turnOwners = turnOwners.OrderBy(o => o.initiativeOrder).ToList();
        currentTurnOwner = turnOwners.First();
        turnIndex = 0;
        currentTurnState = TurnState.Start;
        StartTurn();
    }

    private void StartTurn()
    {
        currentTurnOwner.StartTurn();
        currentTurnState = TurnState.Current;
    }

    public void PassTurnOwner()
    {
        turnIndex = (turnIndex + 1) % turnOwners.Count;
        currentTurnOwner = turnOwners[turnIndex];
        currentTurnState = TurnState.Start;
        StartTurn();
    }

    public void WaitForConfirmAction(Action<Vector3> action)
    {
        if (currentTurnOwner is BG_PlayerTurnController controller)
        {
            controller.WaitForConfirmAction(action);
        }
    }

    public void SetTurnOwner(BG_TurnController turnOwner)
    {
        turnIndex = turnOwners.IndexOf(turnOwner);
        currentTurnOwner = turnOwners[turnIndex];
    }

    public void Register(BG_TurnController turnController)
    {
        turnOwners.Add(turnController);
        turnController.stateManager = this;
    }

    public void Unregister(BG_TurnController turnController)
    {
        turnOwners.Remove(turnController);
    }
}
