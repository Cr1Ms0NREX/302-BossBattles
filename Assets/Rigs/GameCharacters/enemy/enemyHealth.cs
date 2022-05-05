using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    Animator animator;
    public int maxHealth = 100;
    int currentHealth; 
    int isDeadHash;

    // Start is called before the first frame update
    void Start()
    {
        
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        isDeadHash = Animator.StringToHash("isDead");

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
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
