using System.Collections;
using System.Collections.Generic;

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
        //if (PoolingObj != null) return;
        ////PoolingObj = transform.Find("/Pooling").GetComponent<Transform>();
        //PoolingObj=this.gameObject;
    }
    public void Start()
    {
        StartSpawningBombs();
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

    public IEnumerator SpawnBomb(float delay)
    {
        while (checkBombInPool() <= 6) // Đảm bảo không spawn quá nếu điều kiện sai
        {
            yield return new WaitForSeconds(delay);
            GetObjectFromPool(0,2);
            GetObjectFromPool(2,4);
            GetObjectFromPool(4,6);
            GetObjectFromPool(6,8);
        }
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
        foreach (GameObject obj in listbomb)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.parent = PoolingObj.transform;
                obj.transform.position = RandomPos(st, end).position;
                obj.transform.rotation = RandomPos(st,end).rotation;
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
