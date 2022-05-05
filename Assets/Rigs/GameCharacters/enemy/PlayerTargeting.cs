using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{

    public float visionDistance = 100;

    [Range(1, 20)]
    public int roundsPerSecond = 5;

    public int health = 100;
    public int currentHealth;
    private bool isDead = false;

    public TargetableObjects target { get; private set; }
    public bool playerWantsToAim { get; private set; }
    public bool playerWantsToAttack { get; private set; }

    private List<TargetableObjects> validTargets = new List<TargetableObjects>();
    private float cooldownScan = 0;
    private float cooldownPickTarget = 0;
    private float cooldownAttack = 0;


    //private CameraControler cam;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = health;
    }

    void Update()
    {
        playerWantsToAttack = Input.GetButton("Fire1");
        playerWantsToAim = Input.GetButton("Fire2");

        cooldownScan -= Time.deltaTime;
        cooldownPickTarget -= Time.deltaTime;
        cooldownAttack -= Time.deltaTime;

        if (playerWantsToAim)
        {
            if (target != null)
            {
                Vector3 toTarget = target.transform.position - transform.position;
                toTarget.y = 0;
                if (toTarget.magnitude > 100 && !CanSeeThing(target))
                {
                    target = null;
                }
            }
            if (cooldownScan <= 0) ScanForTargets();
            if (cooldownPickTarget <= 0) PickATarget();
        }
        else
        {
            target = null;
        }
        
        DoAttack();
    }

    public void DoAttack()
    {
        if (cooldownAttack > 0) return;
        if (!playerWantsToAim) return;
        if (!playerWantsToAttack) return;
        if (target == null) return;
        if (!CanSeeThing(target)) return;

        cooldownAttack = 1f / roundsPerSecond;

        // Spawn Projectiles...
        // Damage Health

    }
    public void TakeDamage(int explosionDamage)
    {
        health -= explosionDamage;
        if (health < 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
    void ScanForTargets()
    {
        cooldownScan = .5f;
        validTargets.Clear();
        TargetableObjects[] things = GameObject.FindObjectsOfType<TargetableObjects>();
        foreach (TargetableObjects thing in things)
        {
            if (CanSeeThing(thing))
            {
                validTargets.Add(thing);
            }
        }
    }

    private bool CanSeeThing(TargetableObjects thing)
    {
        Vector3 vToThing = thing.transform.position - transform.position;

        // Is too far away see?
        if (vToThing.sqrMagnitude > visionDistance * visionDistance) return false;
        // How much is in front of player?
        float alignment = Vector3.Dot(transform.forward, vToThing.normalized);
        // is within <180 degrees of forward
        if (alignment < .4f) return false;
        // check for occlusion...
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = vToThing;
        Debug.DrawRay(ray.origin, ray.direction * visionDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, visionDistance))
        {
            bool canSee = false;
            Transform xform = hit.transform;
            do
            {
                if (xform.gameObject == thing.gameObject)
                {
                    canSee = true;
                    break;
                }
                xform = xform.parent;
            } while (xform != null);
            if (!canSee) return false;
        }
        return true;
    }

    void PickATarget()
    {
        if (target) return;
        float closestDistanceSoFar = 0;
        foreach (TargetableObjects thing in validTargets)
        {
            Vector3 vToThing = thing.transform.position - transform.position;
            float dis = vToThing.sqrMagnitude;
            if (dis < closestDistanceSoFar || target == null)
            {
                closestDistanceSoFar = dis;
                target = thing;
            }
        }
    }

}
