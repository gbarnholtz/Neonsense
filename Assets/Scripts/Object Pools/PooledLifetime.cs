using System.Collections;
using UnityEngine;

public class PooledLifetime : PooledObject
{
    [SerializeField] private float lifetime;
    void OnEnable()
    {
        StartCoroutine(WaitToRelease());
    }

    private IEnumerator WaitToRelease() {
        yield return new WaitForSeconds(lifetime);
        Release();
    }

}
