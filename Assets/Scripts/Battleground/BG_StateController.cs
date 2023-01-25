using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState
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

public class BG_StateController : MonoBehaviour, IInputController
{
    private InputManager input;
    private Camera mainCamera;
    private BG_HUD_Overlay hudOverlay;

    public TurnState currentTurnState;
    public BG_EntityController selectedEntity;

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.Instance;
        input.Register(this, DefaultActionMaps.BattlegroundActions);
        input.Register(this, DefaultActionMaps.NumberKeyActions);

        mainCamera = Camera.main;

        hudOverlay = BG_UIDocManager.Instance.hudOverlay;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (input.PollKeyDownIgnoreUI(this, KeyAction.BG_LeftClick)
            && Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, layerMask: LayerUtility.AllBut("Default")))
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
                hudOverlay.ClearButtonActions();
                if (selectedEntity != null) selectedEntity = selectedEntity.Deselect();
                HandleGridSelection(gridHandler, hit);
            }
        }
        else if (input.PollKeyDownIgnoreUI(this, KeyAction.BG_LeftClick))
        {
            hudOverlay.ClearButtonActions();
            if (selectedEntity != null) selectedEntity = selectedEntity.Deselect();
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
        hudOverlay.InitButtons(building.LoadButtons());
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
}
