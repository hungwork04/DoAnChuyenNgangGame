using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundpoundCollider : MeleeColliderInteract
{
    public float dashtime = 0.2f;
    public playerAvatar ava;
    public Rigidbody2D rb;
    public PlayerMovement playerMovement;
    public bool isDashDown = false;
    protected Collider2D playerCol;
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
        if (playerCol != null)
        {
            playerCol.usedByEffector = true;
        }
        rb.velocity = new Vector2(0f, -dashingPower * 1.5f);
        
        while (!playerMovement.isGrounded && playerMovement.isDashing)
        {
            yield return null;
        }
        playerCol.usedByEffector = false;
        StopDash();
    }

    private void StopDash()
    {
        Debug.Log("Dừng dásh");
        playerMovement.isDashing = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1;
        base.ActivateCollider(dashtime, 10); // Gọi base khi dash kết thúc
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
                    rigid.velocity = Vector2.zero;
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
