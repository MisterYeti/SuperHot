using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    public Transform playerTarget;
    public Transform playerHead;
    public float stopDistance = 5.0f;
    public FireBulletOnActivate gun;

    private Rigidbody[] rigidbodies = null;

    private Quaternion localRotationGun;

    private void Start()
    {
        localRotationGun = gun.spawnPoint.localRotation;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        SetupRagdoll();
    }

    private void Update()
    {
        _agent.SetDestination(playerTarget.position);

        float distance = Vector3.Distance(playerTarget.position, transform.position);

        if (distance < stopDistance)
        {
            _agent.isStopped = true;
            _animator.SetBool("Shoot", true);
        }
    }

    public void ShootEnemy()
    {
        Vector3 playerHeadPosition = playerHead.position - Random.Range(0,0.4f) * Vector3.up;

        gun.spawnPoint.forward = (playerHeadPosition - gun.spawnPoint.position).normalized;

        gun.FireBullet();
    }

    public void ThrowGun()
    {
        gun.spawnPoint.localRotation = localRotationGun;

        gun.transform.parent = null;
        Rigidbody rb = gun.GetComponent<Rigidbody>();
        rb.velocity = BallisticVelocityVector(gun.transform.position, playerHead.position, 45);
        rb.angularVelocity = Vector3.zero;
    }

    private Vector3 BallisticVelocityVector(Vector3 source, Vector3 target, float angle)
    {
        Vector3 direction = target - source;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        //calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sign(2 * a));
        return velocity * direction.normalized;
    }

    public void SetupRagdoll()
    {
        foreach (var item in rigidbodies)
        {
            item.isKinematic = true;
        }
    }

    public void Dead(Vector3 hitPosition)
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        foreach (Collider collider in Physics.OverlapSphere(hitPosition,0.3f))
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(1000, hitPosition, 0.3f);
            }
        }

        ThrowGun();
        _animator.enabled = false;
        _agent.enabled = false;
        this.enabled = false;
    }

}
