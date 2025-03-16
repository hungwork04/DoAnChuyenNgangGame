using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayUI : MonoBehaviour
{
    public GameObject image1; // Đối tượng chứa ảnh 1
    public GameObject image2; // Đối tượng chứa ảnh 2
    public Button quitBtn;
    void Start()
    {
        this.gameObject.SetActive(false);
        // Đưa ảnh 1 lên phía trước ảnh 2
        image1.transform.SetSiblingIndex(0);
        image2.transform.SetSiblingIndex(1);
        quitBtn.onClick.AddListener(() => { this.gameObject.SetActive(false); });
    }

    // Hàm này có thể được gọi từ button hoặc sự kiện khác
    public void SwapOrderHowToplayer()
    {
        //int index1 = image1.transform.GetSiblingIndex();
        //int index2 = image2.transform.GetSiblingIndex();

        image1.transform.SetSiblingIndex(0);
        image2.transform.SetSiblingIndex(1);
    }
    public void SwapOrderHowToControl()
    {
        //int index1 = image1.transform.GetSiblingIndex();
        //int index2 = image2.transform.GetSiblingIndex();

        image1.transform.SetSiblingIndex(1);
        image2.transform.SetSiblingIndex(0);
    }
}
