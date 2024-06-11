using System.Collections;
using UnityEngine;

public class AttackControllerMono : MonoBehaviour
{
    private WaitForSeconds OnDelay = new WaitForSeconds(0.4f);
    private WaitForSeconds OffDelay = new WaitForSeconds(0.15f);
    [SerializeField] private PlayerStatHandler _statHandler;
    [SerializeField] public MeshCollider batCollider;
    
    public IEnumerator ColliderSet(MeshCollider bat)
    {
        yield return OnDelay;
        bat.enabled = true;
        yield return OffDelay;
        bat.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && other.gameObject.TryGetComponent<IDamage>(out IDamage enemy))
        {
            Debug.Log("Enemy Attacked!");
            enemy.HandleDamage(_statHandler.stat.Power);
        }
    }
}