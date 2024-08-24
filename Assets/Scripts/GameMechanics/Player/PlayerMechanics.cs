using MessagePack;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    public StateMachine<PlayerMechanics> StateMachine { get; set; }
    public PlayerAnimationController playerAnimationController;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb2D;
    public BoxCollider2D boxCollider2d;
    public LayerMask platformLayerMask;
    public LayerMask enemyLayerMask;
    [HideInInspector]
    public KeyCode attackInput, jumpInput, walkLeftInput, walkRightInput, runInput, attackModeInput;
    public BoxCollider2D hitBoxRight;
    public BoxCollider2D hitBoxLeft;
    public float runSpeed;
    public float walkSpeed;
    public float jumpForce;
    public BlockedMovement rightSideBlock, leftSideBlock;
    public WallSlide rightSideWallSlide, leftSideWallSlide;
    public bool airLanded = false;
    public bool simplyFell = false;
    private HealthBarController healthBarController;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        healthBarController = GetComponent<HealthBarController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //testing
        if (CurrentSave.Instance.SaveFile == null)
            CurrentSave.Instance.SaveFile = 0 + ".kabereiSave";
        //testing

        SaveFile save = MessagePackSerializer.Deserialize<SaveFile>(File.ReadAllBytes(Application.persistentDataPath + "/Saves/" + CurrentSave.Instance.SaveFile));
        transform.position = new Vector3(save.PlayerPositionX, save.PlayerPositionY,transform.position.z);
        StateMachine = new StateMachine<PlayerMechanics>(this);
        SetInput();
        NoSwordLandState();
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.Upate();
    }
    
    #region States switching for buttons
    public void NoSwordAirState()
    {
        StateMachine.ChangeState(NoSwordAir.Instance);
    }
    public void NoSwordLandState()
    {
        StateMachine.ChangeState(NoSwordLand.Instance);
    }
    public void SwordAirState()
    {
        StateMachine.ChangeState(SwordAir.Instance);
    }
    public void SwordLandState()
    {
        StateMachine.ChangeState(SwordLand.Instance);
    }
    #endregion
    #region Mechanics states common methods
    public void FaceRight()
    {
        spriteRenderer.flipX = false;
    }
    public void FaceLeft()
    {
        spriteRenderer.flipX = true;
    }
    public bool IsGrounded()
    {
        /*
        float extraHeightText = 0.05f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);
        Color rayColor;
        rayColor  = raycastHit.collider != null ? Color.green : rayColor = Color.red;
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);
        return raycastHit.collider != null;
        */
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.05f, platformLayerMask);
        return raycastHit.collider != null;
    }
    public void LockPlayerAir()
    {
        rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void UnlockPlayerAir()
    {
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public bool Blocked()
    {
        return rightSideBlock.blockedMovement || leftSideBlock.blockedMovement;
    }
    public bool Blocked(bool facingDirection)
    {
        switch (facingDirection)
        {
            case false:
                return rightSideBlock.blockedMovement;
            default:
                return leftSideBlock.blockedMovement;
        }
    }
    public bool TouchingSlide()
    {
        return rightSideWallSlide.blockedMovement || leftSideWallSlide.blockedMovement;
    }
    public bool TouchingSlide(bool facingDirection)
    {
        switch (facingDirection)
        {
            case false:
                return rightSideWallSlide.blockedMovement;
            default:
                return leftSideWallSlide.blockedMovement;
        }
    }
    public void DamageEnemy()
    {
        /*
        switch (spriteRenderer.flipX)
        {
            case false:
                //right
                foreach(Collider2D collider in hitBoxRight.collisions)
                {
                    collider.SendMessage("Damage");
                }
                break;
            default:
                //left
                foreach (Collider2D collider in hitBoxLeft.collisions)
                {
                    collider.SendMessage("Damage");
                }
                break;
        }
        */
        RaycastHit2D raycastHit;
        switch (spriteRenderer.flipX)
        {
            case false:
                //right
                raycastHit = Physics2D.BoxCast(hitBoxRight.bounds.center, hitBoxRight.bounds.size, 0, Vector2.zero, 0, enemyLayerMask);
                break;
            default:
                //left
                raycastHit = Physics2D.BoxCast(hitBoxLeft.bounds.center, hitBoxLeft.bounds.size, 0, Vector2.zero, 0, enemyLayerMask);
                break;
        }
        if(raycastHit)
        {
            raycastHit.collider.SendMessage("Damage");
        }
    }
    #endregion
    public void SetInput()
    {
        attackInput = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("attackInput"));
        jumpInput = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpInput"));
        walkLeftInput = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("walkLeftInput"));
        walkRightInput = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("walkRightInput"));
        runInput = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("runInput"));
        attackModeInput = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("attackModeInput"));
    }
    public void SaveGame()
    {
        SaveFile save = MessagePackSerializer.Deserialize<SaveFile>(File.ReadAllBytes(Application.persistentDataPath + "/Saves/" + CurrentSave.Instance.SaveFile));
        save.PlayerPositionX = transform.position.x;
        save.PlayerPositionY = transform.position.y;
        File.WriteAllBytes(Application.persistentDataPath + "/Saves/" + CurrentSave.Instance.SaveFile, MessagePackSerializer.Serialize(save));
    }
    public void DamagePlayer()
    {
        if (healthBarController.CheckIfDead(0.25f))
        {
            healthBarController.MinusHealth(0.25f);
            ObjetcDie();
        }
        healthBarController.MinusHealth(0.25f);
    }
    public void ObjetcDie()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLoader");
    }
}