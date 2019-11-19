using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 velocity = Vector3.zero;
    private float TimeTillDeath = 10f;
    public MyAgentsScript myParent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeTillDeath -= Time.deltaTime;
        if (TimeTillDeath < 0f)
        {
           // myParent.AddReward(-0.1f);
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.tag == "Player" && other.gameObject.transform.parent.GetComponent<MyAgentsScript>() != myParent)
            {
                Destroy(gameObject);
                other.gameObject.transform.parent.gameObject.GetComponent<MyAgentsScript>().Done();
               // other.gameObject.transform.parent.gameObject.GetComponent<MyAgentsScript>().AddReward(-2);
                if (myParent != null)
                {
              //      myParent.AddReward(3);
                }
            }
            if (other.gameObject.tag == "wall")
            {
                Destroy(gameObject);
                if (myParent != null)
                {
               //     myParent.AddReward(-0.1f);
                }
            }
            if (other.gameObject.tag == "obstacles")
            {
                Destroy(gameObject);
                if (myParent != null)
                {
               //     myParent.AddReward(-0.1f);
                }
                if (other.transform.parent.gameObject.GetComponent<ObstacleScript>() != null)
                {
                    other.transform.parent.gameObject.GetComponent<ObstacleScript>().HitByBullet(myParent);
                }
            }
        }        
       
    }
}
