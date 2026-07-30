using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private Camera _camera;
    private GameManager _gameManager;

    public void Init(GameManager gameManager)
    {
        this._gameManager = gameManager;
        _camera = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
        movementDirection = movementDirection.normalized;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        
        Vector2 worldPos = _camera.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);
        if (lookDirection.magnitude < .9f)
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            isAttacking = true;
        else if(context.canceled)
            isAttacking = false;
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Dash();
    }

    public override void Death()
    {
        base.Death();
        _gameManager.GameOver();
    }
}
