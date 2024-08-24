using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Bullet : MonoBehaviour
{
    Animator animator;
    RaycastHit2D hit;
    bool used = false;
    [SerializeField]
    LayerMask playerLayerMask;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(BulletTimer());
        animator.Play("Bullet-Idle");
    }

    private void Update()
    {   
        /*
        if (used)
            return;
        hit = Physics2D.CircleCast(transform.position, 0.22f, Vector2.zero, 9);
        if (hit)
        {
            used = true;
            StartCoroutine(WaitForHitAnim());
            hit.transform.gameObject.GetComponent<AdventurerMechanics>().DamagePlayer();
        }
        */
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (used) return;
        if (collision.gameObject.layer == 9)
        {
            Debug.Log("shot");
            used = true;
            StartCoroutine(WaitForHitAnim());
            GameObject.Find("/Player").GetComponent<PlayerMechanics>().DamagePlayer();
        }
    }

    IEnumerator WaitForHitAnim()
    {
        animator.SetBool("hit", true);
        yield return new WaitForSeconds(0.8f);
        Despawn();
    }
    public void Despawn()
    {
        Destroy(this.gameObject);
    }
    IEnumerator BulletTimer()
    {
        yield return new WaitForSeconds(4f);
        Despawn();//Despawn didn't hit
    }



}
