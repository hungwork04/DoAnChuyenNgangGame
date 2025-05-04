using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerSpawnManager : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints; // Danh sách các điểm spawn
    //public InputActionAsset  inputAction;
    [Header("Player References")]
    public GameObject player; // Danh sách các nhân vật đã có trong scene
    public List<PlayerDeviceInfo> playerDevicesInfo;
    void Start()
    {
        
        playerDevicesInfo = GameDataManager.instance.playerDevicesInfo;
        if (playerDevicesInfo != null && playerDevicesInfo.Count > 0)
        {
            SpawnPlayers();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy thông tin người chơi để spawn nhân vật!");
        }
    }

    public void SpawnPlayers()
    {
        foreach (var playerInfo in playerDevicesInfo)
        {
            // Kiểm tra vị trí spawn hợp lệ
            if (playerInfo.playerIndex < 0 || playerInfo.playerIndex >= spawnPoints.Length)
            {
                Debug.LogError($"Player index {playerInfo.playerIndex} không hợp lệ!");
                continue;
            }

            // Lấy nhân vật đã tồn tại trong scene
            GameObject playersp = Instantiate(player);
            playersp.name = "player" + playerInfo.playerIndex;
            if (playersp == null)
            {
                Debug.LogError($"Không tìm thấy player tại index {playerInfo.playerIndex}");
                continue;
            }
            // Đổi loại nhân vật (characterType)
            var avatar = playersp.GetComponentInChildren<playerAvatar>();
            if (avatar != null)
            {
                //int tesst = 2;//characterType
                if(avatar.plcl==Color.white) avatar.plcl = playerInfo.playercolor;
                //Debug.Log(avatar.plcl);
                avatar.setAva(playerInfo.characterType);

            }
            else
            {
                Debug.LogError($"Player tại index {playerInfo.playerIndex} không có CharacterManager!");
                continue;
            }

            // Di chuyển nhân vật đến vị trí spawn
            Transform spawnPoint = spawnPoints[playerInfo.playerIndex];
            playersp.transform.position = spawnPoint.position;
            playersp.transform.rotation = spawnPoint.rotation;


            // Gán thiết bị điều khiển cho PlayerInput
            var playerMovement = playersp.GetComponentInChildren<PlayerMovement>();
            var playerInput = playerMovement.gameObject.GetComponent<PlayerInput>();
            //Debug.Log(playerInput.transform.parent.gameObject);
            
            if (playerInput != null)
            {
                // Gán thiết bị đầu vào
                InputDevice device = playerInfo.device;
                if (device != null)
                {
                    try
                    {
                        playerInput.SwitchCurrentControlScheme(device);
                    }
                    catch (ArgumentException)
                    {
                        Debug.Log("lỗi kết nối thiết bị");
                    }

                    //Debug.Log($"Player {playerInfo.playerIndex} sử dụng thiết bị {device.displayName}");
                }
                else
                {
                    Debug.LogError($"Player tại index {playerInfo.playerIndex} không có thiết bị điều khiển hợp lệ!");
                }
            }
            else
            {
                Debug.LogError($"Player tại index {playerInfo.playerIndex} không có PlayerInput!");
            }
        }
    }
}
