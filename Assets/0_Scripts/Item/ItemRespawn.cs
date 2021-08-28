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

            //get tile position
            Vector3 tile = tileMap[randX, randY].transform.position;

            //����
            if (itemPrefab is null)
                continue;

            SoundManager.PlaySfx(ClipType.ItemSpawn);
            var obj = Instantiate(itemPrefab, new Vector3(tile.x, tile.y + 0.1f,
                0), Quaternion.identity);
            var objViewImage = obj.transform.Find("ViewImage").gameObject;
            objViewImage.transform.localScale = new Vector2(0.0f, 0.0f);

            var objShadow = obj.transform.Find("ShaderImage").gameObject;
            objShadow.transform.localScale = new Vector2(0.0f, 0.0f);

            if (type != 0)
            {
                int angler = Random.Range(0, 2);
                objViewImage.transform.rotation = (angler == 0) ? Quaternion.Euler(0, 0, 18) : Quaternion.Euler(0, 0, -18);
            }
        }
    }
}
