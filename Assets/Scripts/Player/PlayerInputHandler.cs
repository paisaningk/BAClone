using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] PlayerContext ctx;
   
    public void OnMove(InputAction.CallbackContext c)
    {
        ctx.moveInput = c.ReadValue<Vector2>();
       
    }
    public void OnLook(InputAction.CallbackContext c)
    {
        ctx.lookInput = c.ReadValue<Vector2>();
    }
    public void OnDash(InputAction.CallbackContext c)
    {
        if (!c.performed) return;
        ctx.dash.TryDash();
    }

    public void OnFire(InputAction.CallbackContext c)
    {
        // if (!c.performed) return;
        // ctx.weapon.TryShoot();
        if (c.performed)  ctx.weapon.SetFiring(true);
        if (c.canceled)   ctx.weapon.SetFiring(false);
    }

    public void OnReload(InputAction.CallbackContext c)
    {
        if (!c.performed) return;
        ctx.weapon.TryReload();
    }
}