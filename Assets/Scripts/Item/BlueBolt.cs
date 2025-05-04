using UnityEngine;

public class BlueBolt : ItemDefault
{
    //giải khống chế
    public override void UsingItem(Transform owner)
    {
        var plyerAva = owner.GetComponentInChildren<playerAvatar>();
        ImpactOnPlayer impact = plyerAva.avatarList[plyerAva.index].GetComponent<ImpactOnPlayer>();
        if (impact != null) { 
            impact.refreshYourSelf();
        }
    }

}
