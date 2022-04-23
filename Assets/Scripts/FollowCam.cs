using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject Player;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0,Player.transform.position.y,-10);
    }
}
