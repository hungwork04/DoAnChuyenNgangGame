using System.Collections;
using UnityEngine;

public class RedBolt :  ItemDefault
{
    //Hồi máu
    public float healValue = 25f;
    public override void UsingItem(Transform owner)
    {
        var playerHeal = owner.GetComponentInChildren<PlayerHealth>();
        if (playerHeal)
        {
            playerHeal.doBuffHeal(healValue);
        }
    }

}
