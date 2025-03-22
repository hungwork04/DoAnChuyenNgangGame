using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBullet : MonoBehaviour
{
    public float shootingSpeed = 1f;
    public GameObject bullet;
    public Transform shootPos;
    public Transform PlayerVec;
    public int bulletNum = 0;
    public void shooting()
    {
        if(bulletNum<=0) return;
        GameObject bul = Instantiate(bullet, shootPos.position, shootPos.rotation);
        Rigidbody2D rigid = bul.GetComponent<Rigidbody2D>();
        Vector2 shootDirection = PlayerVec.localScale.x > 0 ? Vector2.right : Vector2.left;
        if (shootDirection.x < 0)
        {
            Vector2 newvec = new Vector2(-bul.transform.localScale.x, bul.transform.localScale.y);
            bul.transform.localScale=newvec;
        }
        //Debug.Log(PlayerVec.localScale.x);
        // Áp dụng lực vào viên đạn theo hướng bắn
        rigid.linearVelocity = shootDirection * shootingSpeed;
        bulletNum--;
    }

}