using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask playerLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 100;

    public bool EnableInput = true;
    bool canAttack = true;
    float canAttackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        // Detect player in range of attack
        Collider[] hitPlayer= Physics.OverlapCapsule(attackPoint.position, attackPoint.position, attackRange, playerLayers);

        // Damage them
        foreach (Collider player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            //GetComponent<PlayerHealth>().ReciveHealth(100);
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
