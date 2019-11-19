using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{

    public MyAgentsScript BehindPlayer = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitByBullet(MyAgentsScript bulletParent)
    {
        if (BehindPlayer != null && BehindPlayer != bulletParent)
        {
           // BehindPlayer.AddReward(0.5f);
            //bulletParent.AddReward(-0.5f);
        }
    }
}
