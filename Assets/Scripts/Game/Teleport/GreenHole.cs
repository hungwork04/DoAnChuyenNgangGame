using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreenHole : MonoBehaviour
{
    public Transform otherHole;
    public bool canTeleport = true;
    private float teleportDelay = 0.5f; // Thời gian ngắn để tránh teleport ngược lại

    // Dictionary để theo dõi người chơi đã teleport
    private static Dictionary<int, float> playerCooldowns = new Dictionary<int, float>();
    private float playerCooldownTime = 5f; // Thời gian người chơi không thể teleport lại (3 giây)

    void Update()
    {
        // Tạo một danh sách tạm thời để lưu các ID cần xóa
        List<int> playersToRemove = new List<int>();
        
        // Tạo một bản sao của các key để duyệt qua
        List<int> playerIDs = new List<int>(playerCooldowns.Keys);
        
        // Cập nhật cooldown của người chơi
        foreach (var playerID in playerIDs)
        {
            playerCooldowns[playerID] -= Time.deltaTime;
            if (playerCooldowns[playerID] <= 0)
            {
                playersToRemove.Add(playerID);
            }
        }
        
        // Xóa người chơi đã hết cooldown
        foreach (var playerID in playersToRemove)
        {
            playerCooldowns.Remove(playerID);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (otherHole == null) return;
        
        // Kiểm tra xem đây có phải là player không
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && canTeleport)
        {
            Transform playerTransform = collision.transform.parent.parent;
            
            // Lấy ID của player
            int playerID = playerTransform.GetInstanceID();
            
            // Kiểm tra xem người chơi có đang trong thời gian cooldown không
            if (playerCooldowns.ContainsKey(playerID))
            {
                return; // Người chơi vẫn đang trong thời gian cooldown
            }
            
            // Vô hiệu hóa teleport tạm thời
            canTeleport = false;
            
            // Teleport người chơi
            Debug.Log("tele");
            playerTransform.position = otherHole.position;
            
            // Thêm người chơi vào danh sách cooldown
            playerCooldowns[playerID] = playerCooldownTime;
            
            // Bắt đầu cooldown cho cổng
            StartCoroutine(ResetTeleportCooldown());
        }
        
    }
    
    private IEnumerator ResetTeleportCooldown()
    {
        // Đợi một khoảng thời gian ngắn
        yield return new WaitForSeconds(teleportDelay);
        
        // Kích hoạt lại teleport
        canTeleport = true;
    }
}
