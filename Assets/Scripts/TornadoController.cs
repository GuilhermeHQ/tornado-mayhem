using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour
{
    public CharacterController controller;
    public Transform gfx;
    public float rotationSpeed = 1f;
    public float speed = 6f;
    public float growthRatio = .25f;
    
    bool isEnabled = true;
    private Vector3 tornadoSize;
    private Vector3 inputs = Vector3.zero;
    
    

    private void Start()
    {
        tornadoSize = GetMaxBounds(this.gameObject).size;
    }

    private void Grow()
    {
        
    }

    private void Shrink()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputs = Vector3.zero;
        inputs.x = isEnabled? Input.GetAxis("Horizontal") : 0;
        inputs.z = isEnabled? Input.GetAxis("Vertical") : 0;

        inputs = inputs.normalized;

        if (inputs.magnitude >= 0.1f)
        {
            controller.Move(inputs * (speed * Time.deltaTime));
        }
        
        gfx.Rotate(Vector3.up, rotationSpeed* Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        var objsize = other.collider.bounds.size;
        var objVolume = objsize.x * objsize.y * objsize.z;

        var tornadoVolume = tornadoSize.x * tornadoSize.y * tornadoSize.z;
        if (objVolume <= tornadoVolume)
        {
            Debug.Log("Building Destroyed");
        }
    }
    
    Bounds GetMaxBounds(GameObject parent)
    {
        var total = new Bounds(parent.transform.position, Vector3.zero);
        foreach (var child in parent.GetComponentsInChildren<Collider>())
        {
            total.Encapsulate(child.bounds);
        }
        return total;
    }
    
    private void OnDrawGizmos()
    {
        // // Draw each child's bounds as a green box.
        // Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        // foreach (var child in GetComponentsInChildren<Collider>())
        // {
        //     Gizmos.DrawCube(child.bounds.center, child.bounds.size);
        // }
 
        // Draw total bounds of all the children as a white box.
        Gizmos.color = new Color(0f, 1f, 1f, 0.1f);
        Gizmos.DrawCube(gameObject.transform.position + Vector3.up * tornadoSize.y/2, tornadoSize);
    }
}
