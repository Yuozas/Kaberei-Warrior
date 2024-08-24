using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1AnimationController : MonoBehaviour
{
    [SerializeField]
    Animator enemy, gun;

    public void StartRun()
    {
        enemy.SetBool("running", true);
    }
    public void StopRun()
    {
        enemy.SetBool("running", false);
    }
    public void Jump()
    {
        enemy.SetTrigger("jump");
    }
    public void Land()
    {
        enemy.SetTrigger("land");
    }
    public void Hit()
    {
        enemy.SetTrigger("hit");
    }
    public void Shoot()
    {
        enemy.SetTrigger("shoot");
    }
    public void Die()
    {
        enemy.SetTrigger("die");
    }
    public void GunShoot()
    {
        gun.SetTrigger("shoot");
    }
    public void BulletHit(Animator bullet)
    {
        bullet.SetTrigger("hit");
    }
}
