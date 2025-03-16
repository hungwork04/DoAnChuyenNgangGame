using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAtkforBigGuy : MeleeColliderInteract
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if ((layer.value & (1 << collision.gameObject.layer)) != 0)
        {
            //colPos.position= this.transform.position;
            if (this.transform.GetComponent<BoxCollider2D>() != null)
            {
                Collider2D[] cols = Physics2D.OverlapBoxAll(this.transform.position, this.transform.GetComponent<BoxCollider2D>().size, 0f);
                processingKnockBack(cols);
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

            if (rigid != null && rigid.gameObject != this.transform.parent.parent.parent.gameObject)
            {
                Debug.Log(rigid.gameObject);
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
                    processedRigidbodies.Add(rigid.transform);
                    //Debug.Log("Force applied to: " + rigid.gameObject.name);
                    var health = rigid.transform.GetComponent<PlayerHealth>();
                    if (health)
                    {
                        health.takeDame(damage);
                        Debug.Log(health.gameObject);
                    }
                }
            }
        }
    }
}
