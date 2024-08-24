using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController[] runtimeAnimatorController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void NoSwordAir()
    {
        animator.runtimeAnimatorController = runtimeAnimatorController[0];
    }
    public void NoSwordLand()
    {
        animator.runtimeAnimatorController = runtimeAnimatorController[1];
    }
    public void SwordAir()
    {
        animator.runtimeAnimatorController = runtimeAnimatorController[2];
    }
    public void SwordLand()
    {
        animator.runtimeAnimatorController = runtimeAnimatorController[3];
    }
}
