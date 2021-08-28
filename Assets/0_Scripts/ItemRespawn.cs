using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawn : MonoBehaviour
{
    public float respawnTime = 4.0f;

    private void Start()
    {
        StartCoroutine(RespawnItemCo());
    }
    IEnumerator RespawnItemCo()
    {
        var tileMap = FindObjectOfType(typeof(TileMap)) as TileMap;
        while (true)
        {
            float delay = Random.Range(1, respawnTime);
            yield return new WaitForSeconds(delay);
            Debug.Log("아이템 생성!!  " + delay);
        }
    }
}
