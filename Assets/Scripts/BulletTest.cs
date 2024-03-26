using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;

public class BulletTest : NetworkBehaviour
{
    private void Start()
    {
        Debug.Log("Bullet Spawn");
        gameObject.transform.DOLocalMove(gameObject.transform.position + Vector3.forward, 5f).SetLoops(3, LoopType.Incremental);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player got hit");
        }
    }
}
