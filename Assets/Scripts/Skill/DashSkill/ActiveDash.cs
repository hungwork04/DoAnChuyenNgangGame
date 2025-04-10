using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveDash : MonoBehaviour
{
    public float dashtime = 0.2f;
    public playerAvatar ava;
    public Rigidbody2D rb;
    public PlayerMovement playerMovement;
    private void Awake()
    {
        if(ava==null) ava = transform.parent.GetComponentInParent<playerAvatar>();
        rb = ava.GetComponentInParent<Rigidbody2D>();
        playerMovement =rb.gameObject.GetComponentInChildren<PlayerMovement>();
    }
    public IEnumerator Dash(float dashingPower)
    {
        playerMovement.isDashing = true;
        float originalGravity = rb.gravityScale;
        ava.GetComponentInChildren<ImpactOnPlayer>().SkillInUse.Add(0);
        // Tạm thời vô hiệu hóa trọng lực và ma sát
        rb.gravityScale = 0;
        rb.linearVelocity=Vector2.zero;
        // Lấy hướng Dash (trái hoặc phải)
        rb.linearVelocity = new Vector2(ava.transform.localScale.x * dashingPower*1.5f, 0f);

        // Debug thông tin vận tốc để kiểm tra

        // Giữ trạng thái Dash trong khoảng thời gian dashtime
        yield return new WaitForSeconds(dashtime*0.5f);
        // Khôi phục trọng lực và ma sát
        playerMovement.isDashing = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        ava.GetComponentInChildren<ImpactOnPlayer>().SkillInUse.Remove(0);

        //Debug.Log("Dash hoàn tất");
    }

}
