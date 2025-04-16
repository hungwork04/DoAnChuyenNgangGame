using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrowUpSkill", menuName = "Skills/GrowUpSkill")]
public class GrowUpSkill : Skill
{
    public int damage = 0;        // Sát thương
    public float GrowUpRemainTime=5f;
    public override void Activate(GameObject character)
    {
        // Kích hoạt Collider tạm thời
        ActiveGrowUpSkill act = character.GetComponentInChildren<ActiveGrowUpSkill>();
        //Debug.Log("here: " + character);

        if (act == null) return;
        act.StartCoroutine(act.growUp(GrowUpRemainTime));

        //Debug.Log($"{character.name} used Kick!");
    }
}
