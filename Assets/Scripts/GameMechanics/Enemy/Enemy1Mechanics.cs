using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMode { seeker, attack }
public class Enemy1Mechanics : MonoBehaviour
{
    Transform enemy;
    [SerializeField]
    FloorDetector floorDetector;
    [SerializeField]
    JumpDetector jumpDetector;
    [SerializeField]
    JumpObstacleDetector jumpObstacleDetector;
    [SerializeField]
    FallDetector fallDetector;
    Rigidbody2D rb2D;
    [SerializeField]
    float moveSpeed;
    Enemy1AnimationController animationController;
    BoxCollider2D boxCollider2D;
    [SerializeField]
    LayerMask platformLayerMask;
    bool jumping = false;
    bool falling = false;
    bool hittingWall = false;
    public EnemyMode enemyMode;
    RaycastHit2D hitPlayer;
    [SerializeField]
    float bulletPower;
    [SerializeField]
    GameObject bulletPrefab;
    bool shootingInProgress = false;
    [SerializeField]
    LayerMask playerLayerMaks;
    bool dontUpdate = false;
    [SerializeField]
    int health = 2;
    private void Awake()
    {
        enemy = transform;
        rb2D = GetComponent<Rigidbody2D>();
        animationController = GetComponent<Enemy1AnimationController>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        enemyMode = EnemyMode.seeker;
    }
    private void Update()
    {
        if (dontUpdate) return;
        if (falling)
        {
            if (IsGrounded())
            {
                falling = false;
                Landed();
            }
        }
        else
        {
            if (!IsGrounded())
            {
                if (jumping)
                {
                    falling = false;
                    return;
                }
                else
                {
                    falling = true;
                    return;
                }
            }
            else
            {
                falling = false;
            }
        }
        
        if (hittingWall)
        {
            Stop();
            FlipDirection();
            hittingWall = false;
            return;
        }

        CheckForPlayer();

        if (!falling)
        {
            switch (enemyMode)
            {
                case EnemyMode.seeker:
                    animationController.StartRun();
                    SeekerMode();
                    break;
                case EnemyMode.attack:
                    Stop();
                    AttackMode();
                    break;
            }
        }
        else
        {
            enemyMode = EnemyMode.seeker;
        }

    }
    public void CheckForPlayer()
    {
        hitPlayer = Physics2D.Raycast(enemy.position, new Vector2(enemy.localScale.x, 0), 4f, playerLayerMaks);
        //Debug.DrawRay(enemy.position, new Vector2(enemy.localScale.x, 0) * 4f, Color.white);
        if (hitPlayer)
        {
            enemyMode = EnemyMode.attack;
            return;
        }
        enemyMode = EnemyMode.seeker;
    }
    public void SeekerMode()
    {
        if (floorDetector.NoFloor())
        {
            Stop();
            if (jumpDetector.NoFloor())
            {
                if (fallDetector.NoFloor())
                {
                    FlipDirection();
                }
                else
                {
                    GoStraight();
                }
                return;
            }
            else
            {
                if (jumpObstacleDetector.NoFloor())
                {
                    Jump();
                }
                else
                {
                    FlipDirection();
                }
                return;
            }
        }
        else
        {
            GoStraight();
        }
        if (!jumpDetector.NoFloor())
        {
            if (jumpObstacleDetector.NoFloor())
            {
                Jump();
            }
            else
            {
                FlipDirection();
            }
            return;
        }
    }
    public void AttackMode()
    {
        if (!shootingInProgress)
        {
            shootingInProgress = true;
            StartCoroutine(ShootTimer());
        }
    }

    public void BulletShoot(GameObject bullet)
    {
        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(enemy.localScale.x*bulletPower,0));
    }
    IEnumerator ShootTimer()
    {
        
        yield return new WaitForSeconds(1f);
        while (enemyMode == EnemyMode.attack)
        {

            BulletShoot(Instantiate(bulletPrefab, enemy.position, Quaternion.Euler(0, 0, 0)));
            yield return new WaitForSeconds(2f);
        }
        shootingInProgress = false;
    }
    public void FaceRight()
    {
        enemy.localScale = new Vector3(1, enemy.localScale.y, enemy.localScale.z);
    }
    public void FaceLeft()
    {
        enemy.localScale = new Vector3(-1, enemy.localScale.y, enemy.localScale.z);
    }

    public void GoRight()
    {
        rb2D.velocity = Vector2.right * moveSpeed;
    }
    public void GoLeft()
    {
        rb2D.velocity = Vector2.left * moveSpeed;
    }
    public void GoStraight()
    {
        switch (enemy.localScale.x)
        {
            case 1:
                GoRight();
                break;
            case -1:
                GoLeft();
                break;
        }
    }
    public void FlipDirection()
    {
        switch (enemy.localScale.x)
        {
            case -1:
                FaceRight();
                break;
            case 1:
                FaceLeft();
                break;
        }
    }
    public void Stop()
    {
        rb2D.velocity = Vector2.zero;
        animationController.StopRun();
    }
    IEnumerator DoActionsTest()
    {
        GoStraight();
        yield return new WaitForSeconds(2f);
        FaceLeft();
        GoStraight();
        yield return new WaitForSeconds(2f);
        Stop();
    }
    public void Jump()
    {
        Stop();
        switch (enemy.localScale.x)
        {
            case 1:
                rb2D.velocity = new Vector2(2f,6f);
                break;
            case -1:
                rb2D.velocity = new Vector2(-2f, 6f);
                break;
        }
        StartCoroutine(WaitJumpAnim());
    }
    IEnumerator WaitJumpAnim()
    {
        jumping = true;
        animationController.Jump();
        yield return new WaitForSeconds(0.3f);
        jumping = false;
    }
    public bool IsGrounded()
    {

        /*
        float extraHeightText = 0.05f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);
        Color rayColor;
        rayColor  = raycastHit.collider != null ? Color.green : rayColor = Color.red;
        Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2D.bounds.extents.x * 2f), rayColor);
        return raycastHit.collider != null;
        */
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.05f, platformLayerMask);
        return raycastHit.collider != null;
        
    }
    public void Landed()
    {
        animationController.Land();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        switch (collision.gameObject.layer)
        {
            case 8:
                hittingWall = true;
                break;
            case 9:
                //attack player
                break;
        }
    }
    public void ObjetcDie()
    {
        StartCoroutine(WaitForFullDeath());
    }
    public void Despawn()
    {
        StartCoroutine(WaitForDeath());
    }
    IEnumerator WaitForDeath()
    {
        dontUpdate = true;
        animationController.Die();
        yield return new WaitForSeconds(3.6f);
        dontUpdate = false;
        gameObject.SetActive(false);
    }
    IEnumerator WaitForFullDeath()
    {
        dontUpdate = true;
        animationController.Die();
        yield return new WaitForSeconds(3.6f);
        dontUpdate = false;
        Destroy(enemy.gameObject);
    }
    IEnumerator WaitForHit()
    {
        if (health > 1)
        {
            dontUpdate = true;
            health--;
            animationController.Hit();
            yield return new WaitForSeconds(0.3f);
            dontUpdate = false;
        }
        else
        {
            StartCoroutine(WaitForFullDeath());
        }
    }
    public void Damage()
    {
        StartCoroutine(WaitForHit());
    }
}
