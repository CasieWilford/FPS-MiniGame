using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("FPSPlayer"))
        {
            UnkillableEnemyShooter.startFiring = true;
        }
    }
}
