using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSetup : MonoBehaviour
{
    public int damage;
    public int randomAdd;
    // Start is called before the first frame update
    void Start()
    {
        damage += Random.Range(0,randomAdd);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
