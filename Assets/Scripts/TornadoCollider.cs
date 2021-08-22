using UnityEngine;

public class TornadoCollider : MonoBehaviour
{
    [SerializeField] private TornadoController tornadoController;

    private void OnTriggerEnter(Collider other)
    {
        tornadoController.OnTornadoTriggerEnter(other);
    }
}
