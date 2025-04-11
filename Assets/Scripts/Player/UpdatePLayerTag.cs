using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
}
