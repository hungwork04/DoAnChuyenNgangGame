using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterSkillManager : MonoBehaviour
{
    [Header("Character Configuration")]
    public PlayerAttribute currentCharacter; // Nhân vật hiện tại (có 2 kỹ năng) // sửa lại sao cho áp dụng với nhiều tướng
    public playerAvatar avatar;
    private float[] cooldownTimers = new float[2]; // Bộ đếm cooldown cho 2 kỹ năng
    
    private void Awake()
    {
        if(avatar==null) avatar = transform.parent.GetComponentInChildren<playerAvatar>();
    }
    void Start()
    {
        // Khởi tạo cooldown ban đầu
        cooldownTimers[0] = 0;
        cooldownTimers[1] = 0;
        if(avatar!=null)
        currentCharacter =avatar.GetComponentInChildren<ImpactOnPlayer>().whichPlayer;
    }

    void Update()
    {
        // Giảm cooldown theo thời gian
        UpdateCooldownTimers();

        // Kích hoạt kỹ năng 1
/*        if (Input.GetKeyDown(KeyCode.Q))//phair sua
        {
            ActivateSkill(0); // Kích hoạt kỹ năng đầu tiên
        }*/

/*        // Kích hoạt kỹ năng 2
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateSkill(1); // Kích hoạt kỹ năng thứ hai
        }*/
    }

    private void UpdateCooldownTimers()
    {
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
                cooldownTimers[i] -= Time.deltaTime;
        }
    }

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
        if (cooldownTimers[skillIndex] > 0)
        {
            Debug.Log($"Skill {skill.skillName} is on cooldown: {cooldownTimers[skillIndex]:F1}s left.");
            return;
        }

        // Kích hoạt kỹ năng
        if (skill != null)
        {
            avatar.avatarList[avatar.index].GetComponent<Animator>().SetTrigger("isSkill1");
            var user = avatar.avatarList[avatar.index];
            skill.Activate(user);
            cooldownTimers[skillIndex] = skill.cooldown;
            Debug.Log($"Activated skill: {skill.skillName}");
        }
    }
}