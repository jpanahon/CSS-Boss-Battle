using UnityEngine;
using StarterAssets;

public class WeaponHandler : MonoBehaviour
{
    private StarterAssetsInputs _input;

    public int selectedWeapon = 0;
    public int previousWeapon;

    // Start is called before the first frame update
    void Start()
    {
        Disarm();

        _input = transform.root.GetComponent<StarterAssetsInputs>();
        previousWeapon = selectedWeapon;
        SelectWeapon(selectedWeapon);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ActivateWeapon(Transform weapon)
    {
        weapon.gameObject.SetActive(true);

        if (weapon.TryGetComponent(out Gun gun))
        {
            gun.UpdateDisplay();
        }
        else if (weapon.TryGetComponent(out Melee melee))
        {
            melee.UpdateDisplay();
        }
    }

    void Disarm()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
    }

    public void SelectWeapon(int slot)
    {
        Disarm();

        previousWeapon = selectedWeapon;
        selectedWeapon = slot;

        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                ActivateWeapon(weapon);
                break;
            } 

            i++;
        }
    }


    public Weapon CurrentWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                if (weapon.TryGetComponent(out Gun gun))
                {
                    return gun;
                }
                else if (weapon.TryGetComponent(out Melee melee))
                {
                    return melee;
                }
            }
     
            i++;
        }

        return null;
    }

    public bool WeaponChanged()
    {
        return selectedWeapon != previousWeapon;
    }
}
