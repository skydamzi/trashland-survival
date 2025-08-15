using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    void Awake()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.playerTransform = this.transform;
            Debug.Log("플레이어 오브젝트가 PlayerManager에 등록");
        }
    }
}
