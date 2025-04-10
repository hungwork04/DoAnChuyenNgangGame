using System.Collections;
using UnityEngine;

public class SpawnPosReturn : MonoBehaviour
{
    public GameObject GOtoReturn;
    public GameObject parentObj;
    public Transform posToReturn=null;
    public ImpactOnPlayer impact;
    private Coroutine currCoroutine=null;
    private void Awake()
    {
        if(impact == null) 
            impact = GetComponentInParent<ImpactOnPlayer>();
    }
    public void doReturn(float remainTime)
    {
        if (impact.isUseSkill)
        {
            returnPos();
           // Debug.Log("here");
        }
        else
        {
            spawnPos(remainTime);
           // Debug.Log("here2");
        }
    }
    public void spawnPos(float remainTime)
    {
        if (posToReturn != null) return;
        posToReturn = Instantiate(GOtoReturn,parentObj.transform.position,Quaternion.identity).transform;
        if (impact != null) {
            impact.isUseSkill = true;
            impact.SkillInUse.Add(1);
        }
        if (currCoroutine != null)
        {
            StopCoroutine(currCoroutine);
            currCoroutine = null;
        }
        currCoroutine = StartCoroutine(countDowntoReturn(remainTime));

    }
    public IEnumerator countDowntoReturn(float remainTime)
    {
        yield return new WaitForSeconds(remainTime);
        returnPos();
    }
    private void OnDisable()
    {
        returnPos();
    }
    public void returnPos()
    {
        if (posToReturn != null)
        {
            if(currCoroutine != null)
            {
                StopCoroutine(currCoroutine);
                currCoroutine = null;
            }
            parentObj.transform.position = posToReturn.position;
            Destroy(posToReturn.gameObject);
            posToReturn = null;
            impact.isUseSkill = false;
            impact.SkillInUse.Remove(1);
        }
    }
}
