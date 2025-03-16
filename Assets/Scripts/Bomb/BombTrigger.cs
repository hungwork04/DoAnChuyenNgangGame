using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrigger : MonoBehaviour
{
    public BombController bombController;
    private void Awake()
    {
       if(bombController==null) bombController = transform.parent.GetComponentInChildren<BombController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (bombController.isOn)
            {
                //Debug.Log("bomblife==0");
                //bombController.bombLife = 0;
                //bombController.currentTime = 0;
                bombController.StopAllCoroutines();
                bombController.Explose();
            }

        }
    }
}
