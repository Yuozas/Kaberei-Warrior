using System.Collections;
using UnityEngine;

public class SwordAir : State<PlayerMechanics>
{
    private static SwordAir instance;

    float moveSpeed = 1;
    bool jumped;
    int combo = 0;
    bool update;
    bool attacking;
    float timer = 0f;
    private SwordAir()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static SwordAir Instance
    {
        get
        {
            if (instance == null)
                new SwordAir();
            return instance;
        }
    }
    public override void EnterState(PlayerMechanics owner)
    {
        owner.playerAnimationController.SwordAir();
        update = true;
        if (owner.simplyFell)
        {
            return;
        }
        owner.StartCoroutine(Jump(owner));

    }
    public override void ExitState(PlayerMechanics owner)
    {
        //owner.settingsPanel.SetActive(false);
    }
    public override void UpdateState(PlayerMechanics owner)
    {
        if (!update) return;

        if (owner.IsGrounded() && (jumped || owner.simplyFell))
        {
            if (owner.playerAnimationController.animator.GetBool("WallSlide"))
            {
                owner.playerAnimationController.animator.SetBool("WallSlide", false);
                if (owner.spriteRenderer.flipX)
                    owner.FaceRight();
                else
                    owner.FaceLeft();
            }
            owner.airLanded = true;
            jumped = false;
            if (combo == 3)
            {
                //update = false;
                owner.UnlockPlayerAir();
                attacking = false;
                owner.StartCoroutine(Land(owner));
            }
            else
            {
                owner.simplyFell = false;
                owner.SwordLandState();
            }
            return;
        }

        if (attacking) return;
        if (Input.GetKeyDown(owner.attackInput))
        {
            Attack(owner);
            return;
        }

        if (Input.GetKey(owner.walkLeftInput) && Input.GetKey(owner.walkRightInput))
        {
            Still(owner);
            return;
        }
        else if (Input.GetKey(owner.walkLeftInput) || Input.GetKey(owner.walkRightInput))
        {
            if (owner.Blocked())
            {
                if (owner.Blocked(false) && !owner.Blocked(true))
                {
                    if (Input.GetKey(owner.walkLeftInput))
                    {
                        owner.playerAnimationController.animator.SetBool("WallSlide", false);
                        WalkLeftInput(owner);
                    }
                    else if (owner.TouchingSlide(false))
                    {
                        owner.playerAnimationController.animator.SetBool("WallSlide", true);
                        owner.FaceRight();
                        Still(owner);
                        return;
                    }
                    else
                    {
                        //owner.FaceRight();
                        Still(owner);
                        return;
                    }

                }
                else if (owner.Blocked(true) && !owner.Blocked(false))
                {
                    if (Input.GetKey(owner.walkRightInput))
                    {
                        owner.playerAnimationController.animator.SetBool("WallSlide", false);
                        WalkRightInput(owner);
                    }
                    else if (owner.TouchingSlide(true))
                    {
                        owner.playerAnimationController.animator.SetBool("WallSlide", true);
                        owner.FaceLeft();
                        Still(owner);
                        return;
                    }
                    else
                    {
                        //owner.FaceLeft();
                        Still(owner);
                        return;
                    }
                }
                else
                {
                    Still(owner);
                    return;
                }
            }
            else
            {
                if (owner.playerAnimationController.animator.GetBool("WallSlide"))
                {
                    owner.playerAnimationController.animator.SetBool("WallSlide", false);
                    if (owner.spriteRenderer.flipX)
                        owner.FaceRight();
                    else
                        owner.FaceLeft();
                }
                if (Input.GetKey(owner.walkLeftInput))
                    WalkLeftInput(owner);
                else if (Input.GetKey(owner.walkRightInput))
                    WalkRightInput(owner);
            }
        }
        else
        {
            owner.playerAnimationController.animator.SetBool("WallSlide", false);
            Still(owner);
            return;
        }

        if (owner.Blocked())
            return;
        Move(owner);
    }
    private void WalkLeftInput(PlayerMechanics owner)
    {
        moveSpeed = -1;
        owner.FaceLeft();
    }
    private void WalkRightInput(PlayerMechanics owner)
    {
        moveSpeed = 1;
        owner.FaceRight();
    }
    private void Still(PlayerMechanics owner)
    {
        owner.rb2D.velocity = Vector2.up * owner.rb2D.velocity.y;
    }
    private void Move(PlayerMechanics owner)
    {
        owner.rb2D.velocity = new Vector2(moveSpeed * owner.runSpeed, owner.rb2D.velocity.y);
    }
    private IEnumerator Jump(PlayerMechanics owner)
    {
        jumped = false;
        owner.playerAnimationController.animator.SetTrigger("Jump");
        owner.rb2D.AddForce(Vector2.up * owner.jumpForce);
        while (owner.IsGrounded())
        {
            yield return new WaitForEndOfFrame();
        }
        jumped = true;
    }
    private IEnumerator Land(PlayerMechanics owner)
    {
        owner.playerAnimationController.animator.SetBool("Land", true);
        Still(owner);
        yield return new WaitForSeconds(0.1f);
        owner.DamageEnemy();
        yield return new WaitForSeconds(0.2f);
        owner.playerAnimationController.animator.SetBool("Land", false);
        update = true;
        combo = 0;
        owner.simplyFell = false;
        owner.SwordLandState();
    }
    private void Attack(PlayerMechanics owner)
    {
        if (attacking) return;

        if (timer <= 0)
            combo = 0;

        switch (combo)
        {
            case 0:
                owner.StartCoroutine(AttackComboTimer());
                owner.StartCoroutine(Attack1(owner));
                combo = 1;
                break;
            case 1:
                timer = 1f;
                owner.StartCoroutine(Attack2(owner));
                combo = 2;
                break;
            case 2:
                timer = 0;
                owner.StartCoroutine(Attack3(owner));
                combo = 3;
                break;
        }

    }
    private IEnumerator AttackComboTimer()
    {
        timer = 1f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator Attack1(PlayerMechanics owner)
    {
        attacking = true;
        owner.LockPlayerAir();
        owner.playerAnimationController.animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.1f);
        owner.DamageEnemy();
        yield return new WaitForSeconds(0.3f);//Attack1 animation time 0.4f
        owner.UnlockPlayerAir();
        attacking = false;
    }
    private IEnumerator Attack2(PlayerMechanics owner)
    {
        attacking = true;
        owner.LockPlayerAir();
        owner.playerAnimationController.animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(0.1f);
        owner.DamageEnemy();
        yield return new WaitForSeconds(0.2f);//Attack2 animation time 0.3f
        owner.UnlockPlayerAir();
        attacking = false;
    }
    private IEnumerator Attack3(PlayerMechanics owner)
    {
        attacking = true;
        owner.LockPlayerAir();
        owner.playerAnimationController.animator.SetTrigger("Attack3");
        yield return new WaitForSeconds(0.1f);//Attack3 ready animation time
        owner.DamageEnemy();
        owner.UnlockPlayerAir();
    }
}