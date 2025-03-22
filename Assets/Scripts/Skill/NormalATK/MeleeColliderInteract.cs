using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeColliderInteract : MonoBehaviour
{
    public Collider2D MeleeCollider; // Collider cho kỹ năng
    //public PointEffector2D knockbackeEffect;
    //public Transform colPos;
    public float radiusCol;
    public LayerMask layer;
    public int damage;

    protected void Awake()
    {
        if(MeleeCollider==null) MeleeCollider = GetComponent<Collider2D>();
        //knockbackeEffect=GetComponent<PointEffector2D>();
    }
    private void Start()
    {
        if (MeleeCollider != null)
        {
            MeleeCollider.enabled = false; // Tắt Collider khi bắt đầu
        }
    }

    public virtual void ActivateCollider(float duration, int skillDamage)
    {
        if (MeleeCollider != null)
        {
            damage = skillDamage;
            MeleeCollider.enabled = true; // Bật Collider
            MeleeCollider.GetComponentInParent<ImpactOnPlayer>().isUsingSkill = true;
            // Tắt Collider sau thời gian kích hoạt
            Invoke(nameof(DeactivateCollider), duration);

        }
    }

    protected virtual void DeactivateCollider()
    {
        if (MeleeCollider != null)
        {
            MeleeCollider.enabled = false; // Tắt Collider
            MeleeCollider.GetComponentInParent<ImpactOnPlayer>().isUsingSkill = false;
            processedRigidbodies.Clear();
        }
    }
    public HashSet<Transform> processedRigidbodies = new HashSet<Transform>();
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if ((layer.value & (1 << collision.gameObject.layer)) != 0)
        {
            //colPos.position= this.transform.position;
            Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, radiusCol, layer);
            processingKnockBack(cols);
        }
    }
    public virtual void processingKnockBack(Collider2D[] cols)
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
                //resetBombWhileHandle(rigid);
                resetPlayerBombHandle(coltrans);
                Debug.Log(coltrans.name);
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
                    Vector2 throwDirection = transform.parent.parent.localScale.normalized;//player
                    rigid.linearVelocity = Vector2.zero;
                    rigid.AddForce(throwDirection * 15 * rigid.mass, ForceMode2D.Impulse);
                    processedRigidbodies.Add(rigid.transform);
                    //Debug.Log("Force applied to: " + rigid.gameObject.name);
                    var health = rigid.transform.GetComponent<PlayerHealth>();
                    if (health)
                    {
                        health.takeDame(damage);
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
    //        //rigid.transform.parent = bombSpawner.GetComponent<BombSpawner>().PoolingObj;
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
                        //Debug.Log("reset bomb" + rigid.gameObject.name);
                    }
                    aby.Object = null;
                    aby.isholdBomb = false;
                }
            }
            //StartCoroutine(impact.GetComponent<ImpactOnPlayer>().ResetKnockbackAfterDelay(impact, 0.2f));
        }
    }
}
