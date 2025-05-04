using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class waterBullet : MonoBehaviour
{
    //public LayerMask layer;
    public GameObject bulletExplose;
    public bool isSpawn=false;
    private void OnEnable()
    {
        isSpawn =false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra va chạm với Grounded, Wall hoặc LayerMask
        if (collision.gameObject.CompareTag("Grounded") ||
            collision.gameObject.CompareTag("Wall") ||
           collision.gameObject.CompareTag("PLAYER"))//sai
        {
            if(isSpawn==true) return;
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 lastPoint = new Vector3(contactPoint.x, this.transform.position.y, contactPoint.z);
            //Debug.Log("Điểm va chạm: " + contactPoint);

            // Tạo hiệu ứng nổ tại điểm va chạm
            if (bulletExplose != null)
            {
                // Lấy localScale của viên đạn
                Vector3 bulletScale = this.transform.localScale;

                // Tạo hiệu ứng nổ tại điểm va chạm và sao chép localScale từ viên đạn
                GameObject eff = Instantiate(bulletExplose, lastPoint, Quaternion.identity);
                //Debug.Log("Điểm va chạm: " + collision);
                isSpawn = true;
                // Đảm bảo hiệu ứng nổ có cùng localScale với viên đạn
                eff.transform.localScale = bulletScale;
            }

            // Hủy viên đạn
            Destroy(this.gameObject);
        }
    }


}
