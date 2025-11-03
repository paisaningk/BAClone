using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterAnimatorController_2 : MonoBehaviour
{
    [Header("Refs")]
    public AnimancerComponent Animancer;
    public PlayerContext Playercontroller;

    [Header("Clips")]
    public DirectionalAnimationSet8 DirectionalAnimation; // 8 ทิศ
    public AnimationClip Idle;
    public AnimationClip Reload;

    [Header("Tuning")]
    [Range(0f, 0.5f)] public float fade = 0.12f;    // crossfade ระหว่าง state
    public float minMoveToRun = 0.1f;               // deadzone อินพุต
    public float animSpeedAtMaxInput = 1.0f;        // ความเร็วอนิเมตอนกดเต็ม

    private AnimationClip _currentClip;

    [Button]
    public void Play(AnimationClip clip) => Animancer.Play(clip, fade);
    
    private AnimancerState _lastState;
    

    void Awake()
    {
        if (!Animancer) Animancer = GetComponent<AnimancerComponent>();
        if (!Animancer) { Debug.LogError("[Anim] Missing AnimancerComponent"); enabled = false; }
        if (!Idle) Debug.LogWarning("[Anim] Idle clip not assigned.");
    }

    void Update()
    {
        // 1) กรณีพิเศษ: รีโหลด → ล็อกอนิเมไว้ที่ Reload
        if (Reload != null && IsReloading()) // TODO: เปลี่ยนเป็นเงื่อนไขรีโหลดจริงของคุณ
        {
            PlayIfChanged(Reload);
            return;
        }

        // 2) คำนวณทิศอินพุตเป็น local ของตัวละคร
        Vector2 input = Playercontroller.moveInput;
        Vector3 world = new Vector3(input.x, 0f, input.y);
        Vector3 local = transform.InverseTransformDirection(world);
        Vector2 dir2  = new Vector2(local.x, local.z);

        // 3) ถ้าแทบไม่ขยับ → Idle
        if (dir2.sqrMagnitude < minMoveToRun * minMoveToRun)
        {
            PlayIfChanged(Idle);
            return;
        }

        // 4) เลือกคลิปตามทิศ 8 ทาง แล้ว crossfade
        dir2 = dir2.normalized;
        var moveClip = DirectionalAnimation.Get(dir2);
        var state = PlayIfChanged(moveClip);

        // 5) ปรับความเร็วอนิเมตามแรงอินพุต (optional)
        if (state != null)
        {
            float inputMag = Mathf.Clamp01(input.magnitude);
            state.Speed = Mathf.Lerp(0.8f, animSpeedAtMaxInput, inputMag);
        }
    }

    // เปลี่ยนคลิปเมื่อจำเป็นเท่านั้น + crossfade
   
    private AnimancerState PlayIfChanged(AnimationClip clip)
    {
        if (clip == null) return null;

        // ถ้าเป็นคลิปเดิม ก็คืน state เดิม (ไม่สั่งเล่นใหม่เพื่อไม่ให้รีสตาร์ท)
        if (_currentClip == clip)
            return _lastState;

        // เล่นคลิปใหม่ + เก็บ state ไว้ใช้งานต่อ
        _currentClip = clip;
        _lastState = Animancer.Play(clip, fade);
        return _lastState;
    }


    // TODO: ใส่เงื่อนไขรีโหลดจริงจากระบบปืนของคุณ
    private bool IsReloading() => false;
}
