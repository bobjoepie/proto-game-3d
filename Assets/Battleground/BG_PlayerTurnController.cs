using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_PlayerTurnController : BG_TurnController, IInputController
{
    private InputManager input;
    private Camera mainCamera;
    private BG_UIDocManager uiDocManager;

    public BG_EntityController selectedEntity;

    private BG_CardManager cardManager;
    private Action<Vector3> actionToConfirm;

    public bool isCurrentTurn;
    public bool isWaitingForConfirmation;
    
    void Start()
    {
        input = InputManager.Instance;
        cardManager = BG_CardManager.Instance;

        input.Register(this, DefaultActionMaps.BattlegroundActions);
        input.Register(this, DefaultActionMaps.NumberKeyActions);

        mainCamera = Camera.main;
        uiDocManager = BG_UIDocManager.Instance;

        isCurrentTurn = false;
        isWaitingForConfirmation = false;

        uiDocManager.hudOverlay.InitEndTurnButton(EndTurn);
    }
    
    void Update()
    {
        if (isWaitingForConfirmation)
        {
            CheckConfirmation();
        }
        else if (isCurrentTurn)
        {
            CheckInputs();
        }
    }

    public override void StartTurn()
    {
        isCurrentTurn = true;
        uiDocManager.hudOverlay.SetStateLabel("Player");
        cardManager.DrawHand();
    }

    public override void EndTurn()
    {
        isCurrentTurn = false;
        isWaitingForConfirmation = false;
        cardManager.DiscardHand();
        stateManager.PassTurnOwner();
    }

    private void CheckInputs()
    {
        if (input.PollKeyDownIgnoreUI(this, KeyAction.BG_LeftClick)
            && Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity,
                layerMask: LayerUtility.Only(new String[] { "Default", "PlayerCollider", "EnemyCollider", "NeutralCollider" })))
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
        if (hit.transform.root.TryGetComponent<BG_UnitController>(out var unit))
        {
            HandleUnitSelection(unit);
        }
        else if (hit.transform.root.TryGetComponent<BG_BuildingController>(out var building))
        {
            HandleBuildingSelection(building);
        }
        else if (hit.transform.root.TryGetComponent<BG_GridHandler>(out var gridHandler))
        {
            uiDocManager.hudOverlay.ClearButtonActions();
            uiDocManager.hudOverlay.HidePanel(PanelType.Button);
            if (selectedEntity != null) selectedEntity = selectedEntity.Deselect();
            HandleGridSelection(gridHandler, hit);
        }
    }

    private void HandleUnitSelection(BG_EntityController entity)
    {
        if (selectedEntity != null) selectedEntity = selectedEntity.Deselect();
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
    }

    private void CheckConfirmation()
    {
        if (input.PollKeyDownIgnoreUI(this, KeyAction.BG_Confirm)
            && Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, layerMask: LayerUtility.Only("Battleground")))
        {
            ConfirmAction(hit.point);
        }
        else if (input.PollKeyDown(this, KeyAction.BG_Cancel))
        {
            CancelAction();
        }
    }

    public void QueuePlayerConfirmAction(Action<Vector3> action)
    {
        isWaitingForConfirmation = true;
        actionToConfirm = action;
        input.HoldActionMap(this);
        input.Register(this, DefaultActionMaps.BG_ConfirmationActions);
    }

    public void ConfirmAction(Vector3 pos)
    {
        actionToConfirm.Invoke(pos);
        isWaitingForConfirmation = false;
        actionToConfirm = null;
        input.Unregister(this, DefaultActionMaps.BG_ConfirmationActions);
        input.ReleaseActionMap(this);
    }

    public void CancelAction()
    {
        isWaitingForConfirmation = false;
        actionToConfirm = null;
        input.Unregister(this, DefaultActionMaps.BG_ConfirmationActions);
        input.ReleaseActionMap(this);
    }
}
