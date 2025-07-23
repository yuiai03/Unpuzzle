using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private void Start()
    {
        RotationEffect();
    }

    private void RotationEffect()
    {
        transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);

    }
    private void OnDisable()
    {
        transform?.DOKill();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Tile"))
        {
            var tile = collision.GetComponent<Tile>();
            if (tile)
            {
                AudioManager.Instance.PlaySFX(AudioType.Explosion);

                tile.transform.DOKill();
                Destroy(collision.gameObject);
                GameManager.Instance.CurrentLevelManager.RemoveToList(tile);
                PoolManager.Instance.GetObject<Explosion>(PoolType.Explosion, collision.transform.position, null);
            }
        }
    }
}
