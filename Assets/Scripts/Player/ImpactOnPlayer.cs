using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImpactOnPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D collider2d;
    [Header("put player skill manage in")]
    public PlayerAttribute whichPlayer;
    public float slideSpeed = 2f;// Tốc độ trượt khi va vào tường

    public bool isSliding = false;
    public bool isKnockback=false;
    public bool isStunned = false;
    public bool isUsingSkillCanMove = false;
    public bool isUseSkill = false;
    public bool isClimbing = false;
    public bool isSlowed = false;
    public bool isTouchingWall = false;
    public float wallContactTime = 0f;
    public bool hasWallJumped = false;

    public List<int> SkillInUse = new List<int>();

    public Coroutine currSlowedCorou;
    public PlayerMovement playerMovement;
    public Vector2 oldOffset;
    public Vector2 oldSize;
    private void OnEnable()
    {
        this.GetComponent<BoxCollider2D>().offset = oldOffset;
        this.GetComponent<BoxCollider2D>().size = oldSize;
        
    }
    private void Awake()
    {
        if (playerMovement == null) playerMovement = transform.parent.parent.GetComponentInChildren<PlayerMovement>();
        if (collider2d==null) collider2d = GetComponent<Collider2D>();
        if (rb == null) rb = transform.parent.parent.GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        startgravityScale = rb.gravityScale;
    }
    private void Update()
    {
        if (isKnockback)
        {
            StartCoroutine(ResetKnockbackAfterDelay(this, 0.45f));//find other ways!!
        }
        if (isStunned)
        {
            StartCoroutine(ResetStunnedAfterDelay(this, 5f));
        }
        if (!canClimbing)
        {
            rb.gravityScale = startgravityScale;
        }

        if (isTouchingWall && wallContactTime > 0.5f && !playerMovement.isGrounded )
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2);
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }if((!isKnockback||!isStunned)&&(playerMovement.moveSpeed == 0f||playerMovement.jumpForce == 0f))
        {
            playerMovement.moveSpeed = playerMovement.startmoveSpeed;
            playerMovement.jumpForce = playerMovement.startjumpForce;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        // Kiểm tra va chạm với tường
        if (collision.gameObject.CompareTag("Wall") && !playerMovement.isGrounded)
        {
            wallContactTime += Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !playerMovement.isGrounded)
        {
            isTouchingWall = true;
            // Reset biến hasWallJumped khi chạm tường mới
            hasWallJumped = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Không reset double jump ở đây nữa
            isTouchingWall = false;
            wallContactTime = 0;
        }
    }
    public IEnumerator beSlowed()
    {

        playerMovement.PlayerColor.color= Color.green;
        playerMovement.moveSpeed -=0.4f;
        isSlowed = true;
        yield return new WaitForSeconds(10f);
        Debug.Log("hết time");
        playerMovement.moveSpeed = playerMovement.startmoveSpeed;
        isSlowed = false;
        playerMovement.PlayerColor.color = playerMovement.originalColor;

    }
    public void refreshYourSelf()
    {
        StopAllCoroutines();
        playerMovement.moveSpeed = playerMovement.startmoveSpeed;// trường hợp captain
        playerMovement.jumpForce = playerMovement.startjumpForce;
        playerMovement.PlayerColor.color = playerMovement.originalColor;
        isSlowed = false;
        isKnockback = false;
        isStunned = false;
    }
    public void startSlowed()
    {
        if (currSlowedCorou != null)
        {
            StopCoroutine(currSlowedCorou);
            //Debug.Log("xóa cor");
            currSlowedCorou = null;
        }
        //Debug.Log(currSlowedCorou?.ToString());
        currSlowedCorou = StartCoroutine(beSlowed());
    }
    public IEnumerator ResetKnockbackAfterDelay(ImpactOnPlayer impact, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (impact != null)
        {
            impact.isKnockback = false;
            playerMovement.moveSpeed = playerMovement.startmoveSpeed;
            playerMovement.jumpForce = playerMovement.startjumpForce;
        }
    }
    public IEnumerator ResetStunnedAfterDelay(ImpactOnPlayer impact, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (impact != null)
        {
            impact.isStunned = false;
            playerMovement.moveSpeed = playerMovement.startmoveSpeed;
            playerMovement.jumpForce = playerMovement.startjumpForce;
        }
    }


    public void blockMove(bool eff)
    {
        //float m,float j,
        if (eff)
        {
            //Debug.Log("lockmove");
            playerMovement.moveSpeed = 0f;
            playerMovement.jumpForce = 0f;
        }
    }


    public float climbSpeed = 5f; // Tốc độ leo thang
    public float startgravityScale;
    public bool canClimbing = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            canClimbing = true;
             // Vô hiệu hóa trọng lực khi leo
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            canClimbing = false;
            isClimbing = false ;
        }
    }
}
