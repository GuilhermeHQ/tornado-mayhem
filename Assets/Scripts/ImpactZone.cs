
using System;
using UnityEngine;

public class ImpactZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var destructibleObject = other.GetComponent<DestructibleObject>();
        if (destructibleObject == null)
        {
            return;
        }

        destructibleObject.StartShaking();
    }

    private void OnTriggerExit(Collider other)
    {
        var destructibleObject = other.GetComponent<DestructibleObject>();
        if (destructibleObject == null)
        {
            return;
        }

        destructibleObject.StopShaking();
    }
}
