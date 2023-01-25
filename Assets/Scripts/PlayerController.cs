using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public enum EntityAnimationState
{
    Idle,
    Moving
}

public class PlayerController : EntityController, IInputController
{
    private InputManager input;
    public AttackController attackController;
    public Animator animator;
    private Camera mainCamera;
    public WeaponSO equippedWeapon;
    public ItemSO equippedItem;

    public bool CanUseItem;
    public EntityAnimationState state = EntityAnimationState.Idle;

    private UIDocManager uiManager;
    private HUD_EquipmentDisplay equipmentDisplay;
    private HUD_VitalDisplay vitalDisplay;
    private GM_PauseMenu pauseMenu;

    private void OnEnable()
    {
        uiManager = UIDocManager.Instance;
        input = InputManager.Instance;

        input.Register(this, DefaultActionMaps.MovementKeyActions);
        input.Register(this, DefaultActionMaps.MenuKeyActions);
        input.Register(this, DefaultActionMaps.MouseKeyActions);
        input.Register(this, DefaultActionMaps.NumberKeyActions);
    }

    private void Start()
    {
        if (attackController == null)
        {
            attackController = GetComponent<AttackController>();
        }

        mainCamera = Camera.main;

        if (uiManager != null)
        {
            equipmentDisplay = uiManager.equipmentDisplay;
            vitalDisplay = uiManager.vitalDisplay;
            pauseMenu = uiManager.pauseMenu;
        }

        vitalDisplay.Show();
        vitalDisplay.SetHealthBar(attributes.curHealth, attributes.maxHealth);
        pauseMenu.Hide();
    }

    private void Update()
    {
        HandleMovement();
        HandleMenuing();
        HandleEquipment();
        HandleWeapons();
        HandleItems();
    }

    private void OnDisable()
    {
        input.Unregister(this);
    }

    private void HandleMovement()
    {
        var horizontal = 0f;
        var vertical = 0f;

        if (input.PollKey(this, KeyAction.Left))
        {
            horizontal -= 1f;
        }
        if (input.PollKey(this, KeyAction.Right))
        {
            horizontal += 1f;
        }
        if (input.PollKey(this, KeyAction.Up))
        {
            vertical += 1f;
        }
        if (input.PollKey(this, KeyAction.Down))
        {
            vertical -= 1f;
        }

        var movement = new Vector3(horizontal, 0, vertical).normalized;
        var rot = Quaternion.Euler(0, 45.0f, 0);
        var isoMatrix = Matrix4x4.Rotate(rot);
        var res = isoMatrix.MultiplyPoint3x4(movement);

        if (horizontal != 0f || vertical != 0f)
        {
            //ChangeAnimationState(EntityAnimationState.Moving);
        }
        else
        {
            //ChangeAnimationState(EntityAnimationState.Idle);
        }

        transform.position += new Vector3(res.x * attributes.moveSpeed * Time.deltaTime, 0, res.z * attributes.moveSpeed * Time.deltaTime);
    }

    private void ChangeAnimationState(EntityAnimationState animState)
    {
        if (state == animState) return;
        state = animState;
        switch (state)
        {
            case EntityAnimationState.Idle:
                animator.Play("StoneGuyIdle");
                break;
            case EntityAnimationState.Moving:
                animator.Play("StoneGuy");
                break;
        }
    }

    private void HandleMenuing()
    {
        if (input.PollKeyDown(this, KeyAction.Tab))
        {
            //GameMenuManager.Instance.ToggleInventoryWindow();
        }
        else if (input.PollKeyDown(this, KeyAction.Escape))
        {
            ToggleMenuActionMaps();
            pauseMenu.ToggleView();
            vitalDisplay.ToggleView();
            // Pause game here, possibly move somewhere else
            Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
        }
    }

    private void ToggleMenuActionMaps()
    {
        input.ToggleActionMaps(this);
        input.Register(this, KeyAction.Escape);
    }

    private void HandleEquipment()
    {
        for (int i = 0; i < DefaultActionMaps.NumberKeyActions.Count; i++)
        {
            if (input.PollKeyDown(this, DefaultActionMaps.NumberKeyActions[i]))
            {
                EquipFromInventory(i);
            }
        }
    }

    private void HandleWeapons()
    {
        if (input.PollKey(this, KeyAction.LeftClick) && equippedWeapon != null)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, 999f, LayerUtility.Only("Default")))
            {
                var targetPoint = hit.point;
                targetPoint.y += 1f;
                if (Vector3.Distance(transform.position, targetPoint) < 3f)
                {
                    attackController.Attack().Forget();
                }
                else
                {
                    attackController.Attack(targetPoint).Forget();
                }
            }
            else
            {
                attackController.Attack().Forget();
            }
        }
    }

    private void HandleItems()
    {
        if (CanUseItem && input.PollKey(this, KeyAction.LeftClick) && equippedItem != null)
        {

        }
    }

    private void EquipFromInventory(int i)
    {
        equippedWeapon = null;
        equippedItem = null;
        equipmentDisplay.ClearDisplay();
        var inventoryItem = inventory.ElementAtOrDefault(i);
        if (inventoryItem == null) return;

        switch (inventoryItem.pickupObj)
        {
            case WeaponSO wp:
                equippedWeapon = wp;
                equipmentDisplay.SetPrimaryWeapon(wp);
                break;
            case ItemSO it:
                equippedItem = it;
                break;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        vitalDisplay.SetHealthBar(attributes.curHealth, attributes.maxHealth);
    }

    public override void DestroyAll()
    {
        actionRadiusController.CancelAutoSortTargets();
        base.DestroyAll();
    }
}