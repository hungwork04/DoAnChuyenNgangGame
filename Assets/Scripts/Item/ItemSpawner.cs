using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner instance;
    public List<GameObject> Items = new List<GameObject>();
    public GameObject PoolingObj;
    public List<GameObject> listItem = new List<GameObject>();
    public List<Transform> listPosSpawn = new List<Transform>();
    private Coroutine spawnItemCoroutine;
    public float waitTime = 30f;
    public bool canspawn = true;

    // Mảng để theo dõi trạng thái của các điểm spawn
    private bool[] spawnPointOccupied;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
        
        // Khởi tạo mảng theo dõi với 6 điểm spawn
        spawnPointOccupied = new bool[6];
    }

    public void Start()
    {
        StartCoroutine(SpawnItem(1f));
    }

    private float lastSpawnTime = -Mathf.Infinity;
    private bool hasWaited = true;

    void Update()
    {
        int activeItems = checkItemInPool();
        if (activeItems < 6)
        {
            if (spawnItemCoroutine == null && canspawn && hasWaited && Time.time - lastSpawnTime >= 30f)
            {
                spawnItemCoroutine = StartCoroutine(SpawnItem(1f));
            }
        }
        else
        {
            if (spawnItemCoroutine != null)
            {
                StopCoroutine(spawnItemCoroutine);
                spawnItemCoroutine = null;
                canspawn = false;
                hasWaited = false;
                StartCoroutine(waitBeforeSpawn());
            }
        }
    }

    IEnumerator waitBeforeSpawn()
    {
        yield return new WaitForSeconds(waitTime);
        canspawn = true;
        hasWaited = true;
    }

    public IEnumerator SpawnItem(float delay)
    {
        while (checkItemInPool() < 6)
        {
            yield return new WaitForSeconds(delay);

            // Tìm vị trí trống để spawn
            int spawnPoint = FindEmptySpawnPoint();
            if (spawnPoint != -1)
            {
                GetObjectFromPool(spawnPoint);
                lastSpawnTime = Time.time;
            }
        }
        spawnItemCoroutine = null;
    }

    // Tìm một điểm spawn trống
    private int FindEmptySpawnPoint()
    {
        // Tìm tất cả các điểm đang có item
        for (int i = 0; i < listPosSpawn.Count && i < 6; i++)
        {
            spawnPointOccupied[i] = false;
        }

        // Đánh dấu các điểm đã có item
        foreach (GameObject item in listItem)
        {
            if (item.activeInHierarchy)
            {
                for (int i = 0; i < listPosSpawn.Count && i < 6; i++)
                {
                    if (Vector3.Distance(item.transform.position, listPosSpawn[i].position) < 0.5f)
                    {
                        spawnPointOccupied[i] = true;
                        break;
                    }
                }
            }
        }

        // Tìm điểm trống
        for (int i = 0; i < 6; i++)
        {
            if (!spawnPointOccupied[i])
            {
                return i;
            }
        }

        return -1; // Không tìm thấy điểm trống
    }

    public int checkItemInPool()
    {
        int amount = 0;
        foreach (GameObject obj in listItem)
        {
            if (obj.activeInHierarchy)
            {
                amount += 1;
            }
        }
        return amount;
    }

    public void GetObjectFromPool(int spawnPointIndex)
    {
        foreach (GameObject obj in listItem)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.parent = PoolingObj.transform;

                Vector3 newPosition = listPosSpawn[spawnPointIndex].position;
                newPosition.x += Random.Range(-0.1f, 0.2f);
                
                obj.transform.position = newPosition;
                obj.transform.rotation = listPosSpawn[spawnPointIndex].rotation;
                return;
            }
        }

        // Nếu không có đối tượng nào trong pool, tạo mới
        GameObject newItem = Instantiate(randomItem(), listPosSpawn[spawnPointIndex].position, listPosSpawn[spawnPointIndex].rotation);
        newItem.transform.parent = PoolingObj.transform;
        listItem.Add(newItem);
    }

    public GameObject randomItem()
    {
        int rad = Random.Range(1, 10);
        if (rad < 5)
        {
            return Items[0];
        }
        else if (rad >= 5 && rad <= 8) // Sửa lại điều kiện logic
        {
            return Items[1];
        }
        else return Items[2];
    }
}
