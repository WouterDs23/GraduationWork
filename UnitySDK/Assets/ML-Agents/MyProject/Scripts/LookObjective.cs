using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LookObjective : MonoBehaviour
{
    public float WalkRadius = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newLocation(GameObject ground)
    {
        int Max = 0;
        Collider myCollider = ground.GetComponent<Collider>();
        if (myCollider != null)
        {
            Vector3 finalPosition = Vector3.zero;
            do
            {
                Vector3 randomDirection = Random.insideUnitSphere * WalkRadius;
                randomDirection += ground.transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, WalkRadius, 1);
                finalPosition = hit.position;


                if (ground.transform.position.x + (ground.transform.localScale.x / 2f) > finalPosition.x && ground.transform.position.x - (ground.transform.localScale.x / 2f) < finalPosition.x)
                {
                    if (ground.transform.position.z + (ground.transform.localScale.z / 2f) > finalPosition.z && ground.transform.position.z - (ground.transform.localScale.z / 2f) < finalPosition.z)
                    {
                        finalPosition.y = transform.position.y;
                        transform.position = finalPosition;
                        return; 
                    }
                }
                Max++;
            }
            while (Max < 10);
        }       
    }
}
