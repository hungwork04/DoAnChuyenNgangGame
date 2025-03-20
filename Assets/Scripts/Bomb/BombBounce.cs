using Unity.VisualScripting;
using UnityEngine;

public class BombBounce : MonoBehaviour
{
    private Rigidbody2D rb;
    public BombController bombController;
    void Start()
    {
        if(rb==null) rb = GetComponent<Rigidbody2D>();
        if(bombController==null) bombController = GetComponentInChildren<BombController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")|| collision.gameObject.CompareTag("Grounded"))
        {
            Bounce(collision);
            if (bombController.bombType + 1 == 3 && bombController.isOn )
            {
                //Debug.Log("bomblife--");
                bombController.bombLife--;
            }
        }
    }
    
    Vector3 lastVelocity;
    private void Update()
    {
        lastVelocity = rb.velocity;
    }
    void Bounce(Collision2D collision)
    {
        // Lấy vector hướng va chạm của quả bom
        var Speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rb.velocity = direction * Speed* 0.7f;
    }
    private void OnEnable()
    {
        this.GetComponent<Transform>().localScale=new Vector3(0.8f,0.8f,0.8f);
    }
}