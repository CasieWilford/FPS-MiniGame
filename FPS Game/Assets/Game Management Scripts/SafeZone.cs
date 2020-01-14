using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("FPSPlayer"))
        {
            UnkillableEnemyShooter.startFiring = false;
        }
    }
}
