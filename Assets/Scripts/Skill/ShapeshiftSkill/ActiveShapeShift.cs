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
        {
            yield break; // Thoát khỏi IEnumerator ngay lập tức nếu có null
        }

        int thisIndex = 2;
        int copyIndex = CopiedPlayer.GetComponentInParent<playerAvatar>()?.index ?? -1; // Kiểm tra null an toàn

        if (copyIndex == -1)
        {
            yield break; // Thoát nếu copyIndex không hợp lệ
        }
        avatar.setAva(copyIndex);
        //đổi cả playercolor bên playermovement
        playerMovement.PlayerColor= avatar.avatarList[(CopiedPlayer.GetComponentInParent<playerAvatar>().index)].GetComponent<SpriteRenderer>();
        //đổi character bên characterSkillmanage
        try
        {
            chaSkillmana.currentCharacter = CopiedPlayer.GetComponent<ImpactOnPlayer>().whichPlayer;
            chaSkillmana.cooldownTimers[0] = chaSkillmana.currentCharacter.skills[0].cooldown;
            chaSkillmana.currCooldownTimers[0] = 0;
            chaSkillmana.cooldownTimers[1] = chaSkillmana.currentCharacter.skills[1].cooldown;
            chaSkillmana.currCooldownTimers[1] = 0;
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("k có skill 2");
        }
        Debug.Log("hể");
        yield return new WaitForSeconds(timeRemain);
        Debug.Log("hết rồi");
        playerMovement.PlayerColor = startPlayer.GetComponent<SpriteRenderer>();
        chaSkillmana.currentCharacter = startPlayer.GetComponent<ImpactOnPlayer>().whichPlayer;
        chaSkillmana.cooldownTimers[0] = chaSkillmana.currentCharacter.skills[0].cooldown;
        chaSkillmana.currCooldownTimers[0] = 0;
        try
        {
            chaSkillmana.cooldownTimers[1] = chaSkillmana.currentCharacter.skills[1].cooldown;
            chaSkillmana.currCooldownTimers[1] = 10;
        }
        catch (IndexOutOfRangeException) {
            Debug.Log("k có skill 2");
        }//sửa lại cho tối ưu ra hàm riêng
        avatar.setAva(thisIndex);
    }

}
