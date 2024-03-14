using Assets.Scripts.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[RequireComponent(typeof(AudioSource))]
public class EnemyController : MonoBehaviour
{
    #region FIELDS
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public GameObject damageMarker;

    public ParticleSystem deathParticle;

    public GameObject xpParticle;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public int DamageToOnePoint;

    public const int damage = 1;
    public int damageAddRange;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    private UnityEvent attack;
    private UnityEvent go;
    private UnityEvent getDamage;
    private UnityEvent die;
    private bool _isAttack;




    AudioSource _audioSource;
    #endregion

    #region UNITY
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
    }
    Rigidbody rb;
    NavMeshAgent nma;
    private void Start()
    {
        nma = gameObject.GetComponent<NavMeshAgent>();
       rb =  gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.drag = PlayerStatesHolder.enemyDrag;
        nma.stoppingDistance = PlayerStatesHolder.distancetoPlayer;
        if (Time.timeScale == 1)
        {//Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
    }
    #endregion

    #region STATES
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        if (go != null) go.Invoke();
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked && _isAttack == false)
        {
            if (attack != null) attack.Invoke();
            player = GameObject.Find("Player").transform;
            player.GetComponent<PlayerStatesHolder>().TakeDamage(damage);//damage + Random.Range(-damageAddRange, damageAddRange));
            StartCoroutine(CollDownAttack());


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    #endregion

    #region ACTIONS
    private IEnumerator CollDownAttack()
    {
        
        _isAttack = true;
        yield return new WaitForSeconds(0.5f);
        _isAttack = false;

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public ParticleSystem PlayDeath()
    {
        return Instantiate(deathParticle, gameObject.transform.position+Vector3.down, Quaternion.identity);
        
/*         particle.Play();
        return particle;*/
    }

    public void TakeDamage(int damage, Transform point)
    {
        health -= damage;
        StartCoroutine(DropNumber(point, damageMarker, damage));
        if (getDamage != null) { getDamage.Invoke(); }
        if (health <= 0) { if (die != null) { die.Invoke(); } Invoke(nameof(DestroyEnemy), 0); ControllerUi.instanse.SetRedAim(); /*ControllerUi.instanse.ChangesLevelBar();*/ }
    }
    public ParticleSystem particle;
    private void DestroyEnemy()
    {
        particle = Instantiate(deathParticle, transform.position  +Vector3.down, deathParticle.transform.rotation);
        Instantiate(xpParticle, gameObject.transform.position + Vector3.down*0.5f, Quaternion.identity);
        particle.Play();
        SpawnPointController.enemy.Remove(gameObject);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Bullet")
        {
            
            _audioSource.Stop();
            _audioSource.Play();
            TakeDamage(other.transform.GetComponent<BulletSetup>().damage, other.transform);
            ControllerUi.instanse.SetWriteAim();
        }
    }
    IEnumerator DropNumber(Transform startPos, GameObject _damageMarker, int damage)
    {
        GameObject gameobject = Instantiate(_damageMarker, Camera.current.WorldToScreenPoint(startPos.position), Quaternion.identity);
        gameobject.transform.parent = GameObject.Find("Canvas").transform;
        gameobject.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        gameobject.GetComponent<NumberDamageIndicator>().SetStartPos(startPos.position);
        yield return new WaitForSeconds(1);
        Destroy(gameobject);
    }
    #endregion
}
