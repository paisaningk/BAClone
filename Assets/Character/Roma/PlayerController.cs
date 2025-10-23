using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 move, mouselook, joysticklook;
    private Vector3 rotationtaget;
    public bool isPc;
   

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouselook = context.ReadValue<Vector2>();
    }
    
    public void OnJoyStickLook(InputAction.CallbackContext context)
    {
        joysticklook = context.ReadValue<Vector2>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (isPc)
        {
            RaycastHit hit;
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(mouselook);
            if (Physics.Raycast(ray, out hit))
            {
                rotationtaget = hit.point;
            }
            moveplayerWithAim();
        }
        else
        {
            if (joysticklook.x != 0 || joysticklook.y != 0)
            {
                moveplayer();
            }
            else
            {
                moveplayerWithAim();   
            }
         
        }
        
    }

    public void moveplayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.20f);
        }
        
        transform.Translate(movement * moveSpeed * Time.deltaTime,  Space.World);
    }

    public void moveplayerWithAim()
    {
        if (isPc)
        {
            // var lookPos = rotationtaget - transform.position;
            // lookPos.y = 0;
            // var rotation = Quaternion.LookRotation(lookPos);
            //
            // Vector3 aimDirection = new Vector3(rotationtaget.x, 0f, rotationtaget.z);
            //
            // if (aimDirection != Vector3.zero)
            // {
            //     transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.20f);
            // }
        }
        else
        {
            Vector3 aimDirection = new Vector3(joysticklook.x, 0f, joysticklook.y);
            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.20f);
            }
        }
        
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        transform.Translate(movement * moveSpeed * Time.deltaTime,  Space.World);
    }

    public void aimOn()
    {
        
    }
}
