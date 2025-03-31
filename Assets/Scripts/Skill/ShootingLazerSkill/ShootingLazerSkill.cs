using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "ShootingLazerSkill", menuName = "Skills/ShootingLazerSkill")]
public class ShootingLazerSkill : Skill
{
    public float activeTime = 5f; // Thời gian kích hoạt Collider
    public override void Activate(GameObject user)
    {
        // Kích hoạt Collider tạm thời
        ActiveLazer collider = user.GetComponentInChildren<ActiveLazer>();
        if (collider != null)
        {
            collider.ToggleLazer(activeTime);

            //Debug.Log($"{user.name} used Kick!1");
        }

        //Debug.Log($"{user.name} used Kick!");
    }
}
