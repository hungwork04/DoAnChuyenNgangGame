using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTeleportHole : MonoBehaviour
{
    public List<Transform> otherDoors;
    public GameObject GreenHole;
    public GameObject hole1;
    public GameObject hole2;

    private float openDuration = 15f;  // Thời gian lỗ mở
    private float closeDuration = 10f; // Thời gian lỗ đóng
    private Coroutine holeCycleCoroutine;

    public void Start()
    {
        // Bắt đầu chu kỳ mở/đóng lỗ
        holeCycleCoroutine = StartCoroutine(HoleCycle());
    }

    // Coroutine để quản lý chu kỳ mở/đóng lỗ
    private IEnumerator HoleCycle()
    {
        while (true)
        {
            // Tạo lỗ mới ở vị trí ngẫu nhiên
            GetRandomPos();

            // Lỗ mở trong 15 giây
            yield return new WaitForSeconds(openDuration);

            // Chạy animation đóng trước khi ẩn lỗ
            float closeAnimationDuration = 0.5f; // Thời gian của animation đóng

            // Kiểm tra xem đối tượng có đang hoạt động không trước khi gọi coroutine
            if (hole1 != null && hole1.activeInHierarchy && gameObject.activeInHierarchy)
            {
                try
                {
                    StartCoroutine(hole1.GetComponent<GreenHole>().CloseAndHide(closeAnimationDuration));
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Không thể bắt đầu coroutine CloseAndHide cho hole1 trong HoleCycle: " + e.Message);
                    // Nếu không thể bắt đầu coroutine, vô hiệu hóa trực tiếp
                    hole1.SetActive(false);
                }
            }

            if (hole2 != null && hole2.activeInHierarchy && gameObject.activeInHierarchy)
            {
                try
                {
                    StartCoroutine(hole2.GetComponent<GreenHole>().CloseAndHide(closeAnimationDuration));
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Không thể bắt đầu coroutine CloseAndHide cho hole2 trong HoleCycle: " + e.Message);
                    // Nếu không thể bắt đầu coroutine, vô hiệu hóa trực tiếp
                    hole2.SetActive(false);
                }
            }

            // Đợi 10 giây trước khi tạo lỗ mới
            yield return new WaitForSeconds(closeDuration);
        }
    }

    public void GetRandomPos(){
        // Ẩn lỗ cũ nếu còn tồn tại, với animation đóng
        float closeAnimationDuration = 0.5f; // Thời gian của animation đóng

        // Chọn vị trí ngẫu nhiên
        int pos1 = Random.Range(0, otherDoors.Count);
        int pos2 = Random.Range(0, otherDoors.Count);
        while(pos1 == pos2)
        {
            pos2 = Random.Range(0, otherDoors.Count);
        }

        if(GreenHole==null) return;

        // Ẩn lỗ cũ nếu đang hiển thị
        if (hole1 != null && hole1.activeInHierarchy && gameObject.activeInHierarchy)
        {
            try
            {
                StartCoroutine(hole1.GetComponent<GreenHole>().CloseAndHide(closeAnimationDuration));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Không thể bắt đầu coroutine CloseAndHide cho hole1 trong GetRandomPos: " + e.Message);
                // Nếu không thể bắt đầu coroutine, vô hiệu hóa trực tiếp
                hole1.SetActive(false);
            }
        }

        if (hole2 != null && hole2.activeInHierarchy && gameObject.activeInHierarchy)
        {
            try
            {
                StartCoroutine(hole2.GetComponent<GreenHole>().CloseAndHide(closeAnimationDuration));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Không thể bắt đầu coroutine CloseAndHide cho hole2 trong GetRandomPos: " + e.Message);
                // Nếu không thể bắt đầu coroutine, vô hiệu hóa trực tiếp
                hole2.SetActive(false);
            }
        }

        // Tạo lỗ mới hoặc tái sử dụng lỗ cũ
        if (hole1 == null)
        {
            hole1 = Instantiate(GreenHole, otherDoors[pos1].position, Quaternion.identity);
        }
        else
        {
            // Đặt lại vị trí và kích hoạt lại
            hole1.transform.position = otherDoors[pos1].position;
            hole1.GetComponent<GreenHole>().Reactivate();
        }

        if (hole2 == null)
        {
            hole2 = Instantiate(GreenHole, otherDoors[pos2].position, Quaternion.identity);
        }
        else
        {
            // Đặt lại vị trí và kích hoạt lại
            hole2.transform.position = otherDoors[pos2].position;
            hole2.GetComponent<GreenHole>().Reactivate();
        }

        // Liên kết hai lỗ với nhau
        hole1.GetComponent<GreenHole>().otherHole = hole2.transform;
        hole2.GetComponent<GreenHole>().otherHole = hole1.transform;
    }

    // Phương thức để dừng chu kỳ (nếu cần)
    public void StopHoleCycle()
    {
        if (holeCycleCoroutine != null)
        {
            StopCoroutine(holeCycleCoroutine);
            holeCycleCoroutine = null;
        }

        // Hủy lỗ hiện tại với animation đóng
        float closeAnimationDuration = 0.5f; // Thời gian của animation đóng

        // Kiểm tra xem đối tượng có đang hoạt động không trước khi gọi coroutine
        if (hole1 != null && hole1.activeInHierarchy && gameObject.activeInHierarchy)
        {
            try
            {
                StartCoroutine(hole1.GetComponent<GreenHole>().CloseAndHide(closeAnimationDuration));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Không thể bắt đầu coroutine CloseAndHide cho hole1: " + e.Message);
                // Nếu không thể bắt đầu coroutine, vô hiệu hóa trực tiếp
                hole1.SetActive(false);
            }
        }

        if (hole2 != null && hole2.activeInHierarchy && gameObject.activeInHierarchy)
        {
            try
            {
                StartCoroutine(hole2.GetComponent<GreenHole>().CloseAndHide(closeAnimationDuration));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Không thể bắt đầu coroutine CloseAndHide cho hole2: " + e.Message);
                // Nếu không thể bắt đầu coroutine, vô hiệu hóa trực tiếp
                hole2.SetActive(false);
            }
        }
    }

    // Phương thức để khởi động lại chu kỳ (nếu cần)
    public void RestartHoleCycle()
    {
        StopHoleCycle();
        holeCycleCoroutine = StartCoroutine(HoleCycle());
    }

    private void OnDestroy()
    {
        // Dừng coroutine mà không gọi StopHoleCycle để tránh lỗi
        if (holeCycleCoroutine != null)
        {
            StopCoroutine(holeCycleCoroutine);
            holeCycleCoroutine = null;
        }

        // Không gọi các coroutine mới khi đối tượng đang bị hủy
        // Chỉ vô hiệu hóa các lỗ nếu chúng vẫn tồn tại
        if (hole1 != null)
        {
            // Vô hiệu hóa trực tiếp thay vì qua coroutine
            hole1.SetActive(false);
        }

        if (hole2 != null)
        {
            // Vô hiệu hóa trực tiếp thay vì qua coroutine
            hole2.SetActive(false);
        }
    }
}
