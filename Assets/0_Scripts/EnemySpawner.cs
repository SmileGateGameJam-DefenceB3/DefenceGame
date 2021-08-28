using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public void StartSpawn()
    {
        StartCoroutine(nameof(SpawnCo));
    }
    
    public void EndSpawn()
    {
        StopCoroutine(nameof(SpawnCo));
    }

    private IEnumerator SpawnCo()
    {
        while (true)
        {
            yield return null;
        }
    }
}
