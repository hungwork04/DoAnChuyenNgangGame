using UnityEngine;

public class GreenBolt : ItemDefault
{
    public override void UsingItem(Transform owner)
    {
        var playerSkill = owner.GetComponentInChildren<CharacterSkillManager>();
        if (playerSkill)
        {
            playerSkill.currCooldownTimers[0] = 0;
            playerSkill.currCooldownTimers[1] = 0;
        }
    }
}
