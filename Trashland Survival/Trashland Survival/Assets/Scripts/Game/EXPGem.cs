using UnityEngine;

public class EXPGem : MonoBehaviour
{
    public int expAmount;
    public float attractionSpeed;
    private Transform playerTransform;

    void Start()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.playerTransform != null)
        {
            playerTransform = PlayerManager.Instance.playerTransform;
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= PlayerManager.Instance.magnetPower)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                transform.position += direction * attractionSpeed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerManager.Instance != null && other.gameObject == PlayerManager.Instance.playerTransform.gameObject)
        {
            PlayerManager.Instance.GainExp(expAmount);
            Destroy(gameObject);
        }
    }
    public void SetExperience(int amount)
    {
        expAmount = amount;
    }
}