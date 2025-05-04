using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBombPooler : MonoBehaviour
{
    public static EffectBombPooler instance;

    // Danh sách các prefab effect bomb
    public List<GameObject> effectBombPrefabs = new List<GameObject>();

    // Dictionary để lưu trữ các pool cho từng loại effect bomb
    private Dictionary<int, List<GameObject>> effectBombPools = new Dictionary<int, List<GameObject>>();

    // Số lượng effect bomb khởi tạo ban đầu cho mỗi loại
    public int initialPoolSize = 5;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // Khởi tạo pool cho từng loại effect bomb
        InitializePools();
    }



    private void InitializePools()
    {
        // Tạo pool cho từng loại effect bomb
        for (int i = 0; i < effectBombPrefabs.Count; i++)
        {
            List<GameObject> pool = new List<GameObject>();

            // Tạo các đối tượng ban đầu cho pool
            for (int j = 0; j < initialPoolSize; j++)
            {
                GameObject obj = CreateNewEffectBomb(i);
                obj.SetActive(false);
                pool.Add(obj);
            }

            // Thêm pool vào dictionary
            effectBombPools.Add(i, pool);
        }
    }

    private GameObject CreateNewEffectBomb(int bombType)
    {
        GameObject obj = Instantiate(effectBombPrefabs[bombType], transform);
        return obj;
    }

    // Lấy một effect bomb từ pool
    public GameObject GetEffectBombFromPool(int bombType, Vector3 position, Quaternion rotation)
    {
        // Kiểm tra xem có pool cho loại bomb này chưa
        if (!effectBombPools.ContainsKey(bombType))
        {
            Debug.LogWarning("Không có pool cho loại effect bomb: " + bombType);
            return null;
        }

        List<GameObject> pool = effectBombPools[bombType];

        // Tìm một đối tượng không active trong pool
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        // Nếu không có đối tượng nào không active, tạo một đối tượng mới
        GameObject newObj = CreateNewEffectBomb(bombType);
        newObj.transform.position = position;
        newObj.transform.rotation = rotation;
        pool.Add(newObj);

        return newObj;
    }

    // Trả một effect bomb về pool
    public void ReturnToPool(GameObject effectBomb)
    {
        Debug.Log("EffectBombPooler.ReturnToPool called for " + effectBomb.name);

        // Reset các component nếu cần
        ResetEffectBomb(effectBomb);

        // Ẩn effect bomb
        effectBomb.SetActive(false);
        Debug.Log("Effect bomb set to inactive: " + effectBomb.name);
    }

    // Reset các component của effect bomb
    private void ResetEffectBomb(GameObject effectBomb)
    {
        Debug.Log("Resetting effect bomb components for " + effectBomb.name);

        // Reset các component PointEffector2D nếu có
        PointEffector2D[] pointEffectors = effectBomb.GetComponentsInChildren<PointEffector2D>(true);
        foreach (PointEffector2D effector in pointEffectors)
        {
            effector.enabled = false;
            Debug.Log("Disabled PointEffector2D on " + effector.gameObject.name);
        }

        // Reset các component BombEffected
        BombEffected[] bombEffects = effectBomb.GetComponentsInChildren<BombEffected>(true);
        foreach (BombEffected effect in bombEffects)
        {
            // Hủy bỏ các Invoke đang chờ
            effect.CancelInvoke();
            Debug.Log("Cancelled Invokes on BombEffected " + effect.gameObject.name);
        }

        // Có thể thêm các reset khác nếu cần
    }
}
