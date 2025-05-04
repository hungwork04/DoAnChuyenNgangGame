using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectType2 : BombEffected
{
    protected override void OnEnable()
    {
        // Gọi OnEnable của lớp cha để kế thừa các chức năng
        base.OnEnable();

        // Thực hiện các chức năng riêng của lớp con
        StartCoroutine(ExpandRadiusAndProcessLogic());
    }
    HashSet<Collider2D> processedColliders = new HashSet<Collider2D>(); // Để lọc các đối tượng đã quét
    //bomb choáng
    public IEnumerator ExpandRadiusAndProcessLogic()//tính toán cho bán kính quét dần dần
    {
        float currentRadius = 0f;              // Bán kính ban đầu
        float maxRadius = 1.3f;                 // Bán kính tối đa
        float timeToExpand = 0.25f;              // Thời gian để bán kính đạt tối đa
        float expansionSpeed = maxRadius / timeToExpand; // Tính toán tốc độ mở rộng

        while (currentRadius < maxRadius)
        {
            // Tăng dần bán kính
            currentRadius += expansionSpeed * Time.deltaTime;

            // Quét tất cả Collider trong bán kính hiện tại
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, currentRadius, targetLayers);

            // Duyệt qua các Collider và lọc những đối tượng chưa xử lý
            foreach (Collider2D collider in cols)
            {
                if (!processedColliders.Contains(collider)) // Chỉ xử lý nếu chưa được thêm vào
                {
                    processedColliders.Add(collider);
                }
            }

            // Tạm dừng đến frame tiếp theo để bán kính tiếp tục tăng
            yield return null;
        }

        //Debug.Log("Radius expansion finished. Starting logic processing.");

        foreach (Collider2D col in processedColliders)
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
                        health.takeDame(10);
                    }
                }

            }
        }
    }


    private void OnDrawGizmos()
    {
        float currentRadius = 0f;
        float maxRadius = 1f;
        float expansionSpeed = maxRadius / 0.2f; // Tốc độ mở rộng
        float timeElapsed = 0f;

        // Gizmos hiển thị hình tròn với bán kính tăng dần
        while (currentRadius < maxRadius)
        {
            timeElapsed += Time.deltaTime;
            currentRadius = expansionSpeed * timeElapsed;

            Gizmos.color = Color.red; // Màu đỏ cho hình tròn
            Vector3 previousPoint = transform.position + new Vector3(currentRadius, 0, 0);
            for (int i = 1; i <= 50; i++)
            {
                float angle = i * Mathf.PI * 2 / 50;
                float x = Mathf.Cos(angle) * currentRadius;
                float y = Mathf.Sin(angle) * currentRadius;

                Vector3 currentPoint = transform.position + new Vector3(x, y, 0);
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
        }
    }

    protected override void OnDisable()
    {
        processedColliders.Clear();
        processedRigidbodies.Clear();
    }
    public override void resetPlayerBombHandle(Transform coltrans)
    {
        var impact = coltrans.gameObject.GetComponentInChildren<ImpactOnPlayer>();
        var aby = coltrans.gameObject.GetComponentInChildren<PlayerAby>();
        if (impact != null)
        {
            if (!impact.isStunned)
            {
                impact.StopAllCoroutines();
                impact.isStunned = true;
                Debug.Log("reset bomb " + impact.gameObject);
                if (aby.Object != null)
                {
                    var rigid = aby.Object.GetComponentInParent<Rigidbody2D>();
                    if (rigid == null) return;
                    rigid.bodyType = RigidbodyType2D.Dynamic;
                    rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rigid.transform.parent = BombSpawner.instance.PoolingObj.transform;
                    rigid.gameObject.GetComponent<Collider2D>().isTrigger = false;
                    Debug.Log("reset bomb" + rigid.gameObject.name);

                }
                aby.Object = null;
                aby.isholdBomb = false;
            }
        }
    }

}
