
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //Relative position between player and camera
    public Vector3 m_cameraPosition;

    //GameObeject
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //Get initial relative postion between player and camera
        m_cameraPosition = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Keep the relative position between player and camera from beginning
        transform.position = player.transform.position + m_cameraPosition;
    }
}
