using UnityEngine;

public enum WeaponStat { Damage, FireRate, ProjectileSpeed, Range, ProjectileCount }

[CreateAssetMenu(fileName = "NewPerk", menuName = "Survivor/Perk")]
public class PerkSO : ScriptableObject
{
    public string perkName;
    [TextArea] public string description;
    public WeaponStat stat;
    [Tooltip("Multiplier for float stats (e.g. 1.2 = +20%). For FireRate: bonus added to accumulator (1.0 = +100% = halve cooldown).")]
    public float value = 1f;

    public void Apply(GameObject player)
    {
        var weapon = player.GetComponent<IWeapon>();
        if (weapon == null) return;

        switch (stat)
        {
            case WeaponStat.Damage:          weapon.Damage *= value; break;
            case WeaponStat.FireRate:        weapon.FireRateBonus += value; break;
            case WeaponStat.ProjectileSpeed: weapon.ProjectileSpeed *= value; break;
            case WeaponStat.Range:           weapon.Range *= value; break;
            case WeaponStat.ProjectileCount: weapon.ProjectileCount += Mathf.RoundToInt(value); break;
        }
    }
}
