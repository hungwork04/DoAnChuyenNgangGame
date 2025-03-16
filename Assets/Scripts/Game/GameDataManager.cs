using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public List<PlayerDeviceInfo> playerDevicesInfo = new List<PlayerDeviceInfo>();//data

    private void OnEnable()
    {
        foreach (PlayerDeviceInfo playerDeviceInfo in playerDevicesInfo)
        {
            Debug.Log(playerDeviceInfo.device);
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerDevicesInfo(List<PlayerDeviceInfo> devicesInfo)
    {
        if (devicesInfo == null)
        {
            Debug.LogError("Thông tin thiết bị của người chơi không hợp lệ!");
            return;
        }

        playerDevicesInfo = devicesInfo;
        //Debug.Log(playerDevicesInfo.Count);

    }
}
