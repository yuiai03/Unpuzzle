using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Coroutine explosionCoroutine;
    private void OnEnable()
    {
        if (explosionCoroutine != null) StopCoroutine(explosionCoroutine);
        explosionCoroutine = StartCoroutine(ExplosionCoroutine());
    }
    private IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(2f);
        PoolManager.Instance.ReturnObject(PoolType.Explosion, gameObject);
    }
}
