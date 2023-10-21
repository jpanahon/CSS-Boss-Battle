using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] public bool canStick = false;

    private bool targetHit;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"COLLIDED WITH {collision.gameObject.name}");
        if (targetHit)
            return;
        else
            targetHit = true;

        if (canStick && targetHit)
        {
            rb.isKinematic = true;
            transform.SetParent(collision.transform);

            if (collision.gameObject.GetComponent<Damageable>() != null)
            {
                int damage = transform.gameObject.GetComponent<Melee>().getThrownDamage();
                collision.gameObject.GetComponent<Damageable>().TakeDamage(damage);
            }
        }
    }
}
