using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DestructibleObject: MonoBehaviour
{
    private Bounds objectSize;
    public ItemType itemType;

    private Coroutine orbitCoroutine;
    private Tween tween;
    private Tween shakeTween;

    private void Start()
    {
        objectSize = GetMaxBounds(this.gameObject);
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
        Gizmos.color = new Color(0.6f, 0.5f, 1f, 0.3f);
        Gizmos.DrawCube(objectSize.center, objectSize.size);
    }

    public void OrbitAround(Transform tornadoGfx)
    {
        orbitCoroutine = StartCoroutine(RotateAround(tornadoGfx));
    }

    public void InterruptOrbit()
    {
        if (orbitCoroutine != null)
        {
            StopCoroutine(orbitCoroutine);
        }
        
        tween?.Kill();

        StartCoroutine(Vanish());
    }

    private IEnumerator Vanish()
    {
        transform.DOScale(0, 2).Play();
        transform.DOLocalMove(Vector3.zero, 2).Play();
        
        yield return new WaitForSeconds(2.1f);
        
        Destroy(this.gameObject);
    }
    
    private IEnumerator RotateAround(Transform tornadoGfx)
    {
        StopShaking();
        
        transform.SetParent(tornadoGfx);

        tween = transform.DOLocalMove(
            new Vector3(
                UnityEngine.Random.Range(-1.5f, 1.5f), 
                UnityEngine.Random.Range(-0.3f, 0.3f), 
                UnityEngine.Random.Range(-1.5f, 1.5f)), 
            1).Play();

        transform.DOScale(transform.localScale * 0.4f, 2);
        
        yield return new WaitUntil(() => !tween.IsPlaying());
        

        tween = transform.DOLocalRotate(UnityEngine.Random.rotation.eulerAngles, 0.5f).SetRelative().SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).Play();
        
        //destructibleObject.transform.DOShakePosition(5, 0.1f, 5, 90f, false, false).Play();
        //tween = destructibleObject.transform.DOShakeRotation(5, 0.1f, 5, 90f, false).Play();
        
        yield return new WaitForSeconds(5);
        tween.Kill();
        yield return Vanish();
    }

    public void StartShaking()
    {
        shakeTween?.Kill();
        shakeTween = transform.DOShakePosition(1, 0.05f, 10, 90f, false, false)
            .SetLoops(-1)
            .SetEase(Ease.Linear)
            .Play();
    }

    public void StopShaking()
    {
        shakeTween?.Kill();
    }
}
