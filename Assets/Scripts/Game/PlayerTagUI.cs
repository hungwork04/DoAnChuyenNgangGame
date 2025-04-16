using System.Collections.Generic;
using UnityEngine;

public class PlayerTagUI : MonoBehaviour
{
    public GameObject playerTag;
    public List<Transform> poses= new List<Transform>();
    public GameObject parentUI;
    public Transform startPos;
    private void Start()
    {
        startPos = poses[0];
        if (PlayerJoinManager.instance.danhsach.Count > 0)
        {
            for (int i = 0; i < PlayerJoinManager.instance.danhsach.Count; i++)
            {
                Vector3 newPos= new Vector3(startPos.position.x + i* 4.3f,startPos.position.y, startPos.position.z);
                GameObject newTag = Instantiate(playerTag, newPos, Quaternion.identity);

                var tag = newTag.GetComponent<UpdatePLayerTag>();
                if (tag)
                {
                    tag.thisPlayer = PlayerJoinManager.instance.danhsach[i];
                }
                PlayerJoinManager.instance.danhsach[i].GetComponentInChildren<PlayerInventory>().playertag = newTag;

                // Set parent và reset transform đúng cách
                newTag.transform.SetParent(parentUI.transform, false); // FALSE rất quan trọng để giữ đúng scale UI

                // Nếu muốn đặt vị trí theo UI layout:
                newTag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                // Hoặc nếu muốn căn theo `poses[i]`:
                newTag.GetComponent<RectTransform>().position = newPos;

            }
        }
    }
}
