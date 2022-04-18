using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallController : MonoBehaviour
{
    public AgentDodge.Team Team;
    public AgentDodge sender;
    public DodgeSetting setting;
    public Rigidbody mRig;

    public void Shoot()
    {
        transform.position = sender.transform.position + sender.transform.forward * 0.9f + sender.transform.up * 0.3f;
        transform.rotation = sender.transform.rotation;
        mRig.velocity = transform.forward * setting.ballSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "blueAgent")
        {
            if (Team == AgentDodge.Team.Purple)
            {
                sender.AddReward(0.1f);
            }
        }
        else if (other.collider.tag == "purpleAgent")
        {
            if (Team == AgentDodge.Team.Blue)
            {
                sender.AddReward(0.1f);
            }
        }
        Destroy(gameObject);
    }
}
