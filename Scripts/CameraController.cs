using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraController : MonoBehaviour
{
    public Transform flag1, flag2, flag3, flag4;

    public PixelPerfectCamera ppCamera;

    //static float t = 0.0f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x > flag4.position.x){
            ppCamera.assetsPPU  = (int)Mathf.Lerp(ppCamera.assetsPPU, 7f, 1f* Time.deltaTime);
            
        }

    }
}
