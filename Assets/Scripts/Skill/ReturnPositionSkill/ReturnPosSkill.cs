using UnityEngine;
[CreateAssetMenu(fileName = "ReturnPosSkill", menuName = "Skills/ReturnPosSkill")]
public class ReturnPosSkill : Skill
{
    public float activeTime = 5f; // Thời gian kích hoạt Collider
    public override void Activate(GameObject user)
    {
        // Kích hoạt Collider tạm thời
        SpawnPosReturn collider = user.GetComponentInChildren<SpawnPosReturn>();
        if (collider != null)
        {
            collider.doReturn(activeTime);

            //Debug.Log($"{user.name} used Kick!1");
        }

        //Debug.Log($"{user.name} used Kick!");
    }
}
