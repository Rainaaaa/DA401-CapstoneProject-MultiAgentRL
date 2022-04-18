using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public List<AgentDodge> blueAgent;
    public List<AgentDodge> purpleAgent;

    public void EndEpisode(AgentDodge deadAgent, float extra)
    {
        if (deadAgent.team == AgentDodge.Team.Blue)
        {
            foreach (var agent in blueAgent)
            {
                agent.AddReward(1+extra);
                agent.EndEpisode();
            }
            foreach (var agent in purpleAgent)
            {
                agent.EndEpisode();
            }
        }
        else
        {
            foreach (var agent in purpleAgent)
            {
                agent.AddReward(1+extra);
                agent.EndEpisode();
            }
            foreach (var agent in blueAgent)
            {
                agent.EndEpisode();
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
