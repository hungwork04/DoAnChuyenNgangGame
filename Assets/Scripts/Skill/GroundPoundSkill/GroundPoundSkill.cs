using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GroundPoundSkill", menuName = "Skills/GroundPoundSkill")]
public class GroundPoundSkill : Skill
{
    public float activeTime = 0.2f; // Thời gian kích hoạt Collider
    public int damage = 20;        // Sát thương

    public override void Activate(GameObject user)
    {
        // Kích hoạt Collider tạm thời
        GroundpoundCollider kickHandler = user.GetComponentInChildren<GroundpoundCollider>();
        if (kickHandler != null)
        {
            //Debug.Log($"{kickHandler.name} used!");
            kickHandler.ActivateCollider(activeTime, damage);
        }

        //Debug.Log($"{user.name} used Kick!");
    }
}
