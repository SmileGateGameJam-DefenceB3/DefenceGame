using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawn : MonoBehaviour
{
    public float respawnTime = 10.0f;
    public GameObject healthItem;
    public GameObject levelUpItem;

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
            //debugging
            int randX = Random.Range(0, 8);
            int randY = Random.Range(0, 5);
            int type = Random.Range(0, 2);
            var itemPrefab = default(GameObject);
            if (type == 0)
                itemPrefab = healthItem;
            else
                itemPrefab = levelUpItem;
            Debug.Log("아이템 생성!!  이전 딜레이: [" + randX + ", " + randY + "]");

            //get tile position
            Vector3 tile = tileMap[randX, randY].transform.position;

            //복제
            if (itemPrefab is null)
                continue;
            var obj = Instantiate(itemPrefab, new Vector3(tile.x, tile.y,
                0), Quaternion.identity);
            obj.transform.localScale = new Vector2(0.3f, 0.3f);
            Debug.Log(obj.name);
            
        }
    }
}
