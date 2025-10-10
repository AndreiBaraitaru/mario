using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    public UnityEvent jump;
    public UnityEvent jumpHold;
    public UnityEvent<int> moveCheck;

    void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("gameplay");
        }
    }

    public void OnMoveAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float moveValue = context.ReadValue<float>();
            int faceRight = moveValue > 0 ? 1 : moveValue < 0 ? -1 : 0;

            if (faceRight != 0)
            {
                moveCheck.Invoke(faceRight);
            }
            else
            {
                moveCheck.Invoke(0);
            }
        }

        if (context.canceled)
        {
            moveCheck.Invoke(0);
        }
    }

    public void OnJumpAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jump.Invoke();
        }
    }

    public void OnJumpHoldAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpHold.Invoke();
        }
    }
}
