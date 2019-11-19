using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.AI;
using UnityEngine.UI;
public class ShooterAcademy : Academy
{
    // Start is called before the first frame update
    public float WalkRadius = 100;
    [SerializeField] private GameObject _VersionText = null;
    public GameObject Ground = null;
    public override void AcademyReset()
    {

    }

    public override void AcademyStep()
    {
        _VersionText.GetComponent<Text>().text = GetEpisodeCount().ToString();
    }

    public Vector3 GetRandomPos()
    {
        WalkRadius= Random.Range(5f, 20f);
        int Max = 0;
        if (Ground == null)
        {
            return Vector3.zero;
        }
        Collider myCollider = Ground.GetComponent<Collider>();
        if (myCollider != null)
        {
            Vector3 finalPosition = Vector3.zero;
            do
            {
                Vector3 randomDirection = Random.insideUnitSphere * WalkRadius;
                randomDirection += Ground.transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, WalkRadius, 1);
                finalPosition = hit.position;


                if (Ground.transform.position.x + (Ground.transform.localScale.x / 2f) > finalPosition.x && Ground.transform.position.x - (Ground.transform.localScale.x / 2f) < finalPosition.x)
                {
                    if (Ground.transform.position.z + (Ground.transform.localScale.z / 2f) > finalPosition.z && Ground.transform.position.z - (Ground.transform.localScale.z / 2f) < finalPosition.z)
                    {
                        return finalPosition;
                    }
                }
                Max++;
            }
            while (Max < 10);


        }
        return Vector3.zero;
    }
}
