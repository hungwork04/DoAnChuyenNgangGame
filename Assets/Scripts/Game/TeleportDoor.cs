using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public List<Transform> otherDoors;
    public bool isOpened = false;
    public Animator ani;
    private void Awake()
    {
        if (ani == null) ani = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var player = collision.transform?.parent?.parent;
            if (player != null)
            {
                var playeraby = player.GetComponentInChildren<PlayerAby>();
                playeraby.isCanOpenDoor = true;
                playeraby.TeleDoor = otherDoors[getRandom()];
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var player = collision.transform?.parent?.parent;
            if (player != null)
            {
                var playeraby = player.GetComponentInChildren<PlayerAby>();
                playeraby.isCanOpenDoor = false;
                playeraby.TeleDoor = null;
            }
        }
    }
    private void Update()
    {
        ani.SetBool("isOpened", isOpened);
    }

    public int getRandom()
    {
        int index = Random.Range(0, 2);
        return index;
    }
}
