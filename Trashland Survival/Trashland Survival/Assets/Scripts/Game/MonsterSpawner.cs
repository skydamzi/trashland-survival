using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("스테이지 웨이브 설정")]
    public List<WaveData> waves;

    [Header("몬스터 스폰 설정")]
    public int maxActiveMonsters = 100;

    private int currentWaveIndex = 0;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (waves.Count > 0)
        {
            StartCoroutine(SpawnWaves());
        }
    }

    private IEnumerator SpawnWaves()
    {
        float elapsedTime = 0f;

        while (currentWaveIndex < waves.Count)
        {
            WaveData currentWave = waves[currentWaveIndex];
            
            foreach (var group in currentWave.monstersInWave)
            {
                StartCoroutine(SpawnMonsterGroup(group, currentWave.waveDuration));
            }

            yield return new WaitForSeconds(currentWave.waveDuration);
            elapsedTime += currentWave.waveDuration;

            currentWaveIndex++;
        }

        Debug.Log("모든 웨이브 종료");
    }

    private IEnumerator SpawnMonsterGroup(MonsterGroup group, float waveDuration)
    {
        float spawnInterval = waveDuration / group.count;

        for (int i = 0; i < group.count; i++)
        {
            while (PoolManager.Instance.GetTotalActiveCount() >= maxActiveMonsters)
            {
                yield return null;
            }

            SpawnMonster(group.monsterData);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMonster(MonsterData monsterData)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject monsterObj = PoolManager.Instance.GetFromPool(monsterData.prefab, spawnPosition, Quaternion.identity);

        if (monsterObj != null)
        {
            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                monster.monsterData = monsterData;
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float spawnDistance = 15f;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = mainCamera.transform.position + (Vector3)randomDirection * spawnDistance;
        spawnPos.z = 0;
        return spawnPos;
    }
}
