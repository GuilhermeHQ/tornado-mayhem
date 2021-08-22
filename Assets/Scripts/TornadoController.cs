using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TornadoController : MonoBehaviour
{
    public CharacterController controller;
    public Transform gfx;
    public float rotationSpeed = 200f;
    public float speed = 6f;
    public float growthRatio = .25f;
    public float maxLevel = 10;
    public int maxObjectsOrbiting = 10;
    public Collider collider;

    public bool isEnabled = true;
    private Vector3 tornadoSize;
    private Vector3 inputs = Vector3.zero;
    private CinemachineFreeLook cinemachineFreeLook;

    // public ItemPoints itemPointConfig;
    // private Dictionary<ItemType, ItemPointData> ItemPointsDict;


    public Action<DestructibleObject> onCollideWithDestructibleObject;

    private Queue<DestructibleObject> orbitingObjects;
    

    private void Start()
    {
        tornadoSize = GetMaxBounds(this.gameObject).size;
        controller.detectCollisions = false;
        cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
        cinemachineFreeLook.m_YAxis.Value = (transform.localScale.x - 1) / (1 + (maxLevel) * growthRatio);
        
        orbitingObjects = new Queue<DestructibleObject>(maxObjectsOrbiting);

        // itemPointConfig = Resources.Load("ItemPointData") as ItemPoints;
        //
        // if (itemPointConfig == null)
        // {
        //     Debug.Log("Não consegui encontrar o arquivo de dados de item!!");
        //     return;
        // }
        //
        // ItemPointsDict = new Dictionary<ItemType, ItemPointData>();
        //
        // foreach (ItemPointData itemPointData in itemPointConfig.itemPointData)
        // { 
        //     ItemPointsDict.Add(itemPointData.itemType, itemPointData);
        // }
    }

    public void Grow()
    {
        transform.localScale += Vector3.one * growthRatio;
        tornadoSize = GetMaxBounds(gameObject).size;
        
        cinemachineFreeLook.m_YAxis.Value = (transform.localScale.x - 1) / (1 + (maxLevel) * growthRatio);
    }

    public void Shrink()
    {
        transform.localScale -= Vector3.one * growthRatio;
        tornadoSize = GetMaxBounds(gameObject).size;
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
    

    
    public void OnTornadoTriggerEnter(Collider other)
    {
        var destructibleObject = other.GetComponent<DestructibleObject>();
        if (destructibleObject == null)
        {
            return;
        }
        
        //Destroying collider
        Destroy(other);
        
        onCollideWithDestructibleObject?.Invoke(destructibleObject);
        
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
    
    // private void OnDrawGizmos()
    // {
    //     
    //     // Draw total bounds of all the children as a white box.
    //     Gizmos.color = new Color(0f, 1f, 1f, 0.1f);
    //     Gizmos.DrawCube(gfx.position, tornadoSize);
    // }

    public void OrbitObject(DestructibleObject destructibleObject)
    {
        if (orbitingObjects.Count == 5)
        {
            var obj = orbitingObjects.Dequeue();
            obj.InterruptOrbit();
        }
        
        destructibleObject.OrbitAround(this.gfx);
        orbitingObjects.Enqueue(destructibleObject);
        //StartCoroutine(RotateAround(destructibleObject));
    }

    private IEnumerator RotateAround(DestructibleObject destructibleObject)
    {
        Tween tween;
        
        destructibleObject.transform.SetParent(this.gfx);

        tween = destructibleObject.transform.DOLocalMove(
            new Vector3(
                Random.Range(-1.5f, 1.5f), 
                Random.Range(-0.3f, 0.3f), 
                Random.Range(-1.5f, 1.5f)), 
            1).Play();

        destructibleObject.transform.DOScale(destructibleObject.transform.localScale * 0.4f, 2);
        
        yield return new WaitUntil(() => !tween.IsPlaying());
        

        tween = destructibleObject.transform.DOLocalRotate(Random.rotation.eulerAngles, 1, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear).Play();
        
        //destructibleObject.transform.DOShakePosition(5, 0.1f, 5, 90f, false, false).Play();
        //tween = destructibleObject.transform.DOShakeRotation(5, 0.1f, 5, 90f, false).Play();
        
        yield return new WaitForSeconds(5);
        tween.Kill();
        Destroy(destructibleObject.gameObject);
    }
}
