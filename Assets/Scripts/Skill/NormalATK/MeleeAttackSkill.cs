using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackSkill", menuName = "Skills/MeleeAttackSkill")]
public class MeleeAttackSkill : Skill
{
    public float activeTime = 0.2f; // Thời gian kích hoạt Collider
    public int damage = 20;        // Sát thương

    public override void Activate(GameObject user)
    {
        // Kích hoạt Collider tạm thời
        MeleeColliderInteract kickHandler = user.GetComponentInChildren<MeleeColliderInteract>();
        if (kickHandler != null)
        {
            //Debug.Log($"{kickHandler.name} used!");
            kickHandler.ActivateCollider(activeTime, damage);
        }

        //Debug.Log($"{user.name} used Kick!");
    }

}
