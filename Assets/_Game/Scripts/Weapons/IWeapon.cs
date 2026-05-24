public interface IWeapon
{
    void Activate();
    float Damage { get; set; }
    float FireRateBonus { get; set; }
    float ProjectileSpeed { get; set; }
    float Range { get; set; }
    int ProjectileCount { get; set; }
}
