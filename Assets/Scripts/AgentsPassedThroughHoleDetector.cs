using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsPassedThroughHoleDetector : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        other.gameObject.GetComponent<FlyingAgent>().OnFliedThroughTheHole();
    }
}
