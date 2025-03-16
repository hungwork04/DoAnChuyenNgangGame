using UnityEngine;
using System.Collections.Generic;

public class DynamicCamera : MonoBehaviour
{
    #region
    public List<Transform> players; // Danh sách các người chơi
    public float minSize = 5f; // Kích thước nhỏ nhất của camera
    public float maxSize = 15f; // Kích thước lớn nhất của camera
    public float zoomSpeed = 5f; // Tốc độ zoom của camera
    public float padding = 2f; // Khoảng cách dư thừa để không cắt nhân vật
    public float fixedY = 0f; // Độ cao cố định của camera (trục Y)
    public float distanceThreshold = 10f; // Ngưỡng tối thiểu để camera lùi ra xa
    public Vector2 xLimit = new Vector2(-10f, 10f); // Giới hạn bên trái và phải trên trục X (min, max)

    private Camera cam;
    private float currentSize;

    void Start()
    {
        cam = GetComponent<Camera>();
        currentSize = cam.orthographicSize;
    }

    void LateUpdate()
    {
        if (players.Count == 0) return; // Nếu không có người chơi, không làm gì

        // Tính vị trí trung tâm giữa các người chơi
        Vector3 targetPosition = GetCenterPoint();

        // Giới hạn vị trí camera trên trục X (bên trái và phải)
        float clampedX = Mathf.Clamp(targetPosition.x, xLimit.x, xLimit.y);
        transform.position = new Vector3(clampedX, fixedY, transform.position.z);

        // Tính toán kích thước camera và giới hạn
        float greatestDistance = GetGreatestDistance();
        if (greatestDistance > distanceThreshold)
        {
            float targetSize = Mathf.Max(minSize, Mathf.Min(greatestDistance / 2f + padding, maxSize));
            currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * zoomSpeed);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, minSize, Time.deltaTime * zoomSpeed);
        }

        cam.orthographicSize = currentSize;
    }

    Vector3 GetCenterPoint()
    {
        if (players.Count == 1)
        {
            return players[0].position; // Nếu chỉ có 1 người chơi, trả về vị trí của họ
        }

        // Tính vị trí trung tâm giữa các người chơi
        var bounds = new Bounds(players[0].position, Vector3.zero);
        foreach (Transform player in players)
        {
            bounds.Encapsulate(player.position); // Mở rộng giới hạn bao gồm tất cả người chơi
        }

        return new Vector3(bounds.center.x, fixedY, 0f); // Trả về trung tâm theo trục X, cố định trục Y
    }

float GetGreatestDistance()
{
    var bounds = new Bounds(players[0].position, Vector3.zero);
    foreach (Transform player in players)
    {
        bounds.Encapsulate(player.position); // Mở rộng giới hạn để bao gồm tất cả người chơi
    }

    // Lấy khoảng cách lớn nhất giữa trục X và trục Y
    float distanceX = bounds.size.x; // Khoảng cách theo trục X
    float distanceY = bounds.size.y; // Khoảng cách theo trục Y
    return Mathf.Max(distanceX, distanceY); // Trả về khoảng cách lớn nhất
}
    #endregion

}


