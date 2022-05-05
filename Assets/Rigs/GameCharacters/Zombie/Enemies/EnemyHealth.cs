using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public HealthBar healthBar;
    Animator animator;
    int isDeadHash;

    public void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        isDeadHash = Animator.StringToHash("isDead");
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet")
        {
            TakeDamage(40);
        }
        if (collision.tag == "Player")
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 5)
        {
            Die();
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            IsDead();
        }
        healthBar.SetHealth(currentHealth);
    }

    public void IsDead()
    {
        if (isDead == true)
        {
            Destroy(gameObject);
        }
    }

    void Die()
    {
        //Debug.Log("Enemy died");
        // Die animation
        bool isDead = animator.GetBool(isDeadHash);
        if (!isDead)
        {
            animator.SetBool(isDeadHash, true);
        }
        if (isDead)
        {
            animator.SetBool(isDeadHash, false);
        }
    }
}
