using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour
{
    public bool start;
    public float timer, t;
    public GameObject prefab;
    public Transform t1,t2,t3,t4;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start){
            if(t <= 0){
                Instantiate(prefab, t1);
                Instantiate(prefab, t2);
                Instantiate(prefab, t3);
                Instantiate(prefab, t4);
                t = timer;
            }
            t -= Time.deltaTime;
        }

        
    }


}
