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
            getRandompos();
            
            // Lỗ mở trong 15 giây
            yield return new WaitForSeconds(openDuration);
            
            // Hủy lỗ cũ
            if (hole1 != null) Destroy(hole1);
            if (hole2 != null) Destroy(hole2);
            
            // Đợi 10 giây trước khi tạo lỗ mới
            yield return new WaitForSeconds(closeDuration);
        }
    }
    
    public void getRandompos(){
        // Hủy lỗ cũ nếu còn tồn tại
        if (hole1 != null) Destroy(hole1);
        if (hole2 != null) Destroy(hole2);
        
        // Chọn vị trí ngẫu nhiên
        int pos1 = Random.Range(0, otherDoors.Count);
        int pos2 = Random.Range(0, otherDoors.Count);
        while(pos1 == pos2)
        {
            pos2 = Random.Range(0, otherDoors.Count);
        }
        
        if(GreenHole==null) return;
        
        // Tạo lỗ mới
        hole1 = Instantiate(GreenHole, otherDoors[pos1].position, Quaternion.identity);
        hole2 = Instantiate(GreenHole, otherDoors[pos2].position, Quaternion.identity);
        
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
        
        // Hủy lỗ hiện tại
        if (hole1 != null) Destroy(hole1);
        if (hole2 != null) Destroy(hole2);
    }
    
    // Phương thức để khởi động lại chu kỳ (nếu cần)
    public void RestartHoleCycle()
    {
        StopHoleCycle();
        holeCycleCoroutine = StartCoroutine(HoleCycle());
    }
    
    private void OnDestroy()
    {
        StopHoleCycle();
    }
}
