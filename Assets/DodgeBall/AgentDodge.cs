using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentDodge : Agent
{
    public enum Team
    {
        Blue = 0,
        Purple = 1
    }

    public Team team;
    public Rigidbody agentRb;
    public BehaviorParameters m_BehaviorParameters;
    public EnvironmentParameters m_EnvironmentParameters;
    public GameObject ball;
    public float m_ForwardSpeed;
    public float m_LateralSpeed;
    public DodgeSetting setting;
    private int canShoot;
    private float shootCD;
    public Transform initialPos;
    public int health = 100;
    public ArenaController ArenaController;
    private int m_ResetTimer = 0;
    public int MaxEnvironmentSteps;

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        canShoot = 1;
        shootCD = 0;
        transform.position = initialPos.position;
        health = 100;
        m_ResetTimer = 0;
    }

    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            EndEpisode();
        }
    }

    private void Update()
    {
        shootCD -= Time.deltaTime;
        if (shootCD < 0)
        {
            canShoot = 1;
        }
        else
        {
            canShoot = 0;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(canShoot);
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;


        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];
        var throwing = act[3];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        if (throwing == 1)
        {
            if (shootCD < 0)
            {
                ShootBall();
                shootCD = setting.shootCD;
            }

        }
        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        agentRb.AddForce(dirToGo * setting.agentRunSpeed,
            ForceMode.VelocityChange);
    }

    public void ShootBall()
    {
        GameObject shootBall = Instantiate(ball);
        var ballController = shootBall.GetComponent<BallController>();
        ballController.sender = this;
        ballController.Shoot();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "purpleBall")
        {
            if (team == Team.Blue)
            {
                health -= 10;
                AddReward(-0.1f);
                if (health <= 0)
                {
                    AddReward(-1);
                    ArenaController.EndEpisode(this, 1 - (float)m_ResetTimer / MaxEnvironmentSteps);
                    /*EndEpisode();*/
                }
            }
        }
        else if (other.collider.tag == "blueBall")
        {
            if (team == Team.Purple)
            {
                health -= 10;
                AddReward(-0.1f);
                if (health <= 0)
                {
                    AddReward(-1);
                    ArenaController.EndEpisode(this, 1 - (float)m_ResetTimer / MaxEnvironmentSteps);
                    /*EndEpisode();*/
                }
            }
        }
    }
}
