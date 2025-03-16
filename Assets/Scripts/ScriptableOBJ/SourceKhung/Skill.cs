using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skills/New Skill")]
public class Skill : ScriptableObject
{
    //khuôn cho mỗi skill
    public string skillName;
    public float cooldown;

    public virtual void Activate(GameObject user)
    {
        Debug.Log($"{user.name} used skill: {skillName}");
    }
}
