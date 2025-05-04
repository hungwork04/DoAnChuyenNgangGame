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
    public Collider2D col2d;
    private void Start()
    {
        originCol=this.GetComponentInParent<SpriteRenderer>().color;
        startSpeed = playerMovement.startmoveSpeed;
        col2d = this.GetComponentInParent<Collider2D>();
        auraObj.SetActive(false);
        col2d.enabled = false;
    }
    public IEnumerator speedUp(float remainTime,int time)
    {
        auraObj.SetActive(true);
        playercol.color = rageColor;
        this.GetComponentInParent<ImpactOnPlayer>().isUseSkill = true;
        this.GetComponentInParent<ImpactOnPlayer>().SkillInUse.Add(1);
        this.GetComponentInParent<Animator>().SetBool("isSpeedUp", true);
        playerMovement.moveSpeed = time*startSpeed;
        col2d.enabled = true;
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
        col2d.enabled = false;
        playercol.color = originCol;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("canbeharm"))
        {
            collision.gameObject.GetComponent<ImpactOnPlayer>().blockMove(true);
            collision.gameObject.GetComponent<ImpactOnPlayer>().isKnockback = true;
        }
    }
}
