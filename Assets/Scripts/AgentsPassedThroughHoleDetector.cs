using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsPassedThroughHoleDetector : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        var flyingAgent = other.gameObject.GetComponent<FlyingAgent>();
        if (flyingAgent.isAlive)
        {
            flyingAgent.OnFliedThroughTheHole();
        }
    }
}
