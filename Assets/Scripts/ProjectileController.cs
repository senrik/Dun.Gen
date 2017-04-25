using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

    public GameObject marker;

    private bool rested = true;
    private float damage = 0.0f;

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(c.collider, GetComponent<Collider>());
        }
        else
        {
            //Instantiate(marker, transform.position, Quaternion.identity);
            if(c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<EnemyStats>().Health -= damage;
            }
            GetComponent<Rigidbody>().Sleep();
            gameObject.SetActive(false);
        }
        
    }


    void OnDisable()
    {
        rested = false;
    }

    public bool Rested
    {
        get { return rested; }
        set { rested = value; }
    }

    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}
