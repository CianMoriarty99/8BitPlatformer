using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayController : MonoBehaviour
{
    public Transform flag;

    void Update()
    {
        if (this.transform.position.x > flag.position.x)
            this.GetComponent<SpriteRenderer>().enabled = true;
    }
}
