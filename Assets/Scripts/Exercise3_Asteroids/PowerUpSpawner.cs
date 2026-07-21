using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject lifeUpPowerUp;
    [SerializeField] private GameObject speedUpPowerUp;
    [SerializeField] private GameObject bulletSizePowerUp;

    [SerializeField] private int initialPowerUpCount = 3;

    private float spawnXMax = 0f;
    private float spawnXMin = 0f;
    private float spawnYMax = 0f;
    private float spawnYMin = 0f;
    [SerializeField] public float playerSafeDistance = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
        spawnXMax = screenHalfWidth + playerSafeDistance;
        spawnXMin = -screenHalfWidth - playerSafeDistance;
        spawnYMax = screenHalfHeight + playerSafeDistance;
        spawnYMin = -screenHalfHeight - playerSafeDistance;

        SpawnInitialPowerUps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnInitialPowerUps()
    {
        for (int i = 0; i < initialPowerUpCount; i++)
        {
            Vector3 spawnPosition;
            do
            {
                float randomX = Random.Range(spawnXMin, spawnXMax);
                float randomY = Random.Range(spawnYMin, spawnYMax);

                spawnPosition = new Vector3(randomX, randomY, 0f);
            } while (Vector3.Distance(spawnPosition, Vector3.zero) < playerSafeDistance);

            SpawnPowerUp(spawnPosition, PowerUpManager.PowerUpType.LifeUp);
        }
    }

    public void SpawnPowerUp(Vector3 position, PowerUpManager.PowerUpType type)
    {
        GameObject powerUpPrefab = null;
        switch (type)
        {
            case PowerUpManager.PowerUpType.MovementSpeedUp:
                powerUpPrefab = speedUpPowerUp;
                break;

            case PowerUpManager.PowerUpType.LifeUp:
                powerUpPrefab = lifeUpPowerUp;
                break;

            case PowerUpManager.PowerUpType.BulletSizeUp:
                powerUpPrefab = bulletSizePowerUp;
                break;
        }
        if (powerUpPrefab != null)
        {
            Instantiate(powerUpPrefab, position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        }
    }
}
