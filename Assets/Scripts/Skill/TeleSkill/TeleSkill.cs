using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TeleSkill", menuName = "Skills/TeleSkill")]
public class TeleSkill : Skill
{
    public int damage = 0;        // Sát thương
    public override void Activate(GameObject character)
    {
        // Kích hoạt Collider tạm thời
        MeleeColliderTele collider = character.GetComponentInChildren<MeleeColliderTele>();
        if (collider != null)
        {
            collider.ActivateCollider(0.3f, damage);
            Debug.Log($"{collider.name} used!");
        }
        //Debug.Log("here: "+ character);
        //Debug.Log("here: ");
        //Debug.Log($"{user.name} used Kick!");
    }

}
