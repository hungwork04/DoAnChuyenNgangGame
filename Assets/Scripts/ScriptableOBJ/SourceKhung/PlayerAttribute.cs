using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Character", menuName = "Characters/New Character")]
public class PlayerAttribute : ScriptableObject
{
    //khuôn cho mỗi nhân vật
    public string characterName;
    public Skill[] skills = new Skill[2]; // Mỗi nhân vật có 2 kỹ năng
}
