using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OnColliderEnterDeath death = other.GetComponent<OnColliderEnterDeath>();

        if (death != null)
        {
            death.enemy.Dead(transform.position);
        }
    }
}
