using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public static BombSpawner instance;
    public GameObject bomb;
    public GameObject PoolingObj;
    public List<GameObject> listbomb = new List<GameObject>();
    public List<Transform> listPosSpawn = new List<Transform>();//
    private Coroutine spawnBombCoroutine;
    private void Awake()
    {
        if (instance != null && instance!=this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }
    public void Start()
    {
        StartSpawningBombs();

        spawnCounts = new int[zoneCount];
        StartCoroutine(SpawnBomb(1f)); // Hoặc delay tùy bạn
    }

    private void Update()
    {
        if (checkBombInPool() > 6)
        {
            if (spawnBombCoroutine != null)
            {
                StopCoroutine(spawnBombCoroutine);
                spawnBombCoroutine = null; // Đặt về null để kiểm tra lại
            }
        }
        else if (spawnBombCoroutine == null) // Chỉ bắt đầu lại nếu chưa có coroutine đang chạy
        {
            StartSpawningBombs();
        }
    }

    private void StartSpawningBombs()
    {
        spawnBombCoroutine = StartCoroutine(SpawnBomb(5f));
    }

    //public IEnumerator SpawnBomb(float delay)
    //{
    //    while (checkBombInPool() <= 6) // Đảm bảo không spawn quá nếu điều kiện sai
    //    {
    //        yield return new WaitForSeconds(delay);
    //        GetObjectFromPool(0,2);
    //        GetObjectFromPool(2,4);
    //        GetObjectFromPool(4,6);
    //        //GetObjectFromPool(6,8);
    //    }
    //}
    // Số lượng vùng spawn bạn đang có
    private int zoneCount = 3;

    // Lưu số lượng bomb đã spawn ở mỗi zone
    private int[] spawnCounts;


    public IEnumerator SpawnBomb(float delay)
    {
        while (checkBombInPool() <= 6)
        {
            yield return new WaitForSeconds(delay);

            int selectedZone = GetBalancedZoneIndex();

            switch (selectedZone)
            {
                case 0:
                    GetObjectFromPool(0, 2);
                    break;
                case 1:
                    GetObjectFromPool(2, 4);
                    break;
                case 2:
                    GetObjectFromPool(4, 6);
                    break;
                    // Nếu bạn có thêm vùng thì mở rộng tiếp
            }

            spawnCounts[selectedZone]++;
        }
    }

    // Thuật toán chọn zone có ít bomb hơn (ngẫu nhiên có trọng số)
    private int GetBalancedZoneIndex()
    {
        int total = 0;
        foreach (int count in spawnCounts) total += count;

        float[] weights = new float[zoneCount];
        for (int i = 0; i < zoneCount; i++)
        {
            // Trọng số ngược: vùng ít spawn sẽ có trọng số cao hơn
            weights[i] = total == 0 ? 1f : (float)(total - spawnCounts[i] + 1);
        }

        // Chọn ngẫu nhiên theo trọng số
        float sum = weights.Sum();
        float rand = Random.Range(0f, sum);

        float accum = 0f;
        for (int i = 0; i < zoneCount; i++)
        {
            accum += weights[i];
            if (rand <= accum)
                return i;
        }

        return 0; // fallback
    }
    public Transform RandomPos(int start, int end)
    {
        int index = Random.Range(start, end);
        return listPosSpawn[index];
    }

    public int checkBombInPool()
    {
        int amount = 0;
        foreach (GameObject obj in listbomb)
        {
            if (!obj.activeInHierarchy)
            {
                amount += 0;
            }else amount += 1;

        }
        return amount;
    }
    public void GetObjectFromPool(int st,int end)//spawn bomb
    {
        // Tìm đối tượng không active trong pool
        //Debug.Log("hehe");

        foreach (GameObject obj in listbomb)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.parent = PoolingObj.transform;

                Transform spawnPoint = RandomPos(st, end);

                // Dịch chuyển trục X ngẫu nhiên trong khoảng từ 0.01 đến 0.03
                Vector3 newPosition = spawnPoint.position;
                newPosition.x += Random.Range(-0.1f, 0.15f);
                //Debug.Log(newPosition.x);
                // Áp dụng vị trí và xoay (rotation) cho đối tượng
                obj.transform.position = newPosition;
                obj.transform.rotation = spawnPoint.rotation;
                return;
            }
        }

        // Nếu không có đối tượng nào, tạo đối tượng mới và thêm vào pool
        GameObject newbom = Instantiate(bomb, RandomPos(st,end).position, RandomPos(st, end).rotation);
        newbom.transform.parent = PoolingObj.transform;
        listbomb.Add(newbom);
        return;
    }
}
