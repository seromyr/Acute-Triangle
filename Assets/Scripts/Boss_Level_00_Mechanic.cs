using System;
using System.Collections;
using UnityEngine;
using Constants;

public class Boss_Level_00_Mechanic : MonoBehaviour
{
    //private int count;

    //private float time;

    private MeshRenderer mr;
    private SphereCollider sc;

    private ParticleSystem takeDamageFX, deathFX;

    [SerializeField]
    private GameObject cube1, cube2;

    private Shooter shooter1, shooter2;

    public event EventHandler OnBulletHit;

    void Start()
    {
        takeDamageFX = transform.Find("DamageParticle").GetComponent<ParticleSystem>();
        deathFX = transform.Find("ExplodeParticle").GetComponent<ParticleSystem>();
        //cube = transform.Find("Cube").gameObject;

        TryGetComponent(out mr);
        TryGetComponent(out sc);
        //cube1.TryGetComponent(out shooter1);
        //cube2.TryGetComponent(out shooter2);


        //shooter1 = cube1.GetComponent<Shooter>();
        //shooter2 = cube2.GetComponent<Shooter>();
    }

    //void Update()
    //{
    //    // Boss health monitor
    //    if (count == GeneralConst.BOSS_00_HEALTH)
    //    {
    //        count = 0;
    //        time = Time.time;

    //        mr.enabled = false;
    //        sc.enabled = false;

    //        shooter1.enabled = false;
    //        shooter2.enabled = false;

    //        cube1.SetActive(false);
    //        cube2.SetActive(false);

    //        deathFX.Play(true);
    //    }

    //    if (Time.time >= time + 4)
    //    {
    //        mr.enabled = enabled;
    //        sc.enabled = enabled;

    //        shooter1.enabled = true;
    //        shooter2.enabled = true;

    //        cube1.SetActive(true);
    //        cube2.SetActive(true);
    //    }
    //}

    // Take damage from player
    private void OnParticleCollision(GameObject other)
    {
        takeDamageFX.Play();
        //count++;
        //Debug.Log(count);

        // Fire up the bullet hit event
        OnBulletHit?.Invoke(this, EventArgs.Empty);
    }

    public void PlayExplosionFX()
    {
        takeDamageFX.gameObject.SetActive(false);
        deathFX.Play(true);
        mr.enabled = false;
        if (sc != null)
        {
            sc.enabled = false;
        }

        //if (bc != null)
        //{
        //    bc.enabled = false;
        //}

        if (transform.childCount > 2)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void KillSelf(float delay)
    {
        StartCoroutine(DestroySelfWithDelay(delay));
    }

    IEnumerator DestroySelfWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
