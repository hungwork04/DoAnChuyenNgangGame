using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInviciableEff : MonoBehaviour
{
    public float invincibleTime = 5f; // Thời gian miễn nhiễm
    public float blinkInterval = 0.2f; // Khoảng thời gian nhấp nháy

    public SpriteRenderer spriteRenderer;
    private bool isInvincible = false;

    void Start()
    {
        playerAvatar plyerAva = this.transform.parent.GetComponentInChildren<playerAvatar>();
        spriteRenderer = plyerAva.avatarList[plyerAva.index].GetComponent<SpriteRenderer>();
    }

    public void ActivateInvincibility()
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        float elapsedTime = 0f;
        while (elapsedTime < invincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Bật/tắt sprite
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        spriteRenderer.enabled = true; // Đảm bảo bật lại khi kết thúc
        isInvincible = false;
    }
}
