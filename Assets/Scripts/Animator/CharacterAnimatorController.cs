using System;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    public AnimancerComponent Animancer;
    public DirectionalAnimationSet8 DirectionalAnimation;
    public PlayerController  Playercontroller;
    public AnimationClip Idle;
    
    [Button]
    public void Play(AnimationClip clip)
    {
        Animancer.Play(clip);
    }

    public void Update()
    {
        Animancer.Play(Playercontroller.move != Vector2.zero ? DirectionalAnimation.Get(Playercontroller.move) : Idle);
    }
}
