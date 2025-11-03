
using UnityEngine;

[RequireComponent(typeof(PlayerContext))]
public class PlayerMovementCC : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] LayerMask groundMask = ~0;

    PlayerContext ctx;
    void Awake() => ctx = GetComponent<PlayerContext>();

    void Update()
    {
        // --- move relative to camera ---
        var cam = Camera.main.transform;
        Vector3 f = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized; 
        Vector3 r = Vector3.ProjectOnPlane(cam.right,   Vector3.up).normalized; 

        Vector2 in2 = ctx.moveInput;
        Vector3 moveWorld = f * in2.y + r * in2.x;      // WASD → world dir ต
        ctx.cc.SimpleMove(moveWorld * moveSpeed);

       
        HandleAiming(groundMask);
    }

    void HandleAiming(LayerMask groundMask)
    {
        if (!ctx.isPC) return;

        var cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(ctx.lookInput);

        if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundMask))
        {
            ctx.aimTarget.position = hit.point;
        }
        else
        {
            
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float d))
                ctx.aimTarget.position = ray.GetPoint(d);
        }

        Vector3 dir = ctx.aimTarget.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.Slerp(
                transform.rotation, Quaternion.LookRotation(dir), 0.2f);
    }
}
