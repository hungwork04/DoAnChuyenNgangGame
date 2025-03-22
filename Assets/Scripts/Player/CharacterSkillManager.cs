using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Unity.Burst.Intrinsics.X86;

public class CharacterSkillManager : MonoBehaviour
{
    [Header("Character Configuration")]
    public PlayerAttribute currentCharacter; // Nhân vật hiện tại (có 2 kỹ năng) // sửa lại sao cho áp dụng với nhiều tướng
    public playerAvatar avatar;
    public float[] cooldownTimers = new float[2]; // Bộ đếm cooldown cho 2 kỹ năng
    public float[] currCooldownTimers = new float[2];
    private void Awake()
    {
        if(avatar==null) avatar = transform.parent.GetComponentInChildren<playerAvatar>();
    }
    void Start()
    {

        // Khởi tạo cooldown ban đầu
        if (avatar!=null)
        currentCharacter =avatar.GetComponentInChildren<ImpactOnPlayer>().whichPlayer;

        if (currentCharacter == null || currentCharacter.skills == null)
            return;

        int skillCount = currentCharacter.skills.Length;
        for (int i = 0; i < skillCount; i++)
        {
            cooldownTimers[i] = currentCharacter.skills[i].cooldown; // Lưu giá trị gốc
            currCooldownTimers[i] = 0; // Bắt đầu từ 0
        }
    }

    void Update()
    {
        // Giảm cooldown theo thời gian
        UpdateCooldownTimers();

    }

    private void UpdateCooldownTimers()
    {
        for (int i = 0; i < currCooldownTimers.Length; i++)
        {
            if (currCooldownTimers[i] > 0)
                currCooldownTimers[i] -= Time.deltaTime;
            else
                currCooldownTimers[i] = 0; // Đảm bảo không âm
        }
    }
    //Hàm cho Cucumber

    public void ActivateSkill(int skillIndex)
    {
        // Kiểm tra kỹ năng có tồn tại không
        if (currentCharacter == null || skillIndex >= currentCharacter.skills.Length)
        {
            Debug.LogWarning("Skill not available.");
            return;
        }

        Skill skill = currentCharacter.skills[skillIndex];

        // Kiểm tra cooldown
        if (currCooldownTimers[skillIndex] > 0)
        {
            Debug.Log($"Skill {skill.skillName} is on cooldown: {currCooldownTimers[skillIndex]:F1}s left.");
            return;
        }

        // Kích hoạt kỹ năng
        if (skill != null)
        {
            if (skillIndex == 0)
            {
                avatar.avatarList[avatar.index].GetComponent<Animator>().SetTrigger("isSkill1");//ifelse de doi animation
            }
            else if (skillIndex == 1 &&(avatar.index!=0 || avatar.index != 3))
            {
                avatar.avatarList[avatar.index].GetComponent<Animator>().SetTrigger("isSkill2");//ifelse de doi animation
            }
            var user = avatar.avatarList[avatar.index];
            skill.Activate(user);
            if (cooldownTimers[skillIndex] == 0)
                cooldownTimers[skillIndex] = skill.cooldown;

            // Reset currCooldownTimers để bắt đầu hồi chiêu
            currCooldownTimers[skillIndex] = cooldownTimers[skillIndex];
            //Debug.Log($"Activated skill: {skill.skillName}");
        }
    }

}