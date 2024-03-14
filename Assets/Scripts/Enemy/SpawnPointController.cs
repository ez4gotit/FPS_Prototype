using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPointController : MonoBehaviour
{


    public GameObject enemyPrefab;
    public float startSpawnDelay;
    public float spawnDelay;

 

    public float standartHealth;
    float timeBuffer;
    public UnityEvent spawnEnemies;

    public GameObject ParentSpawner;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemies.AddListener(SpawnEnemies);
        timeBuffer = startSpawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(timeBuffer<=0)
        {
            if (ParentSpawner.transform.childCount < 150)
            {
                timeBuffer = spawnDelay;
                if (spawnEnemies != null) spawnEnemies.Invoke();
            }
            else
            {
                
                enemy.ToArray()[0].GetComponent<EnemyController>().PlayDeath();
                
                GameObject buff = enemy.ToArray()[0];
                enemy.Remove(buff);
                enemy.Add(buff);

                buff.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                buff.transform.position = gameObject.transform.position;
                timeBuffer = spawnDelay;
            }
        }
        timeBuffer-= Time.deltaTime;
    }

    public static List<GameObject> enemy = new List<GameObject>();
    void SpawnEnemies()
    {

        enemy.Add( Instantiate(enemyPrefab,gameObject.transform.position, Quaternion.identity, ParentSpawner.transform));
        enemy[enemy.Count-1].GetComponent<EnemyController>().health = standartHealth;
    }
}
