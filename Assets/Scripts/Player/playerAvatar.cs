using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAvatar : MonoBehaviour
{
    public List<GameObject> avatarList = new List<GameObject>();
    public PlayerMovement playerMovement;
    public PlayerJoinManager playerJM;
    public int index = 1;
    public Color plcl = Color.white;
    private void Awake()
    {
        playerMovement = this.transform.parent.GetComponentInChildren<PlayerMovement>();
        playerJM = FindAnyObjectByType<PlayerJoinManager>();
        foreach (Transform ava in this.transform)
        {
            avatarList.Add(ava.gameObject);
            ava.gameObject.SetActive(false);
        }
        setAva(index);
    }
    public void setAva(int so)//chon nv
    {
        if (so >= avatarList.Count) return;
        for (int i = 0; i < avatarList.Count; i++)
        {
            if (i == so)
            {
                this.index= so;
                avatarList[so].gameObject.SetActive(true);
                playerMovement.impactOnPlayer = avatarList[so].GetComponent<ImpactOnPlayer>();
                playerMovement.ani = avatarList[so].GetComponent<Animator>();
                if (avatarList[so].gameObject.GetComponent<SpriteRenderer>())
                {
                    //if(plcl != Color.white)
                    avatarList[so].gameObject.GetComponent<SpriteRenderer>().color = plcl;
                    //Debug.Log(plcl);
                }
            }
            else
            {
                avatarList[i].gameObject.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {
        if (playerJM != null)
        {
            playerJM.danhsach.Add(this.transform.parent.gameObject);
            //this.transform.parent.SetParent(playerJM.transform);
        }
    }
}
