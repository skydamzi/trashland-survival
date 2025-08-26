using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public List<WaveData> waves;
    private int currentWaveIndex = 0;

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
    }

    void Start()
    {
        if (waves.Count > 0)
        {
            StartCoroutine(SpawnWaves());
        }
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            UIManager.Instance.UpdateDayStatus(currentWaveIndex + 1);
            WaveData currentWave = waves[currentWaveIndex];
            MonsterSpawner.Instance.StartSpawningWave(currentWave);

            yield return new WaitForSeconds(currentWave.waveDuration);

            currentWaveIndex++;
        }

        Debug.Log("모든 웨이브 종료");
        GameManager.Instance.ChangeState(GameState.Clear);
    }
}