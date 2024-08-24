using System.Collections;
using UnityEngine;

public class NoSwordAir : State<PlayerMechanics>
{
    private static NoSwordAir instance;
    float moveSpeed = 1;
    bool jumped;
    private NoSwordAir()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static NoSwordAir Instance
    {
        get
        {
            if (instance == null)
                new NoSwordAir();
            return instance;
        }
    }
    public override void EnterState(PlayerMechanics owner)
    {
        owner.playerAnimationController.NoSwordAir();
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
            owner.simplyFell = false;
            owner.NoSwordLandState();
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
                owner.playerAnimationController.animator.SetBool("WallSlide", false);
                if (Input.GetKey(owner.walkLeftInput))
                    WalkLeftInput(owner);
                else if (Input.GetKey(owner.walkRightInput))
                    WalkRightInput(owner);
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
            Still(owner);
            return;
        }
        if (Input.GetKey(owner.runInput))
            Run(owner);
        else
            NoRun(owner);

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
    private void Run(PlayerMechanics owner)
    {
        moveSpeed *= owner.runSpeed;
    }
    private void NoRun(PlayerMechanics owner)
    {
        moveSpeed *= owner.walkSpeed;
    }
    private void Still(PlayerMechanics owner)
    {
        owner.rb2D.velocity = Vector2.up * owner.rb2D.velocity.y;
    }
    private void Move(PlayerMechanics owner)
    {
        owner.rb2D.velocity = new Vector2(moveSpeed, owner.rb2D.velocity.y);
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
}