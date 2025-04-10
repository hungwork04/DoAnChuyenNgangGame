using UnityEngine;
[CreateAssetMenu(fileName = "SpeedUpSkill", menuName = "Skills/SpeedUpSkill")]
public class SpeedUpSkill : Skill
{    // Sát thương
    public float durTime = 10f;
    public int time=2;
    public override void Activate(GameObject character)
    {
        // Kích hoạt Collider tạm thời
        ActiveSpeedUp act = character.GetComponentInChildren<ActiveSpeedUp>();
        //Debug.Log("here: " + character);

        if (act == null) return;
        act.StartCoroutine(act.speedUp(durTime, time));

        //Debug.Log($"{character.name} used Kick!");
    }
}
