using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private AsteroidsPlayerController spaceshipControllerScript;
    [SerializeField] private PowerUpSpawner powerUpSpawnerScript;
    public enum PowerUpType { LifeUp, BulletSizeUp, MovementSpeedUp }

    [SerializeField] private PowerUpType type;
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private PowerUpSpawner spawner;
    private Vector2 velocity;

    [SerializeField] private TextMeshProUGUI livesText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawner = FindAnyObjectByType<PowerUpSpawner>();
    //    rb.linearVelocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
