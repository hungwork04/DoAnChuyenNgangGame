using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectType3 : BombEffected
{
    public PolygonCollider2D polygonCollider;
    public PointEffector2D explodeEffect;

    ContactFilter2D filter;
    private void Start()
    {
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        filter = new ContactFilter2D();
        filter.SetLayerMask(targetLayers);
    }

    protected  void Awake()
    {
        if (explodeEffect == null)
        {
            explodeEffect = GetComponent<PointEffector2D>();
            explodeEffect.enabled = false;
        }
    }

    protected override void OnEnable()
    {
        // Gọi OnEnable của lớp cha để kế thừa các chức năng
        base.OnEnable();
        if (explodeEffect != null)
        {
            explodeEffect.enabled = true;
        }
        // Thực hiện các chức năng riêng của lớp con
        StartCoroutine(ExploseBom3());
    }

    public IEnumerator ExploseBom3()
    {
        yield return new WaitForSeconds(0.06f);
        List<Collider2D> cols = new List<Collider2D>();
        // Kiểm tra và thêm các collider trùng lặp vào danh sách
        polygonCollider.Overlap(filter, cols);
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
                if (processedRigidbodies.Add(rigid))
                {
                    processedRigidbodies.Add(rigid);
                    //Debug.Log("Force applied to: " + rigid.gameObject.name);
                    //gây dame
                    var health = rigid.transform.GetComponent<PlayerHealth>();
                    if (health)
                    {
                        health.takeDame(25);
                    }
                }
            }
        }
    }
}


