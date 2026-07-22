/*
 * Assignment: AsteroidsGame - SpaceshipController Script - PART 1 & 2
 * 
 * Objective:
 * Implement a player controller for a spaceship in an Asteroids prototype. The player should be able to rotate the ship,
 * move forward, wrap around the screen, and shoot bullets. 
 * 
 * Requirements:
 * PART 1: Player Movement
 * 1. The player should be able to rotate the ship left and right using A/D keys from an input axis.
 *      This movement should be done with Transform based movement. 
 * 2. The player should be able to thrust forward using only the W key from an input axis
 *      This movement should be done with physics applied to a RigidBody2D. 
 * 3. The player should be able to wrap around the screen when they go off one edge and come back on the other side.
 * 4. The player should be able to teleport to a random location on the screen using left shift in an input button. You 
 *      do not need to check if there is an asteroid there. 
 *      Hint: For determining the random location, you can use the ScreenBounds class (see ScreenWrap.cs for how to use)
 *      
 * PART 2: Shooting
 * 1. The player should be able to shoot bullets using the space key in an input button
 *      Bullets should only go in the direction the ship is facing and bullet speed should be controlled by the Bullet.cs
 
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class AsteroidsPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public float rotationSpeed = 360f;
    [SerializeField] public float thrustForce = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletDelay = 5.0f;
    [SerializeField] private float immuneDuration = 3.0f;

    [SerializeField] public float rotationSpeedUp = 400f;
    [SerializeField] public float thrustForceUp = 20f;
    [SerializeField] public float bulletScaleMultiplier = 2f;
    [SerializeField] public float powerUpDuration = 8.0f;

    [SerializeField] private AsteroidSpawner asteroidSpawnerScript;
    [SerializeField] private Asteroid asteroidScript;
    [SerializeField] private PowerUpManager powerUpManagerScript;
    [SerializeField] private GameObject PlayerSpaceship;
    [SerializeField] private Collider2D spaceshipCollider;

    private Coroutine speedUpDuration;
    private Coroutine bulletSizeUpDuration;
    private bool bulletSizeUpActive = false;

    private float rotationInput;
    private float thrustInput;

    private float RandomY;
    private float RandomX;

    [SerializeField] private Vector2 spawnPosition;

    private int startingLives;
    [SerializeField] public int currentLives;

    public bool isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spaceshipCollider = GetComponent<Collider2D>();
        spaceshipCollider.enabled = true;

        spawnPosition = new Vector2(0,0);
        startingLives = 3;
        currentLives = startingLives;
        isDead = false;
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");

        HandleLives();

        if (!isDead)
        {
            HandleRotation();
            bulletDelay -= 0.1f;
            HandleFire();
            HandleHyperspace();
        }
    }

    void FixedUpdate()
    {
        HandleThrust();
    }

    private void HandleRotation()
    {
        if (Input.GetButton("Horizontal"))
        {
            transform.Rotate(Vector3.back * rotationSpeed * rotationInput * Time.deltaTime);
        }
    }

    private void HandleThrust()
    {
        if (Input.GetButton("Vertical") && !isDead)
        {
            rb.AddForce(transform.up * thrustForce * thrustInput, ForceMode2D.Force);
        }
    }

    private void HandleFire()
    {
        if (Input.GetButtonDown("Fire1") && bulletDelay <= 0.0f)
        {
            FireBullet();
            bulletDelay = 5.0f;
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        if (bulletSizeUpActive == false)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        if (bulletSizeUpActive == true)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.transform.localScale *= bulletScaleMultiplier;
        }
    }

    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            TeleportToRandomLocation();
        }
    }

    private void HandleLives()
    {
        if (currentLives <= 0)
        {
            isDead = true;
            Debug.Log("Out of Lives! Game Over!");
            Destroy(gameObject);
        }
    }

    // If player touches an asteroid, lose a life and destroy asteroid.
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Asteroid Collision

        if (collision.gameObject.CompareTag ("Asteroid"))
        {
            currentLives -= 1;
            transform.position = new Vector2(0,0);
            Destroy(collision.gameObject);
            spaceshipCollider.enabled = false;

            Invoke("ToggleColliderOn", immuneDuration);         
        }    

        // Power Ups Collision and Effect

        if (collision.gameObject.CompareTag ("1Up"))
        {
            currentLives += 1;
            Debug.Log(currentLives);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag ("SpeedUp"))
        {
            Destroy(collision.gameObject);

            if (speedUpDuration != null)
            {
                StopCoroutine(speedUpDuration);
            }

            speedUpDuration = StartCoroutine(SpeedCoroutine());

        }
        if (collision.gameObject.CompareTag ("SizeUp"))
        {
            Destroy(collision.gameObject);

            if (bulletSizeUpDuration != null)
            {
                StopCoroutine(bulletSizeUpDuration);
            }

            bulletSizeUpDuration = StartCoroutine(BulletSizeCoroutine());
        }
    }

    IEnumerator SpeedCoroutine()
    {
        SpeedUpPowerUp();

        yield return new WaitForSeconds(powerUpDuration);

        thrustForce = 10f;
        rotationSpeed = 360f;
        speedUpDuration = null;
    }

    private void SpeedUpPowerUp()
    {
        thrustForce = thrustForceUp;
        rotationSpeed = rotationSpeedUp;
    }

    IEnumerator BulletSizeCoroutine()
    {
        BulletSizeUpPowerUp();

        yield return new WaitForSeconds(powerUpDuration);

        bulletSizeUpActive = false;
    }

    private void BulletSizeUpPowerUp()
    {
        bulletSizeUpActive = true;
    }

    void ToggleColliderOn()
    {
        spaceshipCollider.enabled = true;
    }

    // Fetches a random point on the screen and returns it
    private Vector2 GetRandomPositionOnScreen()
    {
        RandomY = Random.Range(ScreenBounds.ScreenTop, ScreenBounds.ScreenBottom);
        RandomX = Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight);
        return new Vector2(RandomX, RandomY);
    }

    private void TeleportToRandomLocation()
    {
        Vector2 randomPoint = GetRandomPositionOnScreen();

        do
        {
            randomPoint = GetRandomPositionOnScreen();
        } while (!isTeleportSafe(randomPoint));

        if (isTeleportSafe(randomPoint))
        {
            transform.position = randomPoint;
        }
    }

    // Checks if position on screen is safe using OverlapCircleAll to check asteroid prescence
    public bool isTeleportSafe(Vector2 destination)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(destination, asteroidSpawnerScript.playerSafeDistance);

        // If there are no colliders, return isTeleportSafe as true

        if (hitColliders == null)
        {
            return true;
        }

        // If there ARE colliders, count the amount and return isTeleportSafe as false

        foreach (Collider2D hit in hitColliders)
        {
            if (hit == null || hit.gameObject == null)
            {
                continue;
            }

            if (hit.gameObject == this.gameObject)
            {
                continue;
            }

            Asteroid asteroidComponent = hit.GetComponent<Asteroid>();

            if (asteroidComponent != null)
            {
                return false;
            }
        }

        // All other cases return true

        return true;
    }
}
