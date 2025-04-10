using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlowColliderInteract : MonoBehaviour
{
    public Collider2D BlowCollider; // Collider cho kỹ năng
    //public PointEffector2D knockbackeEffect;
    public LayerMask layer;
    private int damage;

    private void Awake()
    {
        if (BlowCollider == null) BlowCollider = GetComponent<Collider2D>();
        //knockbackeEffect=GetComponent<PointEffector2D>();
    }
    private void Start()
    {
        if (BlowCollider != null)
        {
            BlowCollider.enabled = false; // Tắt Collider khi bắt đầu
            
        }
    }

    public void ActivateCollider(float duration, int skillDamage)
    {
        if (BlowCollider != null)
        {
            StartCoroutine(delaysd( duration,skillDamage, 0.2f));
            BlowCollider.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Add(0);
        }
    }
    private void DeactivateCollider()
    {
        if (BlowCollider != null)
        {
            processedRigidbodies.Clear();
            BlowCollider.GetComponentInParent<ImpactOnPlayer>().isUsingSkillCanMove = false;
            BlowCollider.enabled = false; // Tắt Collider
            BlowCollider.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Remove(0);
        }
    }
    HashSet<Transform> processedRigidbodies = new HashSet<Transform>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.layer==LayerMask.NameToLayer("Passthrough"))//passthrou
        //{
            Vector2 boxsize = BlowCollider.bounds.size;
            Vector2 vect = BlowCollider.bounds.center;
            Collider2D[] cols = Physics2D.OverlapBoxAll(vect, boxsize, layer);
        //Debug.LogWarning("trigger");
        foreach (Collider2D col in cols)
            {

            if (processedRigidbodies.Add(col.transform))
                {
                BombController bomctr =col.GetComponentInChildren<BombController>();
                    StartCoroutine(turnOffbomb(bomctr,0.45f));
                    processedRigidbodies.Add(col.transform);
                    //Debug.Log("Force applied to: " + rigid.gameObject.name);
                }
                
            }
        //}
    }

    public IEnumerator delaysd(float duration, int skillDamage, float tim) { 
        yield return new WaitForSeconds(tim);
        damage = skillDamage;
        BlowCollider.enabled = true; // Bật Collider
        BlowCollider.GetComponentInParent<ImpactOnPlayer>().isUsingSkillCanMove = true;

        // Tắt Collider sau thời gian kích hoạt
        Invoke(nameof(DeactivateCollider), duration);


    }
    public IEnumerator turnOffbomb(BombController bomctr,float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        if (bomctr != null)
        {
            if (bomctr.countdownCoroutine != null)
            {
                bomctr.StopCoroutine(bomctr.countdownCoroutine);
                bomctr.isOn = false;
                bomctr.bombLife = 3;
                bomctr.currentTime = 2;
            }

        }
    }
    private void OnDrawGizmos()
    {
        if (BlowCollider == null) return;

        // Lấy kích thước từ Collider
        Vector2 boxSize = BlowCollider.bounds.size;

        Gizmos.DrawWireCube(BlowCollider.bounds.center, boxSize); // Đường viền
    }
}
