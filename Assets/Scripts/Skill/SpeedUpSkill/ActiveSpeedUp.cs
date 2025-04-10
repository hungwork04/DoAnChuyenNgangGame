using System.Collections;
using UnityEngine;

public class ActiveSpeedUp : MonoBehaviour
{
    public float GrowUpRemainTime = 5f;
    public float startSpeed;
    public float growSpeed = 1f;
    public PlayerMovement playerMovement;
    public GameObject auraObj;
    public SpriteRenderer playercol;
    public Color originCol;
    public Color rageColor = new Color(0.702f, 0.705f, 1f);
    private void Start()
    {
        originCol=this.GetComponentInParent<SpriteRenderer>().color;
        startSpeed = playerMovement.startmoveSpeed;
        auraObj.SetActive(false);
    }
    public IEnumerator speedUp(float remainTime,int time)
    {
        auraObj.SetActive(true);
        playercol.color = rageColor;
        this.GetComponentInParent<ImpactOnPlayer>().isUseSkill = true;
        this.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Add(1);
        this.GetComponentInParent<Animator>().SetBool("isSpeedUp", true);
        playerMovement.moveSpeed = time*startSpeed;
        yield return new WaitForSeconds(remainTime);
        endSkill();

    }

    public void endSkill()
    {
        playerMovement.moveSpeed = startSpeed;
        this.GetComponentInParent<ImpactOnPlayer>().isUseSkill = false;
        this.GetComponentInParent<Animator>().SetBool("isSpeedUp", false);
        this.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Remove(1);
        auraObj.SetActive(false);
        playercol.color = originCol;
    }
}
