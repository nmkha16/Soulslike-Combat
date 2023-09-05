using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 mouseDelta;
    public Vector2 moveComposite;
    public Action OnJumpPerformed;
    public Action OnAttackPerformed;
    public Action OnHeavyAttackPerformed;
    public Action OnLockedOnPerformed;
    public Action OnRollPerformed;
    public bool isRunning;
    public bool isLockedOnTarget;
    private Controls controls;

    private void OnEnable() {
        if (controls != null) return;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnJumpPerformed?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveComposite = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnAttackPerformed?.Invoke();
    }

    public void OnRunToggle(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        isRunning = !isRunning;
    }

    public void OnLockOn(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnLockedOnPerformed?.Invoke();
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnHeavyAttackPerformed?.Invoke();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnRollPerformed?.Invoke();
    }
}
