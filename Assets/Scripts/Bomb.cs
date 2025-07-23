using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collisionChecker;
    private void Start()
    {
        ScaleEffect();
    }

    private void ScaleEffect()
    {
        transform.DOScale(Vector3.one * 0.9f, 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

    }
    private void OnDisable()
    {
        transform?.DOKill();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            var tile = collision.GetComponent<Tile>();
            if (tile)
            {
                CollisionChecker();
                Destroy(gameObject);
                AudioManager.Instance.PlaySFX(AudioType.Explosion);
            }
        }
    }

    private void CollisionChecker()
    {
        if (collisionChecker)
        {
            var colliders = Physics2D.OverlapBoxAll(collisionChecker.bounds.center, collisionChecker.bounds.size, 0f);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.CompareTag("Tile"))
                {
                    var tile = collider.GetComponent<Tile>();
                    if (tile)
                    {
                        tile.transform.DOKill();
                        Destroy(collider.gameObject);
                        GameManager.Instance.CurrentLevelManager.RemoveToList(tile);
                        PoolManager.Instance.GetObject<Explosion>(PoolType.Explosion, collider.transform.position, null);
                    }
                }
            }
        }
    }
}
