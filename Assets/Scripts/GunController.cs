using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunController : MonoBehaviour {

    public GameObject projectilePrefab;
    public GameObject physicsProjectilePrefab;
    public GameObject markerPrefab;
    public Transform cameraTransform;
    //public AudioClip shootSoundEffect;
    public Weapon currentWeapon;
    public AudioSource FPSAudioSource;
    public ParticleSystem muzzleFlash;
    public AmmoPanel ammoPanel;
    public Slider reloadSlider;
    //public float shotDamage = 2.0f;
   // public float fireRate = 0.5f;
    public float currentAmmoPoolCount;
    public float maxAmmoPoolCount;
    public float currentAmmoCount;
    public float maxAmmoCount = 30;
    //public Vector3 recoilAmount;
    //public Vector3 maxRecoil;
    public float minConsecutiveShots;
    //public Text recoilDisplay;
    public float recoilRecoverySmoothing;
    public Vector3 maxRecoilJitter;
    //public float reloadTime;
    public float projectileSpeed = 0.5f;
    public float projectileLifespan = 1.0f;
    public float projectileTrailTime = 1.0f;
    public bool physicsBasedProjectile;
    public Transform[] projectileSpawnPoint;

    private GameObject[] projectilePool;
    private float shootTimer;
    private System.Random rand;
    private float projectileRestTime;
    private GameObject offsetMarker;
    private bool reloading;
    private Quaternion initBarrelRot;
    private Vector3 m_recoil, tempBarrelRot;
    private int consecShots;

    void Awake() {
        projectilePool = new GameObject[100];
        
    }

    // Use this for initialization
    void Start () {
	    for(int i = 0; i < projectilePool.Length; i++) {
            if (physicsBasedProjectile)
            {
                projectilePool[i] = Instantiate<GameObject>(physicsProjectilePrefab);
            }
            else
            {
                projectilePool[i] = Instantiate<GameObject>(projectilePrefab);
            }
            projectilePool[i].GetComponent<ProjectileController>().Damage = currentWeapon.damage;
            projectilePool[i].SetActive(false);
        }
        rand = new System.Random();
        shootTimer = currentWeapon.fireRate + 1;
        projectileRestTime = 5.0f;
        reloading = false;
        BindHUD();
        offsetMarker = Instantiate<GameObject>(markerPrefab);
        offsetMarker.SetActive(false);
        
        if (cameraTransform == null)
        {
            cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        initBarrelRot = projectileSpawnPoint[0].localRotation;
        //Debug.Log("InitBarralRot: " + initBarrelRot.ToString());
        consecShots = 0;
	}

    void BindHUD()
    {
        if (GameObject.FindGameObjectWithTag("ReloadSlider"))
        {
            reloadSlider = GameObject.FindGameObjectWithTag("ReloadSlider").GetComponent<Slider>();
            reloadSlider.maxValue = currentWeapon.reloadTime;
        }

        if (GameObject.FindGameObjectWithTag("AmmoPanel"))
        {
            ammoPanel = GameObject.FindGameObjectWithTag("AmmoPanel").GetComponent<AmmoPanel>();
        }
    }

    #region PrivateMethods

    void SpawnProjectile()
    {
        foreach (GameObject go in projectilePool)
        {
            if (!go.activeSelf)
            {
                if (go.GetComponent<ProjectileController>().Rested)
                {
                    if (projectileSpawnPoint.Length < 2)
                    {
                        go.transform.position = projectileSpawnPoint[0].position;
                        go.transform.rotation = projectileSpawnPoint[0].rotation;
                        if (!physicsBasedProjectile)
                        {
                            go.transform.position -= (go.transform.forward * projectileSpeed);
                        }
                        else
                        {
                            if (go.GetComponent<Rigidbody>().IsSleeping())
                            {
                                go.GetComponent<Rigidbody>().WakeUp();
                            }
                            
                        }
                        offsetMarker.transform.position = go.transform.position - (go.transform.forward*projectileSpeed);
                        offsetMarker.SetActive(true);
                        go.GetComponent<TrailRenderer>().time = projectileTrailTime;
                        go.GetComponent<ProjectileController>().Damage = currentWeapon.damage;
                        go.SetActive(true);
                        currentAmmoCount--;
                        StartCoroutine("ResetProjectile", go);
                        break;
                    }
                    else {
                        int spawnIndex;
                        spawnIndex = rand.Next(0, projectileSpawnPoint.Length);
                        go.transform.position = projectileSpawnPoint[spawnIndex].position;
                        go.transform.rotation = projectileSpawnPoint[spawnIndex].rotation;
                        if (!physicsBasedProjectile)
                        {
                            go.transform.position -= (go.transform.forward * projectileSpeed);
                        }
                        offsetMarker.transform.position = go.transform.position;
                        offsetMarker.SetActive(true);
                        go.GetComponent<TrailRenderer>().time = projectileTrailTime;
                        go.SetActive(true);
                        StartCoroutine("ResetProjectile", go);
                        break;
                    }
                }
                else
                {

                }
            }
        }
    }

    void MuzzleFlash()
    {
        if (muzzleFlash.isPlaying)
        {
            muzzleFlash.Stop();
            muzzleFlash.Clear();
        }
        else
        {
            muzzleFlash.Play();
        }
    }

    void Recoil()
    {
        if (consecShots >= minConsecutiveShots)
        {
            //m_recoil.x = (Random.Range(-1, 1) * maxRecoilJitter.x) + currentWeapon.recoil.x;
            //m_recoil.y = (Random.Range(-1.0f, 0.0f) * maxRecoilJitter.y) + currentWeapon.recoil.y;
            //m_recoil.z = 0;
            //if (projectileSpawnPoint.Length < 2)
            //{
            //    tempBarrelRot = projectileSpawnPoint[0].localEulerAngles;
            //    if (tempBarrelRot.x < 360 - currentWeapon.maxRecoil.x && tempBarrelRot.x > 270)
            //    {
            //        tempBarrelRot.x = 360 - currentWeapon.maxRecoil.x;
            //    }
            //    if (tempBarrelRot.y > currentWeapon.maxRecoil.y && tempBarrelRot.y < 90)
            //    {
            //        tempBarrelRot.y = currentWeapon.maxRecoil.y;
            //    }
            //    projectileSpawnPoint[0].localEulerAngles = tempBarrelRot;
            //    projectileSpawnPoint[0].Rotate(-m_recoil, Space.Self);
            //}
        }
    }

    void RecoilReset()
    {
        if (projectileSpawnPoint.Length < 2)
        {
            if (projectileSpawnPoint[0].localRotation != initBarrelRot)
            {
                projectileSpawnPoint[0].localRotation = Quaternion.Lerp(projectileSpawnPoint[0].localRotation, initBarrelRot, Time.deltaTime * recoilRecoverySmoothing);
            }
            else
            {
                consecShots = 0;
            }
        }
    }
    #endregion

    #region Coroutines
    IEnumerator ResetProjectile(GameObject go)
    {
        yield return new WaitForSeconds(projectileLifespan);
        go.GetComponent<ProjectileController>().Rested = false;
        StartCoroutine("RestProjectile", go);
        if(go.tag == "PhysicsObject")
        {
            go.GetComponent<Rigidbody>().Sleep();
        }
        go.SetActive(false);
        go.transform.position = projectileSpawnPoint[0].position;
        yield break;
    }

    IEnumerator RestProjectile(GameObject go)
    {
        go.GetComponent<TrailRenderer>().time = -1;
        yield return new WaitForSeconds(projectileRestTime);
        go.GetComponent<ProjectileController>().Rested = true;
        yield break;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        if(currentAmmoPoolCount >= maxAmmoCount)
        {
            currentAmmoPoolCount -= (maxAmmoCount - currentAmmoCount);
            currentAmmoCount = maxAmmoCount;
        }
        else
        {
            currentAmmoCount = currentAmmoPoolCount;
            currentAmmoPoolCount = 0;
        }
        reloading = false;
        yield break;
    }
    IEnumerator RestRecoil()
    {
        Debug.Log("RestRcoil Start.");
        while(projectileSpawnPoint[0].localEulerAngles != Vector3.zero)
        {
            if (projectileSpawnPoint.Length < 2)
            {
                projectileSpawnPoint[0].localEulerAngles = Vector3.zero;
            }
        }
        yield break;
    }
    #endregion

    void FixedUpdate()
    {
        foreach (GameObject go in projectilePool)
        {
            if (go.activeSelf)
            {
                if (go.GetComponent<Rigidbody>().velocity.magnitude <= 0)
                {
                    go.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * projectileSpeed);
                }
            }
        }

        //projectileSpawnPoint[0].rotation = cameraTransform.rotation;
        transform.rotation = cameraTransform.rotation;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > currentWeapon.fireRate)
            {
                if(!reloading)
                {
                    if(currentAmmoCount > 0)
                    {
                        SpawnProjectile();
                        consecShots++;
                        MuzzleFlash();
                        Recoil();
                        if(FPSAudioSource.clip != currentWeapon.shootSoundEffect)
                            FPSAudioSource.clip = currentWeapon.shootSoundEffect;
                        FPSAudioSource.Play();
                        shootTimer = 0;
                    }                    
                }
            }
        }
        else
        {

            if (!reloading)
            {
                if (Input.GetButtonDown("Reload"))
                {
                    reloading = true;
                    StartCoroutine("Reload");
                }
            }
            if (offsetMarker.activeSelf)
            {
                offsetMarker.SetActive(false);
            }
            RecoilReset();

        }


        if (Input.GetMouseButtonUp(0))
        {
            shootTimer = currentWeapon.fireRate;
            if (muzzleFlash.isPlaying)
            {
                muzzleFlash.Stop();
                muzzleFlash.Clear();
            }
            //StartCoroutine(RestRecoil());
        }

        foreach (GameObject go in projectilePool)
        {
            if (go.activeSelf)
            {
                if (go.tag != "PhysicsObject")
                {
                    go.transform.position += (go.transform.forward * projectileSpeed);
                }
            }
        }

        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 200, Color.red);
        Debug.DrawRay(projectileSpawnPoint[0].position, projectileSpawnPoint[0].forward * 200, Color.blue);
        
	}

    void LateUpdate()
    {
        ammoPanel.currentAmmo.text = currentAmmoCount.ToString();
        ammoPanel.maxAmmo.text = maxAmmoCount.ToString();
        ammoPanel.ammoPool.text = currentAmmoPoolCount.ToString();

        if (reloading)
        {
            if(reloadSlider.GetComponent<CanvasGroup>().alpha <= 0)
            {
                reloadSlider.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                reloadSlider.value += Time.deltaTime;
            }
        }
        else
        {
            if (reloadSlider.GetComponent<CanvasGroup>().alpha > 0)
            {
                reloadSlider.GetComponent<CanvasGroup>().alpha = 0;
                reloadSlider.value = reloadSlider.minValue;
            }
        }

        //recoilDisplay.text = projectileSpawnPoint[0].localEulerAngles.ToString();
    }
}
