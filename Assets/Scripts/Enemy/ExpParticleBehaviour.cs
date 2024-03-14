using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpParticleBehaviour : MonoBehaviour
{
    
    GameObject player;
    PlayerStatesHolder playerStatesHolder;
    float XpFlydistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        player = GameObject.Find("Player");
        playerStatesHolder = player.GetComponent<PlayerStatesHolder>();
        rb = gameObject.GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    Rigidbody rb;
    void Update()
        
    {
        
        
        
        XpFlydistance = playerStatesHolder.xpFlyDistance;
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < XpFlydistance) 
        {
            
           // gameObject.transform.position Vector3.Normalize(player.transform.position - gameObject.transform.position); /** Time.deltaTime * Vector3.Distance(player.transform.position, gameObject.transform.position);*/ 
        rb.velocity = Vector3.Normalize(player.transform.position - gameObject.transform.position)*10;

        }
    }
}
