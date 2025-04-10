using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeColliderTele : MeleeAtkforBigGuy {
    public teleToAtk teleSkill;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        return;
    }
    public override void ActivateCollider(float duration, int skillDamage)
    {
        if (MeleeCollider != null)
        {
            damage = skillDamage;
            teleSkill.doTele();
            MeleeCollider.enabled = true; // Bật Collider
                //colPos.position= this.transform.position;
                Collider2D[] cols = Physics2D.OverlapCircleAll(MeleeCollider.transform.position, radiusCol, layer);
                processingKnockBack(cols);
            MeleeCollider.GetComponentInParent<ImpactOnPlayer>().isUsingSkillCanMove = true;
            // Tắt Collider sau thời gian kích hoạt
            Invoke(nameof(DeactivateCollider), duration);

        }
    }

}
