using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    public AnimancerComponent Animancer;
    
    [Button]
    public void Player(AnimationClip clip)
    {
        Animancer.Play(clip);
    }
}
