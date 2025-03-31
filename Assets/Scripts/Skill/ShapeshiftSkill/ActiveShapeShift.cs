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
    public PlayerAby playerAbility;
    private void Awake()
    {
        colActive = GetComponent<Collider2D>();
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
        //Debug.Log("hể");
        resetPlayerBombHandle(this.transform.parent);
        yield return new WaitForSeconds(timeRemain);

        Debug.Log("hết rồi");
        ApplyAvatarAndSkills(thisIndex, startPlayer);
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
            chaSkillmana.currCooldownTimers[1] = (player == startPlayer) ? 10 : 0;
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
