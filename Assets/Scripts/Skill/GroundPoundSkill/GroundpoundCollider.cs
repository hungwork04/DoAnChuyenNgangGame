using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundpoundCollider : MeleeColliderInteract
{
    public float dashtime = 0.5f;
    public playerAvatar ava;
    public Rigidbody2D rb;
    public PlayerMovement playerMovement;
    public bool isDashDown = false;
    protected Collider2D playerCol;
    public GameObject groundPoundEff;
    private void Start()
    {
        if (ava == null) ava = transform.parent.GetComponentInParent<playerAvatar>();
        rb = ava.GetComponentInParent<Rigidbody2D>();
        playerMovement = rb.gameObject.GetComponentInChildren<PlayerMovement>();
        playerCol= this.GetComponentInParent<ImpactOnPlayer>().gameObject.GetComponent<Collider2D>();
    }

    public IEnumerator Dash(float dashingPower)
    {
        if (playerMovement.isDashing) yield break;

        playerMovement.isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        //if (playerCol != null)
        //{
        //    playerCol.usedByEffector = true;
        //}
        this.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Add(1);
        rb.linearVelocity = new Vector2(0f, -dashingPower * 1.5f);
        
        while (!playerMovement.isGrounded && playerMovement.isDashing)
        {
            yield return null;
        }
        //playerCol.usedByEffector = false;
        StopDash();
    }
    public LayerMask groundLayer;
    private void StopDash()
    {
        Vector2 groundPoint = rb.transform.position; // Mặc định là vị trí nhân vật

        // Tìm vị trí chạm đất bằng Raycast
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, Vector2.down, 1.5f, groundLayer);
        if (hit.collider != null)
        {
            groundPoint = hit.point; // Lấy vị trí chính xác trên mặt đất
            Debug.Log("point");
        }
        Vector2 editGroundPoint = new Vector2(groundPoint.x, groundPoint.y +0.3f);
        var eff = Instantiate(groundPoundEff, editGroundPoint, Quaternion.identity);
        //Debug.Log("Dừng dásh");
        playerMovement.isDashing = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;
        this.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Remove(1);
        base.ActivateCollider(dashtime, 10); // Gọi base khi dash kết thúc
        StartCoroutine(desEffect(eff, dashtime+0.05f));
    }

    private IEnumerator desEffect(GameObject eff,float dashTime)
    {
        yield return new WaitForSeconds(dashTime);
        Destroy(eff);
    }
        
    public override void ActivateCollider(float duration, int skillDamage)
    {
        if (playerMovement != null)
        {
            Debug.Log("k null");
            if (playerMovement.isJumping || playerMovement.isFalling)
            {
                Debug.Log("đủ dk");
                StartCoroutine(Dash(10));
            }
        }
    }

    public override void processingKnockBack(Collider2D[] cols)
    {
        foreach (Collider2D col in cols)
        {
            Transform coltrans = col.transform;
            while (coltrans.parent != null && coltrans.GetComponent<Rigidbody2D>() == null)
            {
                coltrans = coltrans.parent;
            }

            Rigidbody2D rigid = coltrans.GetComponent<Rigidbody2D>();
            //Debug.Log( rigid.gameObject.name);

            if (rigid != null && rigid.gameObject != this.transform.parent.parent.parent.gameObject)
            {
                //resetBombWhileHandle(rigid);
                resetPlayerBombHandle(coltrans);
                var impact = coltrans.gameObject.GetComponentInChildren<ImpactOnPlayer>();// Trường hợp nếu tấn công player
                if (impact != null && impact != this.transform.parent.GetComponentInChildren<ImpactOnPlayer>())
                {
                    if (!impact.isKnockback)
                    {
                        impact.isKnockback = true;
                    }
                    //StartCoroutine(impact.GetComponent<ImpactOnPlayer>().ResetKnockbackAfterDelay(impact, 0.2f));
                }
                if (processedRigidbodies.Add(rigid.transform))
                {
                    Vector2 throwDirection = -transform.parent.parent.position+ rigid.transform.position;//player
                    rigid.linearVelocity = Vector2.zero;
                    rigid.AddForce(throwDirection.normalized *20* rigid.mass, ForceMode2D.Impulse);
                    processedRigidbodies.Add(rigid.transform);
                    Debug.Log("Force applied to: " + rigid.gameObject.name);
                    var health = rigid.transform.GetComponent<PlayerHealth>();
                    if (health)
                    {
                        health.takeDame(damage);
                    }
                }
            }
        }
    }
}
