using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private AsteroidsPlayerController spaceshipControllerScript;
    [SerializeField] private PowerUpSpawner powerUpSpawnerScript;
    public enum PowerUpType { LifeUp, BulletSizeUp, MovementSpeedUp }

    [SerializeField] private PowerUpType type;
    [SerializeField] private float rotationSpeedUp = 400f;
    [SerializeField] private float thrustForceUp = 600f;
    [SerializeField] private float powerUpDuration = 8.0f;

    private PowerUpManager spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 1Up, SizeUp, SpeedUp player interaction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("1Up"))
        {
            spaceshipControllerScript.currentLives += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("SizeUp"))
        {
            Invoke("BulletSizeUpPowerUp", powerUpDuration);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("SpeedUp"))
        {
            Invoke("SpeedUpPowerUp", powerUpDuration);
            Destroy(collision.gameObject);
            spaceshipControllerScript.thrustForce = 500f;
            spaceshipControllerScript.rotationSpeed = 360f;
        }
    }

    private void BulletSizeUpPowerUp()
    {
        
    }

    private void SpeedUpPowerUp()
    {
        spaceshipControllerScript.thrustForce = thrustForceUp;
        spaceshipControllerScript.rotationSpeed = rotationSpeedUp;
    }
}
