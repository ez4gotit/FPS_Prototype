using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDamageIndicator : MonoBehaviour
{
    Vector3 stratPos = new Vector3(0,0,0);
    float deathTime = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.transform.position = Camera.main.WorldToScreenPoint(stratPos);
    }
/*    void Start()
    {
        gameObject.transform.position = Camera.current.WorldToScreenPoint(stratPos);
    }*/

    // Update is called once per frame
    void Update()
    {
        deathTime += Time.deltaTime;
        if (deathTime >= 2) Destroy(gameObject);
        gameObject.transform.position = Camera.main.WorldToScreenPoint(stratPos);
    }

    public void SetStartPos(Vector3 vector)
    {
        stratPos = vector;
    }
}
