using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGrowUpSkill : MonoBehaviour
{
    public float GrowUpRemainTime = 5f;
    public Vector3 originalScale;
    public Vector3 targetScale;
    public float growSpeed = 1f;
    public bool isProcessing =false;
    public Transform parentObj;
    public Collider2D interactCol2d;
    private void Start()
    {
        interactCol2d=GetComponent<Collider2D>();
        if (interactCol2d != null) {
            interactCol2d.enabled = false;
        }
        originalScale = parentObj.localScale;
        targetScale = originalScale ;
    }
    public IEnumerator growUp()
    {
        Debug.Log("Chạy vào đây1");
        isProcessing = true;
        interactCol2d.enabled=true;
        for (int i = 0; i < 3; i++)
        {
            targetScale  *= 1.25f;
            while (originalScale.x < targetScale.x)
            {
                parentObj.localScale = Vector3.Lerp(originalScale, targetScale, growSpeed);
                if (Mathf.Abs(targetScale.x - parentObj.localScale.x) < 0.01f)
                {
                    parentObj.localScale = targetScale;
                    break;
                }
                yield return null;
            }
            Debug.Log(parentObj.localScale);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(GrowUpRemainTime);
        //resetPlayerBombHandle(parentObj);
        while (originalScale.x < targetScale.x)
        {
            parentObj.localScale = Vector3.Lerp(targetScale, originalScale, growSpeed);
            if (Mathf.Abs(originalScale.x - parentObj.localScale.x) < 0.01f)
            {
                parentObj.localScale = originalScale;
                break;
            }
            yield return null;
        }
        Debug.Log("Chạy vào đây4");
        targetScale = originalScale;
        isProcessing = false;
        interactCol2d.enabled = false;

    }
    public void resetPlayerBombHandle(Transform coltrans)
    {
        var aby = coltrans.gameObject.GetComponentInChildren<PlayerAby>();
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
}
