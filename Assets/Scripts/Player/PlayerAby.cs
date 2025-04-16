using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static UnityEngine.InputSystem.InputAction;


public class PlayerAby : MonoBehaviour
{
    public GameObject Object;
    public float holdTime = 0;
    public float maxThrowForce = 15f;
    public float scanRadius = 0.65f;

    public bool isholdBomb=false;
    public bool isCanTakeObj=false;
    public bool isCanOpenDoor=false;
    public bool isPressButton=false;

    public Transform holdBombPos;
    //public BombSpawner bombSpawner;
    public Transform Ava;
    public Transform TeleDoor;
    public PlayerInventory inventory;
    public PlayerPowerBar playerPowerBar;
    private void Awake()
    {
        //bombSpawner = FindObjectOfType<BombSpawner>();
        if(inventory == null)
        {
            inventory = transform.parent.GetComponentInChildren<PlayerInventory>();
        }
        if (Ava==null) Ava = this.transform.parent.GetComponentInChildren<playerAvatar>().transform;
        if (holdBombPos != null) return;
        holdBombPos=transform.Find("HoldBombPos").GetComponent<Transform>();
    }
    private void Start()
    {
        playerPowerBar?.SetHealth(0);
    }
    private void Update()
    {
        checkDKHoldBomb();
        checkPressbutton();
    }
    public void checkDKHoldBomb()
    {
        // Nếu không có Object hoặc object đó không còn active trong scene nữa
        if (Object == null || !Object.transform.parent.gameObject.activeSelf)
        {
            // Không còn giữ bomb nữa và reset Object về null
            isholdBomb = false;
            Object = null;
            return;
        }

        // Nếu đang giữ bomb
        if (isholdBomb)
        {
            // Không thể lấy object nữa
            isCanTakeObj = false;
        }
        // Nếu không giữ bomb và đang có Object khả dụng
        else if (!isholdBomb && Object != null)
        {
            // Có thể lấy object
            isCanTakeObj = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ruou") && inventory.canAdd())
        {
            if (Object != null) return;
            Object = collision.gameObject;
        }
        else if (collision.CompareTag("bomb"))
        {
            if (isholdBomb) return;
            isCanTakeObj = true;
            if (Object != null) return;
            Object = collision.gameObject;
            //Debug.Log("co bom");
        }
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("bomb") || collision.CompareTag("Ruou"))
        {
            if (isholdBomb) return;
            isCanTakeObj = CheckForBombColliders();//Phải check xung quanh sau khi rời khỏi quả bomb phòng trường hợp rời bomb mà vẫn còn quả khác dưới chân
            if (isCanTakeObj) return;
            //Debug.Log("k co bom");
            Object = null;//phòng trường hợp bomb ném đi rồi ấn nhặt bomb quay về người
        }
    }

    public bool CheckForBombColliders()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.parent.position, scanRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("bomb") || (collider.CompareTag("Ruou")&&inventory.canAdd()))
            {
                //Debug.Log("quet xung quanh");
                Object = collider.gameObject;
                return true; 
            }
        }
        return false;
    }
    
    public void takeObj()
    {
        if (Object == null) return;
        if (Object.CompareTag("Ruou"))
        {
            ItemDefault itemToAdd = Object.GetComponentInParent<ItemDefault>();
            if (itemToAdd == null) return;

            if (inventory)
            {
                var items = inventory.items;
                int count = items.Count;
                UpdatePLayerTag updatePLayerTag = inventory.playertag.GetComponent<UpdatePLayerTag>();

                for (int i = 0; i < count; i++)
                {
                    if (items[i] == null)
                    {
                        items[i] = itemToAdd;
                        Object.transform.parent.gameObject.SetActive(false);

                        updatePLayerTag?.setItemtagUi(i);
                        return;
                    }
                }

                if (count >= 3) return;

                items.Add(itemToAdd);
                Object.transform.parent.gameObject.SetActive(false);

                updatePLayerTag?.setItemtagUi(items.Count - 1);
            }
        }
        //Debug.Log(Object.transform.parent?.name);
        playerAvatar thisAva = Ava.GetComponent<playerAvatar>();
        if (thisAva?.index == 5)//character 6
        {
            Object.transform.parent.GetComponentInChildren<BombController>()?.despawnBomb();
            thisAva.playerMovement.ani.SetTrigger("isTakeBomb");
            thisAva.avatarList[5].GetComponentInChildren<ShootingBullet>().bulletNum++;
            Debug.Log("+1");
            return;
        }
            Object.transform.parent.SetParent(holdBombPos);
            Object.transform.parent.localPosition = Vector3.zero;

        Rigidbody2D bombRigidbody = Object.transform.parent.GetComponent<Rigidbody2D>();
        
        if (bombRigidbody != null)
        {
            bombRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            bombRigidbody.bodyType = RigidbodyType2D.Kinematic;  // Tắt vật lý để bom không rơi
            var parentObj = Object.transform.parent.GetComponent<Collider2D>();
            if (parentObj != null)
            {
                parentObj.isTrigger = true;
                //Debug.Log("here1");
            }
        }
        else return;
        isholdBomb = true;
        var bombstate = Object.transform.parent.GetComponentInChildren<BombController>();
        if (!bombstate) return;

        //cầm là bật bomb(bombtime) if(bombtype==1)
        
    }

    public void throwObj(CallbackContext context)
    {
        if (!isholdBomb||Ava.GetComponent<playerAvatar>()?.index==5) return; // Không làm gì nếu đang không  giữ bom
        playerPowerBar.SetMaxHealth(maxThrowForce);
        if(!Object.GetComponent<Collider2D>().CompareTag("bomb")) return;
        if (context.started) // Khi người chơi bắt đầu giữ nút
        {
            holdTime = 0f; // Đặt lại thời gian giữ
            playerPowerBar.SetHealth(0);
        }
        if (context.performed) // Khi người chơi tiếp tục giữ nút
        {
            isPressButton =true;
        }
        checkPressbutton();
        if (context.canceled) // Khi người chơi thả nút
        {
            isPressButton = false;
            float throwForce = Mathf.Min(holdTime * 14f, maxThrowForce); // Tính lực ném
            if (holdTime < 0.25f) throwForce = 11f; // Không ném nếu giữ quá ngắn

            // Kiểm tra và lấy Rigidbody2D từ bom
            Rigidbody2D bombRigidbody= Object.transform.parent.GetComponent<Rigidbody2D>();
            if (bombRigidbody == null)
            {
                Debug.LogWarning("No Rigidbody2D found on the Object!");
                return;
            }

            // Kích hoạt Rigidbody2D
            bombRigidbody.bodyType = RigidbodyType2D.Dynamic;
            bombRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            // Đặt hướng ném (theo hướng nhân vật đang đối diện)
            Vector2 throwDirection = Ava.localScale.normalized;
            float heightReductionFactor = Mathf.Max(1f - throwForce / maxThrowForce, 0.8f);
            throwDirection.y *= heightReductionFactor;
            var parentObj = Object.transform.parent.GetComponent<Collider2D>();
            if (parentObj != null)
            {
                parentObj.isTrigger = false;
                //Debug.Log("here@");
            }

            // Thêm lực ném
            bombRigidbody.linearVelocity = Vector2.zero; // Reset vận tốc trước khi thêm lực
            bombRigidbody.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

            var bombani = Object.transform.parent.GetComponentInChildren<BombController>();
            bombani.isOn = true;
            if(bombani.bombType+1<3)
            {
                bombani.countdownCoroutine = bombani.StartCoroutine(bombani.CountdownAndExplode());
            }

            // Đặt bom vào Pooling
            Object.transform.parent.parent = BombSpawner.instance.PoolingObj.transform;

            // Cập nhật trạng thái
            isholdBomb = false;
            holdTime = 0f;
            playerPowerBar.SetHealth(holdTime);
            //Debug.Log($"Bomb thrown with force: {throwForce}");
        }
    }
    public void checkPressbutton()
    {
        if (isPressButton)
        {
            holdTime += Time.deltaTime * 1.2f;
            if(holdTime * 14f>=maxThrowForce) return;
            playerPowerBar.SetHealth(holdTime * 14f);
        }
        else return;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scanRadius);
    }

    public void TeleByDoor()
    {
        if (TeleDoor == null) return;
        var teledoor =TeleDoor.GetComponent<TeleportDoor>();
        teledoor.isOpened = true;
        this.transform.parent.position = teledoor.transform.position;
    }

}
