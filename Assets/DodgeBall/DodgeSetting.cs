using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DodgeBall", menuName = "ScriptableObjects/DodgeBall", order = 1)]
public class DodgeSetting : ScriptableObject
{
    public float agentRunSpeed;
    public float ballSpeed;
    public float shootCD;
}
