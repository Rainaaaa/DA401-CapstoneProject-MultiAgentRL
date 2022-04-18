using System;
using UnityEngine;

namespace DodgeBall
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField]
        private AgentDodge agent;

        [SerializeField]
        private SpriteRenderer sprite;

        private float initialWidth = 2;
        private void Update()
        {
            if (Camera.main != null)
                transform.LookAt(Camera.main.transform);
            var size = sprite.size;
            size.x = (agent.health / 100f) * initialWidth;
            sprite.size = size;
        }
    }
}
