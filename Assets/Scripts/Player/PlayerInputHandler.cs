using UnityEngine;
using  UnityEngine.InputSystem;
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAby playerAby;
    private CharacterSkillManager characterSkillManager;
    private void Awake()
    {
        characterSkillManager = this.transform.parent.GetComponentInChildren<CharacterSkillManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAby = this.transform.parent.GetComponentInChildren<PlayerAby>();
    }

    // Được gọi khi người chơi di chuyển
    public void OnMove(InputAction.CallbackContext context)
    {
        if (playerMovement.impactOnPlayer.isKnockback||playerMovement.impactOnPlayer.isStunned||playerMovement.isDashing) return;
        if (playerMovement != null)
        {
            if (context.performed)
            {
                Vector2 moveInput = context.ReadValue<Vector2>();
               // Debug.Log("Move input: " + moveInput + " from: " + context.control.device);
                playerMovement?.SetMovementInput(moveInput);
            }
            else if (context.canceled)
            {
                // Dừng di chuyển khi hành động bị hủy
                playerMovement?.SetMovementInput(Vector2.zero);
            }
        }
    }

    // Được gọi khi người chơi nhảy
    public void OnJump(InputAction.CallbackContext context)
    {
        if (playerMovement.impactOnPlayer.isKnockback || playerMovement.impactOnPlayer.isStunned || playerMovement.isDashing) return;
        if (playerMovement != null && context.performed)
        {
            //if (context.performed)
            //{
                playerMovement.climbing();
            //}
            //else if (context.canceled && playerMovement.impactOnPlayer.canClimbing)
            //{
            //    playerMovement.impactOnPlayer.isClimbing = false;
            //    playerMovement.rb.gravityScale=playerMovement.impactOnPlayer.startgravityScale;
            //}
            if (playerMovement.impactOnPlayer.canClimbing) return;
            playerMovement.Jump();
        }
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (playerMovement.impactOnPlayer.isKnockback || playerMovement.impactOnPlayer.isStunned) return;
        if (playerAby != null )
        {
            playerAby.throwObj(context);
        }
    }
    public void OnPick(InputAction.CallbackContext context)
    {
        if (playerMovement.impactOnPlayer.isKnockback || playerMovement.impactOnPlayer.isStunned) return;
        if (playerAby != null && context.performed)
        {
            if (playerAby.isCanOpenDoor && !playerAby.isCanTakeBomb)
            {
                playerAby.TeleByDoor();
                return;
            }
            playerAby.takeObj();
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (playerMovement.impactOnPlayer.isKnockback || playerMovement.impactOnPlayer.isStunned) return;
        if (characterSkillManager != null && context.performed)
        {
            characterSkillManager.ActivateSkill(0);
        }
    }
    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (playerMovement.impactOnPlayer.isKnockback || playerMovement.impactOnPlayer.isStunned) return;
        if (characterSkillManager != null && context.performed)
        {
            characterSkillManager.ActivateSkill(1);
        }
    }
}
