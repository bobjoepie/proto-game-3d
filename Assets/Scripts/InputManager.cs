using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum KeyAction
{
    LeftClick,
    RightClick,

    Up,
    Left,
    Down,
    Right,

    Use,
    SpaceKey,
    EnterKey,

    Slot1,
    Slot2,
    Slot3,
    Slot4,
    Slot5,
    Slot6,
    Slot7,
    Slot8,
    Slot9,
    Slot0,

    Tab,
    Escape,
    Shift,
    Ctrl,
    Alt,

    DialogueContinue,
    DialogueSkip,

    BG_LeftClick,
    BG_RightClick,
    BG_SpaceKey,

    BG_Confirm,
    BG_Cancel,

    BG_Up,
    BG_Left,
    BG_Down,
    BG_Right,

    BG_ZoomIn,
    BG_ZoomOut,
    BG_ResetZoom,

    BG_SpeedUpCamera,
}

public static class DefaultActionMaps
{
    public static readonly List<KeyAction> MouseKeyActions = new List<KeyAction>()
    {
        KeyAction.LeftClick,
        KeyAction.RightClick,
    };

    public static readonly List<KeyAction> MovementKeyActions = new List<KeyAction>()
    {
        KeyAction.Up,
        KeyAction.Left,
        KeyAction.Down,
        KeyAction.Right,
    };

    public static readonly List<KeyAction> NumberKeyActions = new List<KeyAction>()
    {
        KeyAction.Slot1,
        KeyAction.Slot2,
        KeyAction.Slot3,
        KeyAction.Slot4,
        KeyAction.Slot5,
        KeyAction.Slot6,
        KeyAction.Slot7,
        KeyAction.Slot8,
        KeyAction.Slot9,
        KeyAction.Slot0,
    };

    public static readonly List<KeyAction> MenuKeyActions = new List<KeyAction>()
    {
        KeyAction.Tab,
        KeyAction.Escape,
    };

    public static readonly List<KeyAction> DialogueKeyActions = new List<KeyAction>()
    {
        KeyAction.DialogueContinue,
        KeyAction.DialogueSkip,
    };

    public static readonly List<KeyAction> BattlegroundActions = new List<KeyAction>()
    {
        KeyAction.BG_LeftClick,
        KeyAction.BG_RightClick,
        KeyAction.BG_SpaceKey,
    };

    public static readonly List<KeyAction> BG_ConfirmationActions = new List<KeyAction>()
    {
        KeyAction.BG_Confirm,
        KeyAction.BG_Cancel,
    };

    public static readonly List<KeyAction> BG_CameraActions = new List<KeyAction>()
    {
        KeyAction.BG_Up,
        KeyAction.BG_Left,
        KeyAction.BG_Down,
        KeyAction.BG_Right,

        KeyAction.BG_ZoomIn,
        KeyAction.BG_ZoomOut,
        KeyAction.BG_ResetZoom,

        KeyAction.BG_SpeedUpCamera,
    };
}

public interface IInputController {}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private readonly Dictionary<IInputController, HashSet<KeyAction>> actionMaps = new Dictionary<IInputController, HashSet<KeyAction>>();
    private readonly Dictionary<IInputController, HashSet<KeyAction>> heldActionMaps = new Dictionary<IInputController, HashSet<KeyAction>>();

    private readonly Dictionary<KeyAction, KeyCode> KeyActionMap = new Dictionary<KeyAction, KeyCode>()
    {
        {KeyAction.LeftClick            ,       KeyCode.Mouse0},
        {KeyAction.RightClick           ,       KeyCode.Mouse1},

        {KeyAction.Up                   ,       KeyCode.W},
        {KeyAction.Left                 ,       KeyCode.A},
        {KeyAction.Down                 ,       KeyCode.S},
        {KeyAction.Right                ,       KeyCode.D},

        {KeyAction.Use                  ,       KeyCode.E},
        {KeyAction.SpaceKey             ,       KeyCode.Space},
        {KeyAction.EnterKey             ,       KeyCode.Return},

        {KeyAction.Slot1                ,       KeyCode.Alpha1},
        {KeyAction.Slot2                ,       KeyCode.Alpha2},
        {KeyAction.Slot3                ,       KeyCode.Alpha3},
        {KeyAction.Slot4                ,       KeyCode.Alpha4},
        {KeyAction.Slot5                ,       KeyCode.Alpha5},
        {KeyAction.Slot6                ,       KeyCode.Alpha6},
        {KeyAction.Slot7                ,       KeyCode.Alpha7},
        {KeyAction.Slot8                ,       KeyCode.Alpha8},
        {KeyAction.Slot9                ,       KeyCode.Alpha9},
        {KeyAction.Slot0                ,       KeyCode.Alpha0},

        {KeyAction.Tab                  ,       KeyCode.Tab},
        {KeyAction.Escape               ,       KeyCode.Escape},
        {KeyAction.Shift                ,       KeyCode.LeftShift},
        {KeyAction.Ctrl                 ,       KeyCode.LeftControl},
        {KeyAction.Alt                  ,       KeyCode.LeftAlt},

        {KeyAction.DialogueContinue     ,       KeyCode.Return},
        {KeyAction.DialogueSkip         ,       KeyCode.Escape},

        {KeyAction.BG_LeftClick         ,       KeyCode.Mouse0},
        {KeyAction.BG_RightClick        ,       KeyCode.Mouse1},
        {KeyAction.BG_SpaceKey          ,       KeyCode.Space},

        {KeyAction.BG_Confirm           ,       KeyCode.Mouse0},
        {KeyAction.BG_Cancel            ,       KeyCode.Mouse1},

        {KeyAction.BG_Up                ,       KeyCode.W},
        {KeyAction.BG_Left              ,       KeyCode.A},
        {KeyAction.BG_Down              ,       KeyCode.S},
        {KeyAction.BG_Right             ,       KeyCode.D},

        {KeyAction.BG_ZoomIn            ,       KeyCode.PageUp},
        {KeyAction.BG_ZoomOut           ,       KeyCode.PageDown},
        {KeyAction.BG_ResetZoom         ,       KeyCode.Home},

        {KeyAction.BG_SpeedUpCamera     ,       KeyCode.LeftShift},
    };

    private InputManager()
    {
        Instance = this;
    }

    public bool PollKeyDown(IInputController entity, KeyAction action)
    {
        if (Input.GetKeyDown(KeyActionMap[action]) && actionMaps.ContainsKey(entity) && actionMaps[entity].Contains(action))
        {
            return true;
        }
        return false;
    }

    public bool PollKeyUp(IInputController entity, KeyAction action)
    {
        if (Input.GetKeyUp(KeyActionMap[action]) && actionMaps.ContainsKey(entity) && actionMaps[entity].Contains(action))
        {
            return true;
        }
        return false;
    }

    public bool PollKey(IInputController entity, KeyAction action)
    {
        if (Input.GetKey(KeyActionMap[action]) && actionMaps.ContainsKey(entity) && actionMaps[entity].Contains(action))
        {
            return true;
        }
        return false;
    }

    public bool PollKeyDownIgnoreUI(IInputController entity, KeyAction action)
    {
        if (Input.GetKeyDown(KeyActionMap[action]) && actionMaps.ContainsKey(entity) && actionMaps[entity].Contains(action) && !IsHoveringOverUI())
        {
            return true;
        }
        return false;
    }

    public bool PollKeyUpIgnoreUI(IInputController entity, KeyAction action)
    {
        if (Input.GetKeyUp(KeyActionMap[action]) && actionMaps.ContainsKey(entity) && actionMaps[entity].Contains(action) && !IsHoveringOverUI())
        {
            return true;
        }
        return false;
    }

    public bool PollKeyIgnoreUI(IInputController entity, KeyAction action)
    {
        if (Input.GetKey(KeyActionMap[action]) && actionMaps.ContainsKey(entity) && actionMaps[entity].Contains(action) && !IsHoveringOverUI())
        {
            return true;
        }
        return false;
    }

    private bool IsHoveringOverUI()
    {
        return BG_UIDocManager.Instance.IsHoveringOverUI();
    }

    public void Remap(KeyAction keyAction, KeyCode keyCode)
    {
        if (KeyActionMap.ContainsKey(keyAction))
        {
            KeyActionMap[keyAction] = keyCode;
        }
    }

    public void ToggleActionMaps(IInputController entity)
    {
        if (heldActionMaps.ContainsKey(entity))
        {
            ReleaseActionMap(entity);
        }
        else if (actionMaps.ContainsKey(entity))
        {
            HoldActionMap(entity);
        }
    }

    public void HoldActionMap(IInputController entity)
    {
        if (heldActionMaps.ContainsKey(entity))
        {
            heldActionMaps[entity].UnionWith(actionMaps[entity]);
        }
        else
        {
            heldActionMaps.Add(entity, actionMaps[entity]);
        }
        actionMaps.Remove(entity);
    }

    public void ReleaseActionMap(IInputController entity)
    {
        if (actionMaps.ContainsKey(entity))
        {
            actionMaps[entity].UnionWith(heldActionMaps[entity]);
        }
        else
        {
            actionMaps.Add(entity, heldActionMaps[entity]);
        }
        heldActionMaps.Remove(entity);
    }

    public void Register<T>(T entity, List<KeyAction> actionMap)
    {
        switch (entity)
        {
            case IInputController e:
                if (actionMaps.ContainsKey(e))
                {
                    actionMaps[e].UnionWith(actionMap);
                }
                else
                {
                    actionMaps.Add(e, actionMap.ToHashSet());
                }
                break;
        }
    }

    public void Register<T>(T entity, KeyAction action)
    {
        switch (entity)
        {
            case IInputController e:
                if (actionMaps.ContainsKey(e))
                {
                    actionMaps[e].Add(action);
                }
                else
                {
                    actionMaps.Add(e, new HashSet<KeyAction>() { action });
                }
                break;
        }
    }

    public void Unregister<T>(T entity, List<KeyAction> actionMap = null)
    {
        switch (entity)
        {
            case IInputController e:
                if (actionMap == null)
                {
                    actionMaps.Remove(e);
                }
                else if (actionMaps.ContainsKey(e))
                {
                    actionMaps[e].ExceptWith(actionMap);
                }
                break;
        }
    }

    public void Unregister<T>(T entity, KeyAction action)
    {
        switch (entity)
        {
            case IInputController e:
                if (actionMaps.ContainsKey(e))
                {
                    actionMaps[e].Remove(action);
                }
                break;
        }
    }
}
