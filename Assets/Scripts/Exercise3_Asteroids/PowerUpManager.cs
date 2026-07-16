using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private AsteroidsPlayerController spaceshipControllerScript;
    public enum PowerUpType { LifeUp, BulletSizeUp, MovementSpeedUp }

    [SerializeField] private PowerUpType type;
    [SerializeField] private float rotationSpeedUp = 400f;
    [SerializeField] private float thrustForceUp = 600f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
