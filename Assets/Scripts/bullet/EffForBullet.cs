using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffForBullet : BombEffected
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("có va chạm");
        if ((targetLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("Triggered effectImpact");
            effectImpact(collision);
        }
    }

    public override void effectImpact(Collider2D collision)
    {
        float currentRadius = circleCollider.radius;
        //explodeEffect.enabled = true;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.parent.position, currentRadius, targetLayers);
        foreach (Collider2D col in cols)
        {
            Transform coltrans = col.transform;
            // tìm Object ông nội.... chứa Rigidbody2d
            while (coltrans.parent != null && coltrans.GetComponent<Rigidbody2D>() == null)
            {
                coltrans = coltrans.parent;
            }

            Rigidbody2D rigid = coltrans.GetComponent<Rigidbody2D>();

            if (rigid != null)
            {
                //resetBombWhileHandle(rigid);
                //resetPlayerBombHandle(coltrans);
                if (!processedRigidbodies.Contains(rigid))
                {
                    processedRigidbodies.Add(rigid);
                    //gây dame
                    var health = rigid.transform.GetComponent<PlayerHealth>();
                    //Debug.Log("sap tru mau defuau");
                    if (health)
                    {
                        health.takeDame(5);
                        var impact = coltrans.gameObject.GetComponentInChildren<ImpactOnPlayer>();
                        if (impact != null)
                        {
                            impact.startSlowed();
                        }
                        Debug.Log("trừ máu " + health.gameObject);
                    }
                }

            }
        }
    }
}
