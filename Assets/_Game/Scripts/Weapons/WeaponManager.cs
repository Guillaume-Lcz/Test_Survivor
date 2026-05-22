using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private readonly List<IWeapon> _weapons = new();

    public void AddWeapon(IWeapon weapon)
    {
        _weapons.Add(weapon);
    }

    public void RemoveWeapon(IWeapon weapon)
    {
        _weapons.Remove(weapon);
    }
}
