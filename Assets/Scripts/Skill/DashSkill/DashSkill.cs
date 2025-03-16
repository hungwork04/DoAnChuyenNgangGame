using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DashSkill", menuName = "Skills/DashSkill")]
public class DashSkill : Skill
{
    public int damage = 0;        // Sát thương
    public float dashForce = 10;
    public override void Activate(GameObject character)
    {
        // Kích hoạt Collider tạm thời
        ActiveDash act= character.GetComponentInChildren<ActiveDash>();
        //Debug.Log("here: "+ character);

        if (act == null) return;
        act.StartCoroutine(act.Dash(dashForce));
        //Debug.Log("here: ");
        //Debug.Log($"{user.name} used Kick!");
    }
}
