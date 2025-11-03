using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerContext))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash")]
    public float dashDistance = 5f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;
    public float dashInvincibleTime = 0.15f;
    public float dashCost = 10f;
    public LayerMask obstacleMask = ~0;
    public UIManager ui;

    PlayerContext ctx;
    Vector3 lastMoveDir = Vector3.forward;
    bool isDashing, onCooldown;

    public bool IsDashing => isDashing;

    void Awake() => ctx = GetComponent<PlayerContext>();

    public void TryDash()
    {
        if (isDashing || onCooldown) return;
        if (!ctx.stamina.Spend(dashCost)) { Debug.Log("Not enough stamina"); return; }
        
      

        // ทิศจาก WASD หรือทิศล่าสุด
        Vector3 inputDir = new Vector3(ctx.moveInput.x, 0f, ctx.moveInput.y);
        Vector3 dashDir = (inputDir.sqrMagnitude > 0.001f ? inputDir : lastMoveDir);
        if (dashDir.sqrMagnitude < 0.001f) dashDir = transform.forward;
        dashDir.Normalize();
        if (inputDir.sqrMagnitude > 0.001f) lastMoveDir = dashDir;

        // กันชนด้วย CapsuleCast
        float radius = ctx.cc.radius;
        float height = ctx.cc.height;
        Vector3 center = transform.position + Vector3.up * (height * 0.5f);
        Vector3 p1 = center + Vector3.up * (height * 0.5f - radius);
        Vector3 p2 = center - Vector3.up * (height * 0.5f - radius);

        float maxDist = dashDistance;
        if (Physics.CapsuleCast(p1, p2, radius, dashDir, out var hit, dashDistance, obstacleMask, QueryTriggerInteraction.Ignore))
            maxDist = Mathf.Max(0f, hit.distance - 0.05f);

        StartCoroutine(DashRoutine(dashDir, maxDist));
    }

    IEnumerator DashRoutine(Vector3 dir, float dist)
    {
        isDashing = true; onCooldown = true;

        float speed = (dist <= 0f) ? 0f : dist / dashDuration;
        StartCoroutine(InvincibleTimer(dashInvincibleTime));

        float t = 0f;
        while (t < dashDuration)
        {
            ctx.cc.Move(dir * speed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        onCooldown = false;
    }

    IEnumerator InvincibleTimer(float time)
    {
        ctx.health.SetInvincible(true);
        yield return new WaitForSeconds(time);
        ctx.health.SetInvincible(false);
    }
}
