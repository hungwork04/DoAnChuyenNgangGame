using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGrowUpSkill : MonoBehaviour
{
    //public float GrowUpRemainTime = 5f;
    public Vector3 originalScale;
    public Vector3 targetScale;
    public float growSpeed = 1f;
    public bool isProcessing =false;
    public Transform parentObj;
    public Collider2D interactCol2d;
    public GameObject AuraObj;
    private void Start()
    {
        interactCol2d=GetComponent<Collider2D>();
        if (interactCol2d != null) {
            interactCol2d.enabled = false;
        }
        originalScale = parentObj.localScale;
        targetScale = originalScale ;
        AuraObj.SetActive(false);
    }
    public IEnumerator growUp(float GrowUpRemainTime)
    {
        //Debug.Log("Chạy vào đây1");
        this.GetComponentInParent<ImpactOnPlayer>().isUseSkill = true;
        isProcessing = true;
        interactCol2d.enabled=true;
        this.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Add(1);
        AuraObj.SetActive(true);
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
            //Debug.Log(parentObj.localScale);
            yield return new WaitForSeconds(0.5f);
        }
        resetPlayerBombHandle(parentObj);
        StartCoroutine(DecreaseCountDownSkill(parentObj));
        yield return new WaitForSeconds(GrowUpRemainTime);
        //StartCoroutine(ignoreBombVsInteractColinSecond(parentObj));

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
        //Debug.Log("Chạy vào đây4");
        targetScale = originalScale;
        isProcessing = false;
        interactCol2d.enabled = false;
        this.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Remove(1);
        AuraObj.SetActive(false);

    }
    public  void beforeDisable()
    {
        StopAllCoroutines();
        while (originalScale.x < targetScale.x)
        {
            parentObj.localScale = Vector3.Lerp(targetScale, originalScale, growSpeed);
            if (Mathf.Abs(originalScale.x - parentObj.localScale.x) < 0.01f)
            {
                parentObj.localScale = originalScale;
                break;
            }
        }
        this.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Add(1);
        //Debug.Log("Chạy vào đây4");
        targetScale = originalScale;
        isProcessing = false;
        interactCol2d.enabled = false;
        AuraObj.SetActive(false);

    }

    //public IEnumerator ignoreBombVsInteractColinSecond(Transform coltrans)
    //{
    //    var aby = coltrans.gameObject.GetComponentInChildren<PlayerAby>();
    //    if (aby != null)
    //    {
    //        if (aby.Object != null)
    //        {
    //            Physics2D.IgnoreCollision(aby.Object.GetComponent<Collider2D>(), interactCol2d, true);
    //        }
    //    }
    //    yield return new WaitUntil(()=>aby.Object == null);
    //    Debug.Log("ignore");
    //}
    public void resetPlayerBombHandle(Transform coltrans)
    {
        var aby = coltrans.gameObject.GetComponentInChildren<PlayerAby>();
        if (aby != null)
        {
            if (aby.Object != null)
            {
                //Physics2D.IgnoreCollision(aby.Object.GetComponent<Collider2D>(), interactCol2d, true);

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

    public IEnumerator DecreaseCountDownSkill(Transform colTrans)
    {
        if (colTrans == null) yield break; // Kiểm tra null tránh lỗi

        var chaSkill = colTrans.GetComponentInChildren<CharacterSkillManager>();
        Debug.Log("Giamr hooi chiue");
        if (chaSkill == null || chaSkill.cooldownTimers == null || chaSkill.cooldownTimers.Length == 0)
            yield break; // Thoát nếu không có cooldownTimers

        float startCountDown = chaSkill.cooldownTimers[0];
        chaSkill.cooldownTimers[0] = 0.5f;
        chaSkill.currCooldownTimers[0] = 0;
        Debug.Log("Giamr hooi chiue");
        yield return new WaitUntil(() => !isProcessing); 

        // Kiểm tra lần nữa tránh lỗi nếu chaSkill bị xóa trong lúc chờ
        if (chaSkill != null && chaSkill.cooldownTimers != null && chaSkill.cooldownTimers.Length > 0)
        {
            chaSkill.cooldownTimers[0] = startCountDown;
        }
        this.transform.GetComponentInParent<ImpactOnPlayer>().isUseSkill = false;
    }
}
