using UnityEngine;

public class HitMonitor : MonoBehaviour
{
    private int count;

    private float time;

    private MeshRenderer mr;
    private SphereCollider sc;

    private ParticleSystem takeDamageFX, deathFX;

    [SerializeField]
    private GameObject cube1, cube2;

    private Shooter shooter1, shooter2;

    void Start()
    {
        takeDamageFX = transform.Find("DamageParticle").GetComponent<ParticleSystem>();
        deathFX = transform.Find("ExplodeParticle").GetComponent<ParticleSystem>();
        //cube = transform.Find("Cube").gameObject;

        mr = GetComponent<MeshRenderer>();
        sc = GetComponent<SphereCollider>();

        shooter1 = cube1.GetComponent<Shooter>();
        shooter2 = cube2.GetComponent<Shooter>();
    }

    void Update()
    {
        if (count == 50)
        {
            count = 0;
            time = Time.time;

            mr.enabled = false;
            sc.enabled = false;

            shooter1.enabled = false;
            shooter2.enabled = false;

            cube1.SetActive(false);
            cube2.SetActive(false);

            deathFX.Play(true);
        }

        if (Time.time >= time + 4)
        {
            mr.enabled = enabled;
            sc.enabled = enabled;

            shooter1.enabled = true;
            shooter2.enabled = true;

            cube1.SetActive(true);
            cube2.SetActive(true);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        takeDamageFX.Play();
        count++;
        //Debug.Log(count);
    }
}
