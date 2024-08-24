using System.Collections;
using UnityEngine;

public class SwordLand : State<PlayerMechanics>
{
    private static SwordLand instance;
    bool update;
    float moveSpeed = 0;
    int combo = 0;
    float timer = 0f;
    bool attacking = false;
    private SwordLand()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static SwordLand Instance
    {
        get
        {
            if (instance == null)
                new SwordLand();
            return instance;
        }
    }
    public override void EnterState(PlayerMechanics owner)
    {
        owner.playerAnimationController.SwordLand();
        update = true;
    }
    public override void ExitState(PlayerMechanics owner)
    {
        //owner.settingsPanel.SetActive(false);
    }
    public override void UpdateState(PlayerMechanics owner)
    {
        if (!owner.IsGrounded())
        {
            owner.simplyFell = true;
            owner.SwordAirState();
            return;
        }

        if (!update) return;
        if (attacking) return;
        if (Input.GetKeyDown(owner.attackInput))
        {
            Attack(owner);
            return;
        }

        if (Input.GetKeyDown(owner.jumpInput) && !owner.Blocked(owner.spriteRenderer.flipX))
        {
            owner.SwordAirState();
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
                        WalkLeftInput(owner);
                    else
                    {
                        Still(owner);
                        return;
                    }

                }
                else if (owner.Blocked(true) && !owner.Blocked(false))
                {
                    if (Input.GetKey(owner.walkRightInput))
                        WalkRightInput(owner);
                    else
                    {
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
                if (Input.GetKey(owner.walkLeftInput))
                    WalkLeftInput(owner);
                else if (Input.GetKey(owner.walkRightInput))
                    WalkRightInput(owner);
            }
        }
        else
        {
            Still(owner);
            return;
        }
        Move(owner);
    }
    private void WalkLeftInput(PlayerMechanics owner)
    {
        moveSpeed = -1;
        owner.FaceLeft();
        owner.playerAnimationController.animator.SetBool("Running", true);
    }
    private void WalkRightInput(PlayerMechanics owner)
    {
        moveSpeed = 1;
        owner.playerAnimationController.animator.SetBool("Running", true);
        owner.FaceRight();
    }
    private void Still(PlayerMechanics owner)
    {
        owner.rb2D.velocity = Vector2.up * owner.rb2D.velocity.y;
        owner.playerAnimationController.animator.SetBool("Running", false);

        if (Input.GetKeyDown(owner.attackModeInput))
            owner.StartCoroutine(End(owner));
    }
    private void Move(PlayerMechanics owner)
    {
        owner.rb2D.velocity = new Vector2(moveSpeed*owner.runSpeed, owner.rb2D.velocity.y);
    }

    IEnumerator End(PlayerMechanics owner)
    {
        update = false;
        owner.playerAnimationController.animator.SetTrigger("SwrdShte");
        yield return new WaitForSeconds(0.4f);//Sword Draw Length
        owner.NoSwordLandState();
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
        yield return new WaitForSeconds(0.4f);//Attack1 animation time 0.5f
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
        yield return new WaitForSeconds(0.5f);//Attack2 animation time 0.6f
        owner.UnlockPlayerAir();
        attacking = false;
    }
    private IEnumerator Attack3(PlayerMechanics owner)
    {
        attacking = true;
        owner.LockPlayerAir();
        owner.playerAnimationController.animator.SetTrigger("Attack3");
        yield return new WaitForSeconds(0.1f);
        owner.DamageEnemy();
        yield return new WaitForSeconds(0.5f);//Attack3 animation time 0.6f
        owner.UnlockPlayerAir();
        attacking = false;
    }
}