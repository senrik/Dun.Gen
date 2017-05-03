using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {

    public float maxHealth = 20;

    private float health;
    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        health = Mathf.Clamp(health, 0, maxHealth);
	}

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Bullet")
        {
            GameObject tempBullet = collision.transform.gameObject;

            health -= tempBullet.GetComponent<ProjectileController>().Damage;
        }
    }
}
