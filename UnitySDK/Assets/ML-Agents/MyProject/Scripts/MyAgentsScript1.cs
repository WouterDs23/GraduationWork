using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.UI;

public class MyAgentsScript1 : Agent
{
    private ShooterAcademy _Academy;
    public float AgentSpeed = 1f;
    public float AgentRotateSpeed = 2f;
    private RayPerception3D rayPer;
    private GunScript _Gun;
    private float _WaitTimeTillShoot = 0;
    [SerializeField] private GameObject _Ground = null;
    [SerializeField] private GameObject _Ground1 = null;
    [SerializeField] private GameObject _Ground2 = null;
    [SerializeField] private GameObject _Ground3 = null;
    private bool seenPlayer = false;
    private bool lockOnPlayer = false;
    private int BulletsMissed = 0;
    [SerializeField] private string _NameResetParam = "";
    //[SerializeField] private Brain _Begin;
    [SerializeField] private bool _CanShoot = true;
    private bool DestroyObjectives = false;


    private Rigidbody _AgentRigidBody;
    public override void InitializeAgent()
    {
        _Academy = FindObjectOfType(typeof(ShooterAcademy)) as ShooterAcademy;
        _AgentRigidBody = GetComponent<Rigidbody>();
        rayPer = GetComponent<RayPerception3D>();
        _Gun = GetComponent<GunScript>();
        // transform.position = _Academy.GetRandomPos();
    }

    public void ConfigureAgent(float Quality)
    {
        switch (Quality)
        {
            case 0:
                _Academy.Ground = _Ground;
                break;
            case 1:
                _Academy.Ground = _Ground1;
                break;
            case 2:
                // UnityEditor.EditorApplication.isPlaying = false;
                _Academy.Ground = _Ground2;
                break;
            case 3:
                // UnityEditor.EditorApplication.isPlaying = false;
                _Academy.Ground = _Ground3;
                DestroyObjectives = true;
                break;
        }
    }

    public override void CollectObservations()
    {
        seenPlayer = false;
        lockOnPlayer = false;
        // int seenPlayer = 0;
        float rayDistance = 70f;
        float[] rayAngles = { 67.5f, 90f, 112.5f };
        float[] rayAngles2 = { 75f, 90f, 105f };
        float[] rayAngles3 = { 90f };
        string[] detectableObjects = { "wall", "Player", "obstacles", "goal" };

        // AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        //for (int i = 0; i < rayPer.obstacleTags.Count; i++)
        //{
        //    if (rayPer.obstacleTags[i] == "Player")
        //    {
        //        seenPlayer++;
        //    }
        //}
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1f, 0f));
        for (int i = 0; i < rayPer.obstacleTags.Count; i++)
        {
            if (rayPer.obstacleTags[i].tag == "Player" && !seenPlayer)
            {
                seenPlayer = true;
                AddReward(0.1f);
            }
            else if (rayPer.obstacleTags[i].GetComponent<LookObjective>() != null && _CanShoot)
            {
                rayPer.obstacleTags[i].GetComponent<LookObjective>().newLocation(_Academy.Ground);
                AddReward(0.01f);
                if (DestroyObjectives)
                {
                    Destroy(rayPer.obstacleTags[i]);
                }
            }
        }
        if (!seenPlayer)
        {
            AddReward(-0.01f);
        }
        seenPlayer = false;
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, 0f));
        for (int i = 0; i < rayPer.obstacleTags.Count; i++)
        {
            if (rayPer.obstacleTags[i].tag == "Player")
            {
                seenPlayer = true;
            }
        }
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles3, detectableObjects, 0f, 0f));
        for (int i = 0; i < rayPer.obstacleTags.Count; i++)
        {
            if (rayPer.obstacleTags[i].tag == "Player")
            {
                lockOnPlayer = true;
            }
        }
        Vector3 localVel = transform.InverseTransformDirection(_AgentRigidBody.velocity);
        AddVectorObs(localVel.x);
        AddVectorObs(localVel.z);

        AddVectorObs(transform.rotation.y);
        AddVectorObs(transform.position.x);
        AddVectorObs(transform.position.z);
        AddVectorObs(BulletsMissed);


        // if (seenPlayer > 0)
        // {
        //     AddReward(0.001f);
        // }

    }

    public void MoveAgent(float[] act)
    {
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            dirToGo = transform.forward * Mathf.Clamp(act[0], -1f, 1f);
            rotateDir = transform.up * Mathf.Clamp(act[1], -1f, 1f);
        }
        else
        {
            //TODO Remove
            if (_CanShoot)
            {
                switch (Mathf.FloorToInt(act[0]))
                {
                    case 0:
                        if (lockOnPlayer)
                        {
                            AddReward(-2f);
                        }
                        else if (seenPlayer)
                        {
                            AddReward(-1f);
                        }
                        else
                        {
                            AddReward(0.01f);
                        }
                        break;
                    case 1:
                        if (_WaitTimeTillShoot < 0)
                        {
                            _WaitTimeTillShoot = 0.5f;
                            _Gun.Shoot(GetComponent<Transform>());

                            if (lockOnPlayer)
                            {
                                AddReward(4f);
                            }
                            else if (seenPlayer)
                            {
                                AddReward(1f);
                            }
                            else
                            {
                                BulletsMissed++;
                                AddReward(-4f);
                            }
                        }
                        break;

                }
            }
            DoRotation(Mathf.FloorToInt(act[1]));

            DoMove(Mathf.FloorToInt(act[2]));

        }
    }
    private void DoRotation(int action)
    {
        float rotateAmount = 0;
        switch (action)
        {
            case 1:
                rotateAmount = AgentRotateSpeed;
                break;
            case 2:
                rotateAmount = -AgentRotateSpeed;
                break;
        }
        Vector3 rotateVector = transform.up * rotateAmount;
        _AgentRigidBody.MoveRotation(Quaternion.Euler(_AgentRigidBody.rotation.eulerAngles + rotateVector * AgentRotateSpeed));
    }
    private void DoMove(int action)
    {
        float moveAmount = 0;
        switch (action)
        {
            case 1:
                moveAmount = AgentSpeed;
                break;
            case 2:
                moveAmount = AgentSpeed * -0.5f; ;
                break;
        }
        Vector3 moveVector = transform.forward * moveAmount;
        _AgentRigidBody.AddForce(moveVector * AgentSpeed, ForceMode.VelocityChange);
    }
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        _WaitTimeTillShoot -= Time.deltaTime;
        MoveAgent(vectorAction);

        if (GetCumulativeReward() < -20f && _CanShoot)
        {
            _Academy.Done();
        }
        else if (GetCumulativeReward() > 0f && _Academy.GetStepCount() > 5000 && _CanShoot)
        {
            _Academy.Done();
        }
        else
        {
            AddReward(-0.0001f);
        }
    }

    public override void AgentReset()
    {
        BulletsMissed = 0;
        ConfigureAgent(_Academy.resetParameters[_NameResetParam]);
        Vector3 temprot = Vector3.zero;
        temprot.x = _AgentRigidBody.rotation.eulerAngles.x;
        temprot.y = Random.Range(0f, 360f);
        temprot.z = _AgentRigidBody.rotation.eulerAngles.z;
        _AgentRigidBody.MoveRotation(Quaternion.Euler(temprot));

        transform.position = _Academy.GetRandomPos();
    }

    public override void AgentOnDone()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.tag == "behind")
        {
          //  other.transform.parent.gameObject.GetComponent<ObstacleScript>().BehindPlayer = this;
        }
        if (other != null && other.gameObject.tag == "wall")
        {
            AddReward(-5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject.tag == "behind")
        {
            other.transform.parent.gameObject.GetComponent<ObstacleScript>().BehindPlayer = null;
        }
    }
}
