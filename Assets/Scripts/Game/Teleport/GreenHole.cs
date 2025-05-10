using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreenHole : MonoBehaviour
{
    public Transform otherHole;
    public bool canTeleport = true;
    private float teleportDelay = 0.5f; // Thời gian ngắn để tránh teleport ngược lại
    private Animator animator; // Animator component

    // Dictionary để theo dõi người chơi đã teleport
    private static Dictionary<int, float> playerCooldowns = new Dictionary<int, float>();
    private float playerCooldownTime = 5f; // Thời gian người chơi không thể teleport lại (3 giây)

    void Awake()
    {
        // Lấy component Animator
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Chạy animation mở khi xuất hiện
        if (animator != null)
        {
            animator.Play("OpenAni");
            // Sau khi animation mở hoàn tất, chuyển sang animation stay
            StartCoroutine(SwitchToStayAnimation());
        }
    }

    // Coroutine để chuyển sang animation stay sau khi animation open hoàn tất
    private IEnumerator SwitchToStayAnimation()
    {
        // Đợi 0.6 giây (thời gian của animation OpenAni)
        yield return new WaitForSeconds(0.6f);
        // Chuyển sang animation stay
        animator.Play("stayAni");
    }

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

    // Phương thức để chạy animation đóng trước khi hủy
    public void PlayCloseAnimation()
    {
        if (animator != null)
        {
            animator.Play("ClosedAni");
        }
    }

    // Phương thức này sẽ được gọi từ GreenTeleportHole để ẩn GreenHole
    public IEnumerator CloseAndHide(float delay)
    {
        // Chạy animation đóng
        PlayCloseAnimation();

        // Đợi animation đóng hoàn tất
        yield return new WaitForSeconds(delay);

        // Vô hiệu hóa các chức năng thay vì hủy
        canTeleport = false;

        // Vô hiệu hóa collider
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        // Làm cho sprite trở nên trong suốt
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 0f; // Hoàn toàn trong suốt
            spriteRenderer.color = color;
        }

        // Vô hiệu hóa GameObject (không hiển thị nhưng vẫn tồn tại)
        gameObject.SetActive(false);
    }

    // Phương thức để kích hoạt lại GreenHole
    public void Reactivate()
    {
        // Kích hoạt lại GameObject
        gameObject.SetActive(true);

        // Kích hoạt lại collider
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        // Đặt lại màu sắc
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 1f; // Hoàn toàn hiển thị
            spriteRenderer.color = color;
        }

        // Đặt lại khả năng teleport
        canTeleport = true;

        // Chạy animation mở
        if (animator != null)
        {
            animator.Play("OpenAni");
            StartCoroutine(SwitchToStayAnimation());
        }
    }
}
