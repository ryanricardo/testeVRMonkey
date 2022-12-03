using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour {

    Transform playerTransform;
    Vector3 lastPlayerPosition;
    Vector3 playerDelta;
	// Use this for initialization
	void Start () {
        playerTransform = StealthPlayerController.getInstance().transform;
        lastPlayerPosition = new Vector3(playerTransform.transform.position.x, 22.1f, playerTransform.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        playerDelta = new Vector3(playerTransform.transform.position.x, 22.1f, playerTransform.transform.position.z) - lastPlayerPosition;
        transform.position += playerDelta;
        lastPlayerPosition = new Vector3(playerTransform.transform.position.x, 22.1f, playerTransform.transform.position.z);
        //transform.position = new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.transform.position.z);
	}
}
