using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public enum States { idle,moving,hitStun,shocking,attacking}
    public States state = States.idle;
    public bool dead = false;
    public bool hitStun = false;
    public bool boss;
    public AudioSource audioSource;
    public bool friend = false;
    public Transform target;

    public float maxDrainEnergy = 10;
    public float energyLeft = 10;

    public bool visible = true;
    public GameObject bullet;
    public GameObject key;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetState(States newState)
    {
        state = newState;
    }

    public void EndKill()
    {

    }

    public void Fire()
    {
        //Debug.Log("Fire");

        GameObject fireEffect = GameObject.Instantiate(EffectsManager.getInstance().shootEffect);
        fireEffect.transform.position= transform.position + 0.3f * Vector3.up+0.2f*transform.forward;
        fireEffect.transform.forward = transform.forward;

        GameObject thisBullet= GameObject.Instantiate(bullet);
        thisBullet.transform.position = transform.position + 0.3f * Vector3.up;
        thisBullet.transform.forward = transform.forward;
        thisBullet.GetComponent<DamageArea>().friend = friend;
    }

    public virtual void DealDamage(float val)
    {
        energyLeft -= val;
        Debug.Log(energyLeft);
        if(energyLeft <= 0)
        {
            dead = true;
            if(boss)
                Instantiate(key, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z ), Quaternion.identity);
        }
            

        

    }
}
