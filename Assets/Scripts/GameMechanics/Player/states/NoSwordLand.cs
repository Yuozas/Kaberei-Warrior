using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSwordLand : State<PlayerMechanics>
{
    private static NoSwordLand instance;
    float moveSpeed;
    bool update = true;
    private NoSwordLand()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static NoSwordLand Instance
    {
        get
        {
            if (instance == null)
                new NoSwordLand();
            return instance;
        }
    }
    public override void EnterState(PlayerMechanics owner)
    {
        owner.playerAnimationController.NoSwordLand();
    }
    public override void ExitState(PlayerMechanics owner)
    {
        //owner.settingsPanel.SetActive(false);
    }
    public override void UpdateState(PlayerMechanics owner)
    {
        //jumpInput, walkLeftInput, walkRightInput, runInput
        if (!owner.IsGrounded())
        {
            owner.simplyFell = true;
            owner.NoSwordAirState();
            return;
        }

        if (Input.GetKeyDown(owner.jumpInput) && !owner.Blocked(owner.spriteRenderer.flipX))
        {
            owner.NoSwordAirState();
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
                else if(owner.Blocked(true) && !owner.Blocked(false))
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
    private void Still(PlayerMechanics owner)
    {
        owner.rb2D.velocity = Vector2.up * owner.rb2D.velocity.y;
        owner.playerAnimationController.animator.SetBool("Walking", false);
        if (Input.GetKeyDown(owner.attackModeInput))
            owner.StartCoroutine(StartSwordMode(owner));
            
    }
    private void Run(PlayerMechanics owner)
    {
        moveSpeed *= owner.runSpeed;
        owner.playerAnimationController.animator.SetBool("Walking", true);
        owner.playerAnimationController.animator.SetBool("Running", true);
    }
    private void NoRun(PlayerMechanics owner)
    {
        moveSpeed *= owner.walkSpeed;
        owner.playerAnimationController.animator.SetBool("Walking", true);
        owner.playerAnimationController.animator.SetBool("Running", false);
    }
    private void Move(PlayerMechanics owner)
    {
        owner.rb2D.velocity = new Vector2(moveSpeed, owner.rb2D.velocity.y);
    }
    IEnumerator StartSwordMode(PlayerMechanics owner)
    {
        update = false;
        owner.playerAnimationController.animator.SetTrigger("SwrdDrw");
        yield return new WaitForSeconds(0.4f);//Sword Draw Length
        update = true;
        owner.SwordLandState();
    }
    
}