using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public Rigidbody2D rb;
    public float maxhigh = 64f;
    public Transform thisTrans;

    private bool canMove = false; // Chờ 1s mới được phép di chuyển

    public void loadScreenhome()
    {
        SceneManager.LoadScene(0);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        thisTrans = GetComponent<Transform>();
    }

    void Start()
    {
        Invoke(nameof(EnableMove), 1f); // Gọi hàm EnableMove sau 1 giây
    }

    void EnableMove()
    {
        canMove = true;
    }

    void Update()
    {
        if (!canMove) return;

        float distance = maxhigh - thisTrans.position.y;

        if (distance <= 0f)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            float t = Mathf.Clamp01(distance / 5f); // 5f là vùng giảm tốc
            float speed = moveSpeed * t;
            rb.linearVelocity = Vector2.up * speed;
        }
    }
}
