using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputBroadcaster : MonoBehaviour
{
    
    // Player Interaction Events
    public static event Action<InputValue> OnMoveEvent;
    public static event Action<InputValue> OnLookEvent;
    public static event Action<InputValue> OnAttackEvent;
    public static event Action<InputValue> OnInteractEvent;
    public static event Action<InputValue> OnCrouchEvent;
    public static event Action<InputValue> OnJumpEvent;
    public static event Action<InputValue> OnSprintEvent;
    
    // UI Interaction Events
    public static event Action<InputValue> OnClickEvent;
    public static event Action<InputValue> OnRightClickEvent;
    public static event Action<InputValue> OnMiddleClickEvent;
    public static event Action<InputValue> OnScrollWheelEvent;
    public static event Action<InputValue> OnPointEvent;
    public static event Action OnSubmitEvent;
    public static event Action OnCancelEvent;
    
    // System/Device Events
    public static event Action OnDeviceLostEvent;
    public static event Action OnDeviceRegainedEvent;
    public static event Action OnControlsChangedEvent;
    public static event Action<InputValue> OnTrackedDevicePositionEvent;
    public static event Action<InputValue> OnTrackedDeviceOrientationEvent;

    // Player Interaction Methods
    void OnMove(InputValue inputValue)  {
        Debug.Log($"Dispatching movement input: {inputValue.Get<Vector2>()} at position {transform.position}");
        OnMoveEvent?.Invoke(inputValue);
    }

    void OnLook(InputValue inputValue)  {
        // Handle look input if needed
        OnLookEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching look input: {inputValue.Get<Vector2>()} at position {transform.position}");
    }

    void OnAttack(InputValue inputValue)  {
        // Handle attack input if needed
        OnAttackEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching attack input");
    }

    void OnInteract(InputValue inputValue)  {
        // Handle interact input if needed
        OnInteractEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching interact input");
    }

    void OnCrouch(InputValue inputValue)  {
        // Handle crouch input if needed
        OnCrouchEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching crouch input");
    }

    void OnJump(InputValue inputValue)  {
        // Handle jump input if needed
        OnJumpEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching jump input");
    }

    void OnSprint(InputValue inputValue)  {
        // Handle sprint input if needed
        OnSprintEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching sprint input");
    }

    // UI Interaction Methods
    void OnClick(InputValue inputValue)  {
        // Handle click input if needed
        OnClickEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching click input");
    }

    void OnRightClick(InputValue inputValue)  {
        // Handle right-click input if needed
        OnRightClickEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching right-click input");
    }

    void OnMiddleClick(InputValue inputValue)  {
        // Handle middle-click input if needed
        OnMiddleClickEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching middle-click input");
    }

    void OnScrollWheel(InputValue inputValue)  {
        // Handle scroll wheel input if needed
        OnScrollWheelEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching scroll wheel input");
    }

    void OnPoint(InputValue inputValue)  {
        // Handle point input if needed
        OnPointEvent?.Invoke(inputValue);
        Debug.Log($"Dispatching point input: {inputValue.Get<Vector2>()} at position {transform.position}");
    }

    void OnSubmit()  {
        // Handle submit input if needed
        Debug.Log("Submit input received");
    }

    void OnCancel()  {
        // Handle cancel input if needed
        Debug.Log("Cancel input received");
    }

    // System/Device Methods
    void OnDeviceLost()  {
        // Handle device lost if needed
        Debug.Log("Input device lost");
    }

    void OnDeviceRegained()  {
        // Handle device regained if needed
        Debug.Log("Input device regained");
    }

    void OnControlsChanged()  {
        // Handle controls changed if needed
        Debug.Log("Input controls changed");
    }

    void OnTrackedDevicePosition(InputValue inputValue)  {
        // Handle tracked device position input if needed
        Debug.Log($"Dispatching tracked device position input");
    }

    void OnTrackedDeviceOrientation(InputValue inputValue)  {
        // Handle tracked device orientation input if needed
        Debug.Log($"Dispatching tracked device orientation input");
    }

}

