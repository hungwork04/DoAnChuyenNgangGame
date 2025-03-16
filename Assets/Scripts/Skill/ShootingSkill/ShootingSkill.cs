using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShootingSkill", menuName = "Skills/ShootingSkill")]
public class ShootingSkill : Skill
{
    public int damage = 0;        // Sát thương

    public override void Activate(GameObject character)
    {
        // Kích hoạt Collider tạm thời
        //kích hoạt hàm bắn
        ShootingBullet shootingBullet = character.GetComponentInChildren<ShootingBullet>();
        shootingBullet.shooting();
        //Debug.Log("here: "+ character);
        //Debug.Log("here: ");
        //Debug.Log($"{user.name} used Kick!");
    }
}
