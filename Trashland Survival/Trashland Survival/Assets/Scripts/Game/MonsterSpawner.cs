using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance;

    [Header("몬스터 스폰 설정")]
    public int maxActiveMonsters = 100;

    private Camera mainCamera;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        mainCamera = Camera.main;
    }

    public void StartSpawningWave(WaveData waveData)
    {
        foreach (var group in waveData.monstersInWave)
        {
            StartCoroutine(SpawnMonsterGroup(group, waveData.waveDuration));
        }
    }

    private IEnumerator SpawnMonsterGroup(MonsterGroup group, float waveDuration)
    {
        float spawnTotalTime = waveDuration * 0.75f;
        float spawnInterval = spawnTotalTime / group.count;

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
                monster.Initialize(monsterData, PlayerManager.Instance.playerTransform);
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