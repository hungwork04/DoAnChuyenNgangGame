using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffected : MonoBehaviour
{
    //public PointEffector2D explodeEffect;
    public CircleCollider2D circleCollider;

    //public BombSpawner bombSpawner;
    public LayerMask targetLayers;

    //protected virtual void Awake()
    //{
    //    if (bombSpawner == null) bombSpawner =FindAnyObjectByType<BombSpawner>();
    //}
    protected HashSet<Rigidbody2D> processedRigidbodies = new HashSet<Rigidbody2D>();
    public virtual void effectImpact(Collider2D collision)
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
                resetPlayerBombHandle(coltrans);
                if (!processedRigidbodies.Contains(rigid))
                {
                    processedRigidbodies.Add(rigid);      
                    //gây dame
                    var health = rigid.transform.GetComponent<PlayerHealth>();
                    //Debug.Log("sap tru mau defuau");
                    if (health)
                    {
                        health.takeDame(15);
                        Debug.Log("trừ máu "+ health.gameObject);
                    }
                }

            }
        }
    }
 
    //public void resetBombWhileHandle(Rigidbody2D rigid)
    //{
    //    if (rigid.gameObject.layer == LayerMask.NameToLayer("Passthrough"))
    //    {
    //        rigid.bodyType = RigidbodyType2D.Dynamic;
    //        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    //        rigid.transform.parent = bombSpawner.GetComponent<BombSpawner>().PoolingObj;
    //        rigid.gameObject.GetComponent<Collider2D>().isTrigger = false;
    //        Debug.Log("reset bomb tay "+rigid.gameObject.name);
    //    }
    //}

    public virtual void resetPlayerBombHandle(Transform coltrans)
    {
        var impact = coltrans.gameObject.GetComponentInChildren<ImpactOnPlayer>();
        var aby = coltrans.gameObject.GetComponentInChildren<PlayerAby>();
        if (impact != null)
        {
            if (!impact.isKnockback)
            {
                impact.StopAllCoroutines();
                impact.isKnockback = true;
                if (aby != null)
                {
                    if (aby.Object != null)
                    {
                        var rigid = aby.Object.GetComponentInParent<Rigidbody2D>();
                        if (rigid == null) return;
                        rigid.bodyType = RigidbodyType2D.Dynamic;
                        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        rigid.transform.parent = BombSpawner.instance.PoolingObj.transform;
                        rigid.gameObject.GetComponent<Collider2D>().isTrigger = false;
                        Debug.Log("reset bomb"+ rigid.gameObject.name);
                    }
                    aby.Object = null;
                    aby.isholdBomb = false;
                }
            }
            //StartCoroutine(impact.GetComponent<ImpactOnPlayer>().ResetKnockbackAfterDelay(impact, 0.2f));
        }
    }

    protected virtual void OnEnable()
    {
        Invoke("destroyBom", 0.35f);
    }
    protected virtual void OnDisable()
    {
        processedRigidbodies.Clear();
    }
    protected void destroyBom()
    {
        Destroy(transform.parent.gameObject);
    }

}
