using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public HealthBar healthBar;
    public PlayerPowerBar powerBar;
    public PlayerMovement playerMovement;
    public playerAvatar plerAva;
    public PlayerInviciableEff playerInviEff;

    public Collider2D playerCollider;
    //public playerAvatar plyerAva;
    public bool isInvincible = false;
    public bool IsDead=false;

    private void Awake()
    {
        if(playerMovement==null) playerMovement = GetComponentInChildren<PlayerMovement>();
        if(plerAva==null) plerAva = GetComponentInChildren<playerAvatar>();
        if(playerInviEff==null) playerInviEff =GetComponentInChildren<PlayerInviciableEff>();
    }
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        plerAva = this.GetComponentInChildren<playerAvatar>();
        playerCollider = plerAva.avatarList[plerAva.index].GetComponent<Collider2D>();
        IsDead = false;
    }
    public bool wasInviciable= false;
    public void takeDame(float Damage)
    {
        if (currentHealth<=0) return;
        if (isInvincible) return;
        currentHealth -= Damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <=50 && isInvincible == false && wasInviciable==false )
        {
            StartCoroutine(temporaryIgnoreCollision());
            playerInviEff.ActivateInvincibility();
        }
    }
    public void doBuffHeal(float healValue)
    {
        StartCoroutine(BuffOverTime(healValue));
    }
    public IEnumerator BuffOverTime(float healValue)
    {
        float allHeal = healValue;

        while (allHeal > 0)
        {
            if (currentHealth <= 0) yield break;

            float healAmount = Mathf.Min(2f, allHeal);
            currentHealth += healAmount;

            if (currentHealth >= 100)
            {
                currentHealth = 100;
                allHeal = 0; 
            }
            else
            {
                allHeal -= healAmount;
            }

            healthBar.SetHealth(currentHealth);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void Update()
    {
        if (currentHealth <= 0)
        {
            playerMovement.ani.SetBool("isDead", true);
            playerMovement.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(false);
            powerBar.gameObject.SetActive(false);
            plerAva.avatarList[plerAva.index].gameObject.GetComponent<Collider2D>().usedByEffector = true;
            IsDead = true;
        }
    }
    IEnumerator temporaryIgnoreCollision()
    {
        isInvincible = true;
        wasInviciable=true;
        playerCollider.gameObject.layer = LayerMask.NameToLayer("CantNotDead");
        List<GameObject> players = new List<GameObject>();
        foreach (GameObject p in PlayerJoinManager.instance.danhsach) { 
            players.Add(p);
        }
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i] != this.gameObject)
            {
                playerAvatar otherAva = players[i].GetComponentInChildren<playerAvatar>();
                Collider2D col2d = otherAva.avatarList[otherAva.index].GetComponent<Collider2D>();
                //Physics2D.IgnoreCollision(playerCollider, col2d, true);
            }
        }
        yield return new WaitForSeconds(5);

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != this.gameObject)
            {
                playerAvatar otherAva = players[i].GetComponentInChildren<playerAvatar>();
                Collider2D col2d = otherAva.avatarList[otherAva.index].GetComponent<Collider2D>();
                //Physics2D.IgnoreCollision(playerCollider, col2d, false);
            }
        }
        isInvincible = false;
        playerCollider.gameObject.layer = LayerMask.NameToLayer("Player");

    }
}
