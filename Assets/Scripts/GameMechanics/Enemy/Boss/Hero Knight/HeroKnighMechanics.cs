using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnighMechanics : MonoBehaviour
{
    public StateMachine<HeroKnighMechanics> StateMachine { get; set; }
    public PlayerMechanics playerMechanics;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb2D;
    public GameObject attack1HitBox, attack2HitBox;
    public BoxCollider2D attack1HitBoxBC2D, attack2HitBoxBC2D;
    public InRange inRange;
    public Transform target;
    public Animator animator;
    public float moveSpeed;
    public int health = 10;
    public bool getBeaten = false, die = false;
    public int nextAttack = 1;
    public bool attacking = false;
    public LayerMask platformLayerMask;
    public FirstSceneDefeated firstSceneDefeated;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StateMachine = new StateMachine<HeroKnighMechanics>(this);
        Idle();
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.Upate();
    }
    public void Idle()
    {
        StateMachine.ChangeState(IdleKnight.Instance);
    }
    public void Attack()
    {
        StateMachine.ChangeState(AttackKnight.Instance);
    }
    public void Seek()
    {
        StateMachine.ChangeState(SeekKnight.Instance);
    }
    public void GetBeaten()
    {
        StateMachine.ChangeState(GetBeatenKnight.Instance);
    }
    public IEnumerator WaitForFrames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
    public void Damage()
    {
        health--;
        if (health < 0)
        {
            die = true;
        }
        else
        {
            getBeaten = true;
        }
        GetBeaten();
    }

    public void Die()
    {
        firstSceneDefeated.DefeatedScene(true);
        gameObject.SetActive(false);
    }
}
public class IdleKnight : State<HeroKnighMechanics>
{
    private static IdleKnight instance;
    private IdleKnight()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static IdleKnight Instance
    {
        get
        {
            if (instance == null)
                new IdleKnight();
            return instance;
        }
    }
    public override void EnterState(HeroKnighMechanics owner)
    {
        //owner.audioSettingsPanel.SetActive(true);
    }
    public override void ExitState(HeroKnighMechanics owner)
    {
        //owner.audioSettingsPanel.SetActive(false);
    }
    public override void UpdateState(HeroKnighMechanics owner)
    {
        if (owner.inRange.inRange)
            owner.Attack();
        else
            owner.Seek();
    }

}

public class GetBeatenKnight : State<HeroKnighMechanics>
{
    private static GetBeatenKnight instance;
    private GetBeatenKnight()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static GetBeatenKnight Instance
    {
        get
        {
            if (instance == null)
                new GetBeatenKnight();
            return instance;
        }
    }
    public override void EnterState(HeroKnighMechanics owner)
    {
        if (owner.die)
        {
            owner.StartCoroutine(Die(owner));
        }
        else if(owner.getBeaten)
        {
            owner.StartCoroutine(GetBeaten(owner));
        }
    }
    public override void ExitState(HeroKnighMechanics owner)
    {
        //owner.audioSettingsPanel.SetActive(false);
    }
    public override void UpdateState(HeroKnighMechanics owner)
    {

    }
    private IEnumerator Die(HeroKnighMechanics owner)
    {
        owner.animator.SetTrigger("Die");
        yield return new WaitForSeconds(1.1f);
        owner.die = false;
        owner.getBeaten = false;
        owner.Die();
    }
    private IEnumerator GetBeaten(HeroKnighMechanics owner)
    {
        owner.animator.SetBool("TakeHit", true);
        yield return new WaitForSeconds(0.4f);
        owner.animator.SetBool("TakeHit", false);
        owner.die = false;
        owner.getBeaten = false;
        owner.Idle();
    }
}
public class AttackKnight : State<HeroKnighMechanics>
{
    private static AttackKnight instance;
    private AttackKnight()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static AttackKnight Instance
    {
        get
        {
            if (instance == null)
                new AttackKnight();
            return instance;
        }
    }
    public override void EnterState(HeroKnighMechanics owner)
    {
    }
    public override void ExitState(HeroKnighMechanics owner)
    {
        //owner.audioSettingsPanel.SetActive(false);
    }
    public override void UpdateState(HeroKnighMechanics owner)
    {
        if (owner.attacking) return;

        if (owner.inRange.inRange)
            Attack(owner);
        else
            owner.Idle();
    }
    private void Attack(HeroKnighMechanics owner)
    {
        owner.attacking = true;
        switch (owner.nextAttack)
        {
            case 1:
                owner.StartCoroutine(Attack1(owner));
                owner.nextAttack = 2;
                break;
            case 2:
                owner.StartCoroutine(Cooldown(owner));
                owner.nextAttack = 3;
                break;
            case 3:
                owner.StartCoroutine(Attack2(owner));
                owner.nextAttack = 4;
                break;
            case 4:
                owner.StartCoroutine(Cooldown(owner));
                owner.nextAttack = 1;
                break;
        }
    }
    private void Attack1Hitbox(HeroKnighMechanics owner)
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(owner.attack1HitBoxBC2D.bounds.center, owner.attack1HitBoxBC2D.bounds.size, owner.transform.localScale.x > 0 ? 45f : -45f, Vector2.zero, 0, owner.platformLayerMask);
        /*
        Color rayColor;
        rayColor  = raycastHit.collider != null ? Color.green : rayColor = Color.red;
        Debug.DrawRay(owner.attack1HitBoxBC2D.bounds.center + new Vector3(owner.attack1HitBoxBC2D.bounds.extents.x, 0), Vector2.down * (owner.attack1HitBoxBC2D.bounds.extents.y + 0), rayColor);
        Debug.DrawRay(owner.attack1HitBoxBC2D.bounds.center - new Vector3(owner.attack1HitBoxBC2D.bounds.extents.x, 0), Vector2.down * (owner.attack1HitBoxBC2D.bounds.extents.y + 0), rayColor);
        Debug.DrawRay(owner.attack1HitBoxBC2D.bounds.center - new Vector3(owner.attack1HitBoxBC2D.bounds.extents.x, owner.attack1HitBoxBC2D.bounds.extents.y + 0), Vector2.right * (owner.attack1HitBoxBC2D.bounds.extents.x * 2f), rayColor);
        */
        if (raycastHit.collider != null)
            owner.playerMechanics.DamagePlayer();
    }
    private void Attack2Hitbox(HeroKnighMechanics owner)
    {

        RaycastHit2D raycastHit = Physics2D.BoxCast(owner.attack2HitBoxBC2D.bounds.center, owner.attack2HitBoxBC2D.bounds.size, 0, Vector2.zero, 0, owner.platformLayerMask);
        /*
        Color rayColor;

        rayColor = raycastHit.collider != null ? Color.green : rayColor = Color.red;
        Debug.DrawRay(owner.attack2HitBoxBC2D.bounds.center + new Vector3(owner.attack2HitBoxBC2D.bounds.extents.x, 0), Vector2.down * (owner.attack2HitBoxBC2D.bounds.extents.y + 0), rayColor);
        Debug.DrawRay(owner.attack2HitBoxBC2D.bounds.center - new Vector3(owner.attack2HitBoxBC2D.bounds.extents.x, 0), Vector2.down * (owner.attack2HitBoxBC2D.bounds.extents.y + 0), rayColor);
        Debug.DrawRay(owner.attack2HitBoxBC2D.bounds.center - new Vector3(owner.attack2HitBoxBC2D.bounds.extents.x, owner.attack2HitBoxBC2D.bounds.extents.y + 0), Vector2.right * (owner.attack2HitBoxBC2D.bounds.extents.x * 2f), rayColor);
        return raycastHit.collider != null;
        */
        if (raycastHit.collider != null)
            owner.playerMechanics.DamagePlayer();
    }
    private IEnumerator Attack1(HeroKnighMechanics owner)
    {
        owner.animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.3f);
        //owner.attack1HitBox.SetActive(true);
        //yield return owner.StartCoroutine(owner.WaitForFrames(1));
        //owner.attack1HitBox.SetActive(false);
        Attack1Hitbox(owner);
        yield return new WaitForSeconds(0.4f);
        yield return owner.StartCoroutine(JumpBack(owner));
        owner.attacking = false;
    }
    private IEnumerator Attack2(HeroKnighMechanics owner)
    {
        owner.animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(0.3f);
        //owner.attack2HitBox.SetActive(true);
        //yield return owner.StartCoroutine(owner.WaitForFrames(1));
        //owner.attack2HitBox.SetActive(false);
        Attack2Hitbox(owner);
        yield return new WaitForSeconds(0.4f);
        yield return owner.StartCoroutine(JumpBack(owner));
        owner.attacking = false;
    }
    private IEnumerator Cooldown(HeroKnighMechanics owner)
    {
        yield return new WaitForSeconds(1.5f);
        Face(owner);
        owner.attacking = false;
    }
    private void Face(HeroKnighMechanics owner)
    {
        float x = (owner.target.position - owner.transform.position).normalized.x;
        x = x > 0 ? 1 : -1;
        owner.transform.localScale = new Vector3(x, owner.transform.localScale.y, owner.transform.localScale.z);
    }
    private IEnumerator JumpBack(HeroKnighMechanics owner)
    {
        owner.animator.SetTrigger("Jump");
        float x = (owner.target.position - owner.transform.position).normalized.x;
        x = x < 0 ? 1 : -1;
        owner.rb2D.AddForce(new Vector2(x * 2f, 1));
        yield return new WaitForSeconds(0.3f);
        owner.rb2D.velocity = Vector2.zero;

    }
}
public class SeekKnight : State<HeroKnighMechanics>
{
    private static SeekKnight instance;
    private SeekKnight()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static SeekKnight Instance
    {
        get
        {
            if (instance == null)
                new SeekKnight();
            return instance;
        }
    }

    public override void EnterState(HeroKnighMechanics owner)
    {
        //owner.audioSettingsPanel.SetActive(true);
    }
    public override void ExitState(HeroKnighMechanics owner)
    {
        //owner.audioSettingsPanel.SetActive(false);
    }
    public override void UpdateState(HeroKnighMechanics owner)
    {
        owner.rb2D.velocity = Vector2.up * owner.rb2D.velocity.y;
        if (owner.inRange.inRange)
        {
            Still(owner);
            owner.Idle();
        }
        else
            Seek(owner);
    }
    private void Seek(HeroKnighMechanics owner)
    {
        float x = (owner.target.position - owner.transform.position).normalized.x;
        /*Vector3 heading = owner.target.position - owner.transform.position;
        Vector3 direction = heading / heading.magnitude; // This is now the normalized direction.
        RunTowards(direction.x, owner);
        */
        RunTowards(x, owner);
    }
    private void RunTowards(float x, HeroKnighMechanics owner)
    {
        x = x > 0 ? 1 : -1;
        owner.rb2D.velocity = new Vector2(x * owner.moveSpeed, owner.rb2D.velocity.y);
        owner.animator.SetBool("Run", true);
        //Flip(x < 0, owner);
        owner.transform.localScale = new Vector3( x , owner.transform.localScale.y, owner.transform.localScale.z);
    }
    /*
    private void Flip(bool left, HeroKnighMechanics owner)
    {
        owner.spriteRenderer.flipX = left;
        if (left)
        {
            float x = owner.attack1HitBox.offset.x < 0 ? owner.attack1HitBox.offset.x : owner.attack1HitBox.offset.x * -1;
            owner.attack1HitBox.offset = new Vector2(x, owner.attack1HitBox.offset.y);
        }
        else
        {
            float x = owner.attack1HitBox.offset.x > 0 ? owner.attack1HitBox.offset.x : owner.attack1HitBox.offset.x * -1;
            owner.attack1HitBox.offset = new Vector2(x, owner.attack1HitBox.offset.y);
        }
    }
    */
    private void Still(HeroKnighMechanics owner)
    {
        owner.animator.SetBool("Run", false);
    }
}