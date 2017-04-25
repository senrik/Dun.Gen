using UnityEngine;
using System.Collections;

[System.Serializable]
public class Weapon : Pickup{

    public string weaponType;
    public float fireRate, magSize, reloadTime, projectileSpeed, projectileLifespan, damage;
    public AudioClip shootSoundEffect;
    public Vector3 maxRecoil, recoil;
    public float recoilSmoothing;
    public float minConsecutiveShots;


    public Weapon() : base()
    {
        Type = PickupType.Weapon;
        weaponType = "";
        fireRate = 0;
        magSize = 0;
        reloadTime = 0;
        projectileSpeed = 0;
        projectileLifespan = 0;
        shootSoundEffect = new AudioClip();
        recoil = new Vector3();
        
    }

    /// <summary>
    /// Constructor for creating a new weapon with specified stats.
    /// </summary>
    /// <param name="t">Type of the weapon</param>
    /// <param name="fr">Fire rate of the weapon.</param>
    /// <param name="ms">Magazine/Clip size of the weaopn.</param>
    /// <param name="rt">Reload time of the weapon.</param>
    /// <param name="ps">Projectile speed of the weapon.</param>
    /// <param name="pl">Projectile lifespan of the weapon.</param>
    /// <param name="d">Damage the weapon deals per shot.</param>
    public Weapon(string t, float fr, float ms, float rt, float ps, float pl, float d) : base()
    {
        Type = PickupType.Weapon;
        weaponType = t;
        fireRate = fr;
        magSize = ms;
        reloadTime = rt;
        projectileSpeed = ps;
        projectileLifespan = pl;
        damage = d;
        shootSoundEffect = new AudioClip();
        recoil = new Vector3();
        recoilSmoothing = 1;
    }
    /// <summary>
    /// Constructor for creating a new weapon from a stats array.
    /// </summary>
    /// <param name="t">Type of the weapon.</param>
    /// <param name="stats">And array of type float with the stats of the weapon. fire rate @ 0 magazine size @ 1 reload time @ 2 projectile speed @ 3 projectile lifespan @ 4 damage @ 5</param>
    public Weapon(string t, float[] stats) : base()
    {
        Type = PickupType.Weapon;
        weaponType = t;
        if (stats.Length != 6)
        {
            throw new System.Exception("Stats array not correct size. Expected: 5 got: " + stats.Length);
        }
        else
        {
            fireRate = stats[0];
            magSize = stats[1];
            reloadTime = stats[2];
            projectileSpeed = stats[3];
            projectileLifespan = stats[4];
            damage = stats[5];
        }

        shootSoundEffect = new AudioClip();
        recoil = new Vector3();
        recoilSmoothing = 1;
    }
    /// <summary>
    /// Constructor for creating a new weapon from a stats array and AudioClip.
    /// </summary>
    /// <param name="t">Type of the weapon.</param>
    /// <param name="stats">And array of type float with the stats of the weapon. fire rate @ 0 magazine size @ 1 reload time @ 2 projectile speed @ 3 projectile lifespan @ 4 damage @ 5</param>
    /// <param name="shootSound">AudioClip of the weapon's shooting sound.</param>
    public Weapon(string t, float[] stats, AudioClip shootSound) : base()
    {
        Type = PickupType.Weapon;
        weaponType = t;
        if (stats.Length != 6)
        {
            throw new System.Exception("Stats array not correct size. Expected: 5 got: " + stats.Length);
        }
        else
        {
            fireRate = stats[0];
            magSize = stats[1];
            reloadTime = stats[2];
            projectileSpeed = stats[3];
            projectileLifespan = stats[4];
            damage = stats[5];
        }
        shootSoundEffect = shootSound;
        recoilSmoothing = 1;
    }

    public Weapon(string t, float[] stats, AudioClip shootSound, Vector3 r) : this(t, stats, shootSound)
    {
        recoil = r;
        recoilSmoothing = 1;
    }

    public float DPS
    {
        get { return (damage * magSize) / (magSize * fireRate + reloadTime); }
    }
}
