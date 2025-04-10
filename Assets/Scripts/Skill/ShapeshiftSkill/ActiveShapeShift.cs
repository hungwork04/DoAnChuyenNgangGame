using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class ActiveShapeShift : MonoBehaviour
{
    public Collider2D colActive;
    public GameObject CopiedPlayer=null;
    public playerAvatar avatar;
    public PlayerMovement playerMovement;
    public CharacterSkillManager chaSkillmana;
    public GameObject startPlayer;
    public bool isInShapeShifting=false;
    public bool isFirstTimeUse=true;
    //public PlayerAby playerAbility;
    private void Awake()
    {
        colActive = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (avatar)
        {
            this.transform.localScale = new Vector3((avatar.transform.localScale.x)/Math.Abs(avatar.transform.localScale.x), transform.localScale.y);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CopiedPlayer=collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CopiedPlayer = null;
        }
    }
    public IEnumerator activeShapeShiftSkill(float timeRemain)
    {
        if (CopiedPlayer == null || avatar == null)
            yield break; // Thoát nếu có null

        int thisIndex = 2;
        int copyIndex = CopiedPlayer.GetComponentInParent<playerAvatar>()?.index ?? -1;
        if (copyIndex == -1)
            yield break; // Thoát nếu copyIndex không hợp lệ
        
        ApplyAvatarAndSkills(copyIndex, CopiedPlayer);
        startPlayer.GetComponent<ImpactOnPlayer>().SkillInUse.Add(1);
        //Debug.Log("hể");
        isInShapeShifting=true;
        resetPlayerBombHandle(this.transform.parent);

        yield return new WaitForSeconds(timeRemain);

        Debug.Log("hết rồi");

        resolveProblemAtLast(avatar.avatarList[copyIndex]);

        ApplyAvatarAndSkills(thisIndex, startPlayer);
        isInShapeShifting = false;
        startPlayer.GetComponent<ImpactOnPlayer>().SkillInUse.Remove(1);
    }
    public void resolveProblemAtLast(GameObject charact)
    {
        if(charact==null) return;
        if (charact.GetComponentInChildren<MeleeColliderInteract>())
        {
            charact.GetComponentInChildren<MeleeColliderInteract>().DeactivateCollider();
            Debug.Log("resetatk");
        }
        else if (charact.GetComponentInChildren<MeleeAtkforBigGuy>())
        {
            charact.GetComponentInChildren<MeleeAtkforBigGuy>().DeactivateCollider();
            Debug.Log("resetBigGuy1");
        }
        else if (charact.GetComponentInChildren<GroundpoundCollider>())
        {
            charact.GetComponentInChildren<GroundpoundCollider>().DeactivateCollider();
            Debug.Log("resetbombGuy");
        }

        if (charact.GetComponentInChildren<ActiveGrowUpSkill>())
        {
            charact.GetComponentInChildren<ActiveGrowUpSkill>().beforeDisable();
            Debug.Log("resetBigG");
        }else if (charact.GetComponentInChildren<ActiveDash>())
        {
            charact.GetComponentInChildren<ActiveDash>().StopAllCoroutines();
            Debug.Log("resetDash");
        }else if (charact.GetComponentInChildren<ActiveSpeedUp>())
        {
            charact.GetComponentInChildren<ActiveSpeedUp>().endSkill();
        }
    }
    private void ApplyAvatarAndSkills(int avatarIndex, GameObject player)
    {
        avatar.setAva(avatarIndex);
        playerMovement.PlayerColor = avatar.avatarList[avatarIndex].GetComponent<SpriteRenderer>();

        chaSkillmana.currentCharacter = player.GetComponent<ImpactOnPlayer>().whichPlayer;
        chaSkillmana.cooldownTimers[0] = chaSkillmana.currentCharacter.skills[0].cooldown;
        chaSkillmana.currCooldownTimers[0] = 0;

        try
        {
            chaSkillmana.cooldownTimers[1] = chaSkillmana.currentCharacter.skills[1].cooldown;
            chaSkillmana.currCooldownTimers[1] = (player == startPlayer) ? chaSkillmana.cooldownTimers[1] : 0;
            Debug.Log(chaSkillmana.currCooldownTimers[1]);
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("k có skill 2");
        }
    }
    public virtual void resetPlayerBombHandle(Transform coltrans)
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
