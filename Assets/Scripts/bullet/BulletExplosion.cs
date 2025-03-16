using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine("DesEff");
    }
    public IEnumerator DesEff()
    {
        yield return new WaitForSeconds(0.23f); // Thời gian chờ (nếu cần)
        Destroy(this.gameObject);
    }
}
