
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PShootingController : MonoBehaviour
{
    public int DamagePlayer;

    public bool AddBulletSpread = true;

    public int damage;

    public Vector3 BulletSpreadVariance = new Vector3(0f, 0f, 0f);
    public Transform directionPoint;
    public ParticleSystem ShootingSystem;

    public Transform BulletSpawnPoint;

    public ParticleSystem ImpactParticleSystem;

    public TrailRenderer BulletTrail;

    public GameObject weapon;

    public LayerMask Mask;

    public float BulletSpeed = 100;

    float ShootDelay;
    float reloadTimeBuffer = -3;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) reloadTimeBuffer = Time.realtimeSinceStartup;
        if (gameObject.GetComponent<PMoveController>().weapon.ammunitionCurrent == 0) 
        {

            _character.PlayReloadAnimation();

            gameObject.GetComponent<PMoveController>().weapon.FillAmmunition(10);
            reloadTimeBuffer = Time.realtimeSinceStartup;

        }
        //Debug.Log(_character.characterAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
    }
    
    public AudioSource source;

    private float LastShootTime;
    private Character _character;
    private void Awake()
    {
        _character = GetComponent<Character>();
        ShootDelay = 60/weapon.GetComponent<Weapon>().GetRateOfFire();
    }

    public void Shoot()
    {
        
        if (LastShootTime + ShootDelay < Time.time && gameObject.GetComponent<PMoveController>().weapon.ammunitionCurrent >0&& Time.realtimeSinceStartup - reloadTimeBuffer >= 2.0375)
        {


            //Animator.SetBool("IsShooting", true);
            ShootingSystem.Play();
            //source.Play();
            Vector3 direction = GetDirection();

            if (Physics.Raycast(BulletSpawnPoint.position, direction-BulletSpawnPoint.position, out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
                trail.GetComponent<BulletSetup>().damage = damage;
                trail.GetComponent<BulletSetup>().randomAdd = DamagePlayer;
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                LastShootTime = Time.time;
            }

            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
                trail.GetComponent<BulletSetup>().damage = damage;
                StartCoroutine(SpawnTrail(trail, directionPoint.position+(directionPoint.position-BulletSpawnPoint.position)*100f, Vector3.zero, false));

                LastShootTime = Time.time;
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = directionPoint.position;
        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );
            direction.Normalize();
            
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {

        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        //Animator.SetBool("IsShooting", false);
        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
           
           ParticleSystem impact = Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
           

        }

        Destroy(Trail.gameObject, Trail.time);
    }
}
