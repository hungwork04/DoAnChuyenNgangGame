using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class teleToAtk : MonoBehaviour
{
    public GameObject owner;
    public LayerMask layer;
    public Transform pler;
    public playerAvatar plerAva;
    Transform target;
    public float offset;
    private void Awake()
    {
        if (owner != null)
        plerAva = owner.GetComponentInChildren<playerAvatar>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((layer.value & (1 << collision.gameObject.layer)) != 0)
        {
            // Lấy Transform của Rigidbody2D
            target = collision.transform.parent.parent.GetComponent<Rigidbody2D>().transform;

            if (target != null)
            {
                pler = target;
            }


        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((layer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(this.transform.position, this.transform.GetComponent<BoxCollider2D>().size, 0f,layer);//k có offset
            if (cols.Length > 0)
                pler = cols[0].transform;
            else pler = null;
        }
    }
    public void doTele()
    {

        if (pler != null)
        {
            Vector3 lastarget = new Vector3(plerAva.gameObject.transform.localScale.x < 0 ? target.position.x + offset : target.position.x - offset
            , target.position.y
            , target.position.z);
            //Debug.Log(lastarget.x - target.position.x);
            owner.transform.position = lastarget;
        }
            
    }
}
