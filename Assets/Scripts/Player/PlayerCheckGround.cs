
using UnityEngine;

public class PlayerCheckGround : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private void Awake()
    {
        if(playerMovement==null)
        playerMovement = this.transform.parent.parent.parent.GetComponentInChildren<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grounded"))
        {
            playerMovement.isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Grounded"))
        {
            playerMovement.isGrounded = false;
        }
    }
}
