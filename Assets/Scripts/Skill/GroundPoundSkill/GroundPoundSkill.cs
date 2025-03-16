using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GroundPoundSkill", menuName = "Skills/GroundPoundSkill")]
public class GroundPoundSkill : Skill
{
    public float activeTime = 0.9f; // Thời gian kích hoạt Collider
    public int damage = 0;        // Sát thương

    public override void Activate(GameObject user)
    {
        // Kích hoạt Collider tạm thời
        //BlowColliderInteract collider = user.GetComponentInChildren<BlowColliderInteract>();
        //if (collider != null)
        //{
        //    //Debug.Log($"{collider.name} used!");
        //    collider.ActivateCollider(activeTime, damage);
        //}

        //Debug.Log($"{user.name} used Kick!");
    }
}
