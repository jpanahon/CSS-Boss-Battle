using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Melee : Weapon
{
    [Header("Damage Settings")]
    [SerializeField] public int damage;
    [SerializeField] public int headshotMultiplier;


    [Header("Throw Settings")]
    [SerializeField] public bool isThrowable = false;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public GameObject projectile;
    [SerializeField] public int totalThrows;
    [SerializeField] public int force;
    [SerializeField] public double multiplier;
    [SerializeField] public int upwardForce;
    [SerializeField] public float range;
    [SerializeField] public float cooldown = 0f;
    [SerializeField] public Camera main;

    private bool readyToThrow = true;
    private int currentPower = 0;

    private int thrownDamage = 0;
    private Coroutine powering = null;

    public override void Primary()
    {
        print("test");
    }

    public override void Secondary()
    {
        if (isThrowable && readyToThrow)
        {
            readyToThrow = false;

            force = (int)Math.Ceiling(currentPower * multiplier);
            currentPower = 0;

            RaycastHit hit;

            if (Physics.Raycast(main.transform.position, main.transform.forward, out hit, range))
            {
                attackPoint.forward = main.transform.forward;
                GameObject obj = Instantiate(projectile, attackPoint);
                Rigidbody objRb = obj.GetComponent<Rigidbody>();

                // calculate direction
                Vector3 forceDirection = main.transform.forward;
                forceDirection = (hit.point - attackPoint.position).normalized;

                Vector3 forceToAdd = (forceDirection * force) + (Vector3.up * upwardForce);

                objRb.AddForce(forceToAdd, ForceMode.Impulse);

                totalThrows--;

                thrownDamage = force + damage;
            }


            Invoke(nameof(ResetThrow), cooldown);
        }
    }

    void ResetThrow()
    {
        readyToThrow = true;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (isThrowable)
        {
            readyToThrow = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (totalThrows <= 0)
        {
            readyToThrow = false;
        }

        if ((_input.ads && isThrowable) && currentPower == 0)
        {
            powering = StartCoroutine(PowerUp());
        }
        else if (!_input.ads && currentPower >= 1)
        {
            StopCoroutine(powering);
            Secondary();
            weaponStats.text = $"Power: {currentPower}%";
        }
    }


    public override void UpdateDisplay()
    {
        base.UpdateDisplay();

        weaponStats.text = $"Power: {currentPower}%";
    }

    private IEnumerator PowerUp()
    {
        if (currentPower <= 100 && _input.ads)
        {
            yield return new WaitForSeconds(1);
            currentPower += 1;
        }
        else if (currentPower > 100)
        {
            currentPower = 100;
            _input.ads = false;
            yield return null;
        }

        weaponStats.text = $"Power: {currentPower}%";
    }

    public int getThrownDamage()
    {
        return thrownDamage;
    }
}
