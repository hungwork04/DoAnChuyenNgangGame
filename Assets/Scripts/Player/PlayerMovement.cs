using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool isTeleportCooldown = false;

    public float moveSpeed = 5f;    // Tốc độ di chuyển
    public float jumpForce = 8f;   // Lực nhảy
    public float startmoveSpeed;
    public float startjumpForce;


    public Rigidbody2D rb;
    public ImpactOnPlayer impactOnPlayer;
    public playerAvatar playerAvatar;
    public Animator ani;

    public bool isGrounded;
    public bool facingRight = true;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool isDashing = false;
    public float originMass;
    private Vector2 movementInput;
    public PlayerAby playerAby;
    public SpriteRenderer PlayerColor;
    public Color originalColor;
    private void Awake()
    {

        if (playerAvatar == null) {
            playerAvatar = this.transform.parent.GetComponentInChildren<playerAvatar>();
        }
        if (rb == null)
        {
            rb = transform.parent.GetComponent<Rigidbody2D>();
        }
        if (impactOnPlayer == null)
            impactOnPlayer = this.transform.parent.GetComponentInChildren<ImpactOnPlayer>();
        if (playerAby == null) playerAby = this.transform.parent.GetComponentInChildren<PlayerAby>();
        if (playerAvatar == null) return;
        ani = this.playerAvatar.avatarList[playerAvatar.index].GetComponent<Animator>();
    }
    private void Start()
    {
        PlayerColor = playerAvatar.GetComponentInChildren<SpriteRenderer>();
        if (PlayerColor != null)
        {
            originalColor = PlayerColor.material.color; // Lưu lại màu gốc
        }
        originMass = rb.mass;
        startmoveSpeed = moveSpeed;
        startjumpForce = jumpForce;
    }

    private void Update()
    {
        ani.SetBool("isHitted", impactOnPlayer.isKnockback);

        CheckJumpAndFallAni();
    }

    public void BeKnockBack()
    {
        if (impactOnPlayer.isKnockback)
        {
            //Debug.Log("khoa di chuyen");
            impactOnPlayer.blockMove( impactOnPlayer.isKnockback);

        }
        else
        {
            impactOnPlayer.blockMove( impactOnPlayer.isKnockback);
        }
    }
    private void FixedUpdate()
    {
        BeKnockBack();
        BeFrozen();
        Move();
    }
    public void BeFrozen()
    {
        if (impactOnPlayer.isStunned)
        {
            //impactOnPlayer.Stun();
            impactOnPlayer.blockMove(impactOnPlayer.isStunned);
            rb.mass = 10f;
            ChangeToColor(Color.Lerp(Color.blue, Color.white, 0.5f));
            //Debug.Log("3");
            if (impactOnPlayer.isKnockback)
            {
                //impactOnPlayer.Unstun();
                impactOnPlayer.isStunned = false;
                impactOnPlayer.blockMove( impactOnPlayer.isStunned);
                rb.mass = originMass;
                ResetColor();
            }
        }
        else
        {
            //impactOnPlayer.Unstun();
            impactOnPlayer.blockMove(impactOnPlayer.isStunned);
            rb.mass = originMass;
            ResetColor();
        }
    }
    public void ChangeToColor(Color newColor)
    {
        if (PlayerColor != null)
        {
            PlayerColor.material.color = newColor; // Thay đổi màu sắc
        }
    }
    public void ResetColor()
    {
        if (PlayerColor != null)
        {
            PlayerColor.material.color = originalColor; // Đặt lại màu gốc
        }
    }

    // Hàm nhận đầu vào di chuyển
    public void SetMovementInput(Vector2 input)
    {
        movementInput = input;
    }

    // Hàm di chuyển nhân vật
    private void Move()
    {
        if (!impactOnPlayer.isKnockback && isDashing==false&&impactOnPlayer.isUsingSkillCanMove==false)
        {
            rb.linearVelocity = new Vector2(movementInput.x * moveSpeed, rb.linearVelocity.y);
            if (movementInput.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (movementInput.x < 0 && facingRight)
            {
                Flip();
            }

        }
        ani.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
    }

    public bool isCanDoubleJump = false; // Biến để kiểm tra khả năng double jump
    public bool hasDoubleJumped = false; // Biến để kiểm tra đã nhảy lần thứ hai chưa
    public void Jump()
    {
        if (impactOnPlayer.isKnockback) return;

        if (isGrounded)
        {
            // Nhảy bình thường khi ở mặt đất
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            hasDoubleJumped = false;
            isCanDoubleJump = true;
            // Reset wall jump khi chạm đất
            impactOnPlayer.hasWallJumped = false;
        }
        else if (impactOnPlayer.isTouchingWall && !impactOnPlayer.hasWallJumped)
        {
            // Wall jump - chỉ cho phép 1 lần
            rb.linearVelocity = new Vector2(-Mathf.Sign(playerAvatar.transform.localScale.x) * 8, jumpForce);
            impactOnPlayer.wallContactTime = 0f;
            impactOnPlayer.hasWallJumped = true; // Đánh dấu đã wall jump
            
            // Reset double jump khi wall jump
            hasDoubleJumped = false;
            isCanDoubleJump = true;
            
            Debug.Log("Wall jump");
        }
        else if (isCanDoubleJump && !hasDoubleJumped && !isGrounded)
        {
            // Double jump bình thường
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            hasDoubleJumped = true;
            isCanDoubleJump = false;
        }
    }

    private void CheckJumpAndFallAni()
    {
        if (!isGrounded && rb.linearVelocity.y > 0)
        {
            isJumping = true;
            isFalling = false;
        }
        else if (!isGrounded && rb.linearVelocity.y < 0)
        {
            isJumping = false;
            isFalling = true;
        }
        else if (isGrounded)
        {
            isJumping = false;
            isFalling = false;
        }

        ani.SetBool("isJumping", isJumping);
        ani.SetBool("isFalling", isFalling);
    }
    
    // Lật nhân vật
    private void Flip()
    {
        Transform Ava = playerAvatar.transform;
        facingRight = !facingRight;
        Vector3 currentScale = Ava.localScale;
        currentScale.x *= -1;
        Ava.localScale = currentScale;
        FlipHoldBombTransform();
    }
    public void FlipHoldBombTransform()
    {
        Vector3 newvec =playerAby.transform.localScale;
        newvec.x *= -1;
        playerAby.transform.localScale=newvec;
    }
    private void OnEnable()
    {
        if(EndGameCtrl.instance!=null)
        EndGameCtrl.instance.players.Add(this.transform.parent.gameObject);

    }
    private void OnDisable()
    {
        if (EndGameCtrl.instance != null)
            EndGameCtrl.instance.players.Remove(this.transform.parent.gameObject);
    }


    public void climbing()
    {
        impactOnPlayer.isClimbing = true;
        if (impactOnPlayer.canClimbing)
        {
            if(impactOnPlayer.isClimbing==false) return;
            rb.gravityScale=0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, impactOnPlayer.climbSpeed);
        }else 
            impactOnPlayer.isClimbing = false;
    }

}
