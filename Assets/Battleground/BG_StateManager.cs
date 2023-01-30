using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum TurnState
{
    Setup,
    
    Start,
    Current,
    End,
    
    Transition
}

public enum TurnOwner
{
    Player,
    Enemy,
    Neutral,
    Init,
    End
}

public enum TurnState2
{
    Init_PreStart,
    Init_Start,
    Init_Current,
    Init_PreEnd,
    Init_End,

    Player_Setup,
    Enemy_Setup,
    Neutral_Setup,

    Player_PreStart,
    Player_Start,
    Player_Current,
    Player_PreEnd,
    Player_End,

    Enemy_PreStart,
    Enemy_Start,
    Enemy_Current,
    Enemy_PreEnd,
    Enemy_End,

    Neutral_PreStart,
    Neutral_Start,
    Neutral_Current,
    Neutral_PreEnd,
    Neutral_End,
    
    Cast,
    Battle,
    Transition,
    Death,

    Exit_PreStart,
    Exit_Start,
    Exit_Current,
    Exit_PreEnd,
    Exit_End,
}

public class BG_StateManager : MonoBehaviour, IInputController
{
    public static BG_StateManager Instance { get; private set; }

    private InputManager input;
    private Camera mainCamera;
    private BG_UIDocManager uiDocManager;

    public TurnOwner currentTurnOwner;
    public TurnState currentTurnState;
    public BG_EntityController selectedEntity;

    private BG_CardManager cardManager;

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
        uiDocManager.hudOverlay.InitEndTurnButton(EndTurn);

        UniTask.Action(async () =>
        {
            await UniTask.NextFrame();
            InitTurnSetup();

        }).Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        // move inputs from stateManager
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (input.PollKeyDownIgnoreUI(this, KeyAction.BG_LeftClick)
            && Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, layerMask: LayerUtility.AllBut("Default")))
        {
            HandleSelection(hit);
        }
        else if (input.PollKeyDownIgnoreUI(this, KeyAction.BG_LeftClick))
        {
            uiDocManager.hudOverlay.ClearButtonActions();
            uiDocManager.hudOverlay.HidePanel(PanelType.Button);
            if (selectedEntity != null) selectedEntity = selectedEntity.Deselect();
        }
    }

    private void HandleSelection(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent<BG_UnitController>(out var unit))
        {
            HandleUnitSelection(unit);
        }
        else if (hit.transform.TryGetComponent<BG_BuildingController>(out var building))
        {
            HandleBuildingSelection(building);
        }
        else if (hit.transform.TryGetComponent<BG_GridHandler>(out var gridHandler))
        {
            uiDocManager.hudOverlay.ClearButtonActions();
            uiDocManager.hudOverlay.HidePanel(PanelType.Button);
            if (selectedEntity != null) selectedEntity = selectedEntity.Deselect();
            HandleGridSelection(gridHandler, hit);
        }
    }

    private void HandleUnitSelection(BG_EntityController entity)
    {
        if (entity.transform.root.TryGetComponent<BG_GridHandler>(out var gridHandler))
        {
            var localCoord = Vector3Int.FloorToInt(entity.transform.parent.localPosition);
        }
        selectedEntity = entity.Select();
    }

    private void HandleBuildingSelection(BG_BuildingController building)
    {
        uiDocManager.hudOverlay.ShowPanel(PanelType.Button);
        uiDocManager.hudOverlay.InitButtons(building.LoadButtons());
        selectedEntity = building.Select();
    }

    private void HandleGridSelection(BG_GridHandler gridHandler, RaycastHit hit)
    {
        var localCoord = Vector3Int.FloorToInt(gridHandler.transform.InverseTransformPoint(hit.point));
        var x = localCoord.x;
        var y = localCoord.z;
        var childName = "No Objects";
        if (gridHandler.gridCells[x][y].transform.childCount > 0)
        {
            childName = gridHandler.gridCells[x][y].transform.GetChild(0).name;
        }

        //Debug.Log($"({localCoord.x},{localCoord.z}) contains {childName}.");
    }

    public void InitTurnSetup()
    {
        currentTurnOwner = TurnOwner.Init;
        currentTurnState = TurnState.Setup;

        // do setup

        currentTurnOwner = TurnOwner.Player;
        currentTurnState = TurnState.Start;
        StartTurn();
    }

    private void StartTurn()
    {
        // move card draw call to cardManager
        switch (currentTurnOwner)
        {
            case TurnOwner.Player:
                cardManager.DrawHand();
                break;
            case TurnOwner.Enemy:
                break;
        }

        currentTurnState = TurnState.Current;
    }

    public void EndTurn()
    {
        currentTurnState = TurnState.End;

        switch (currentTurnOwner)
        {
            case TurnOwner.Player:
                cardManager.DiscardHand();
                break;
            case TurnOwner.Enemy:
                break;
        }

        PassTurnOwner();
    }

    public void PassTurnOwner()
    {
        switch (currentTurnOwner)
        {
            case TurnOwner.Player:
                currentTurnOwner = TurnOwner.Enemy;
                break;
            case TurnOwner.Enemy:
                currentTurnOwner = TurnOwner.Player;
                break;
        }

        currentTurnState = TurnState.Start;
        StartTurn();
    }
}
