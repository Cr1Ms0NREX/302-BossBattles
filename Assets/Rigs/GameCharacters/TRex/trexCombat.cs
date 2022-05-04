using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trexCombat : MonoBehaviour
{
    Animator animator;
    int isBitingHash;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 100;

    public bool EnableInput = true;
    bool canAttack = true;
    float canAttackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Animator
        animator = GetComponent<Animator>();
        isBitingHash = Animator.StringToHash("isBiting");
    }

    // Update is called once per frame
    void Update()
    {
        

        if (EnableInput)
        {
            bool isBiting = animator.GetBool(isBitingHash);
            bool attackPressed = Input.GetMouseButtonDown(0);
            if (!isBiting && attackPressed)
            {
                Attack();
            }
            if (isBiting && !attackPressed)
            {

                animator.SetBool(isBitingHash, false);
                EnableInput = false;
                
                //canAttackTimer = 0;
                //if (canAttackTimer > 0)
                //{
                    canAttackTimer -= Time.deltaTime;
                    if (canAttackTimer <= 0)
                    {
                    EnableInput = true;
                    canAttackTimer = 0;
                        
                    }
                //}
                
            }
        }

        

    }

    void Attack()
    {
        // Play an attack animation 
        //bool isBiting = animator.GetBool(isBitingHash);
        //bool attackPressed = Input.GetMouseButtonDown(0);
        // If player presses the left mouse button
        //if (!isBiting && attackPressed)
        //{
        // Then set the is walking boolean to be treu
        animator.SetBool(isBitingHash, true);
        //}

        // If player is not pressing the left mouse button
        //if (isBiting && !attackPressed)
        //{
            // Then set the is walking boolean to be treu
            
        //}
        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapCapsule(attackPoint.position, attackPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<enemyHealth>().TakeDamage(attackDamage);
        }
        //canAttack = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
