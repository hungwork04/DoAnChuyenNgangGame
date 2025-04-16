using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UpdatePLayerTag : MonoBehaviour
{
    public GameObject thisPlayer;
    public TextMeshProUGUI plyerName;
    public TextMeshProUGUI skillQ;
    public TextMeshProUGUI skillR;
    public List<Sprite> playerfaces;
    public Transform posAva;
    private void Start()
    {
        if (thisPlayer)
        {
            plyerName.text = thisPlayer.gameObject.name;
            var playerAva = thisPlayer?.GetComponentInChildren<playerAvatar>();

            if (playerAva)
            {
                // Tạo Image mới từ prefab hoặc runtime
                GameObject imgGO = new GameObject("Avatar", typeof(RectTransform), typeof(Image));
                imgGO.transform.SetParent(posAva, false); // Đặt con của posAva (thuộc canvas)

                // Gán sprite vào Image component
                var image = imgGO.GetComponent<Image>();
                image.sprite = playerfaces[playerAva.index];

                // Set vị trí nếu cần (trong hệ tọa độ UI)
                RectTransform rt = imgGO.GetComponent<RectTransform>();
                rt.anchoredPosition = Vector2.zero; // hoặc gán vị trí khác nếu bạn muốn
                rt.sizeDelta = new Vector2(40, 120);       // Kích thước ảnh, bạn có thể thay đổi
                rt.localScale = Vector3.one;                // Đảm bảo không bị phóng to / thu nhỏ
            }
        }
    }
    private void Update()
    {
        if (thisPlayer)
        {
            var skillMNG = thisPlayer?.GetComponentInChildren<CharacterSkillManager>();
            if (skillMNG != null) {
                skillQ.text = ((int)skillMNG.currCooldownTimers[0]).ToString();
                skillR.text = ((int)skillMNG.currCooldownTimers[1]).ToString();
            }
            
        }
    }

    public List<Sprite> Itemface;
    public List<GameObject> Itemstag;
    public Sprite EmptyItem;
    public void setItemtagUi( int index)
    {
        var inventory = thisPlayer.GetComponentInChildren<PlayerInventory>();
        if (inventory != null)
        {
            //for (int i = 0; i < inventory.items.Count; i++)//duyệt tất cả, set lại tất cả mỗi khi gọi
            //{
                var item = inventory.items[index];
                if (item != null)
                {
                    var thisType = (int)item.type - 1;
                    Itemstag[index].GetComponent<Image>().sprite = Itemface[thisType];
                }
                else
                {
                    Debug.Log(index);
                }
           // }
        }
    }
    public void setDefaulttag(int index)
    {
        var inventory = thisPlayer.GetComponentInChildren<PlayerInventory>();
        if (inventory != null)
        {
            var item = inventory.items[index];
               Itemstag[index].GetComponent<Image>().sprite =EmptyItem;
        }
    }
}
