using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject powerUpIndicator;

    [SerializeField] private float speed = 5.0f;
    
    [SerializeField] private float hangTime;
    [SerializeField] private float smashSpeed;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    
    public PowerUpType currentPowerUp = PowerUpType.None;
    public bool hasPowerUp;

    private GameObject _focalPoint;
    private GameObject _tmpRocket;
    private Coroutine _powerupCountDown;
    private Rigidbody _playerRb;

    private float _powerUpStrenght = 15.0f;
    private bool _smashing = false;
    private float _floorY;

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _focalPoint = GameObject.Find("Focal Point");
    }

    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        _playerRb.AddForce(_focalPoint.transform.forward * speed * forwardInput);
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.2f, 0);
        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }

        if(currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !_smashing)
        {
            _smashing = true;
            StartCoroutine(Smash());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            Destroy(other.gameObject);
            powerUpIndicator.gameObject.SetActive(true);

            if(_powerupCountDown != null)
            {
                StopCoroutine(_powerupCountDown);
            }
            _powerupCountDown = StartCoroutine(PowerupCountDownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Player collided with " + collision.gameObject.name + "with powerup set to " + currentPowerUp.ToString());
            enemyRigidbody.AddForce(awayFromPlayer * _powerUpStrenght, ForceMode.Impulse);
        }
    }

    private void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType <Enemy>())
        {
            _tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            _tmpRocket.GetComponent<RocketBahaviour>().Fire(enemy.transform);
        }
    }

    IEnumerator PowerupCountDownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        powerUpIndicator.gameObject.SetActive(false);
    }

    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();

        //store the y position before taking off
        _floorY = transform.position.y;

        //calculate the amount of time we will go up
        float jumpTime = Time.time + hangTime;

        while(Time.time < jumpTime)
        {
            //move the player up while still keeping their x velocity
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, smashSpeed);
            yield return null;
        }
        //now move the player down
        while (transform.position.y > _floorY)
        {
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }
        //cycle through all enemies
        for (int i=0; i<enemies.Length; i++)
        {
            //apply an explosion force that originates from our position
            if (enemies[i] != null)
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, 
                    explosionRadius, 0.0f, ForceMode.Impulse);
        }
        //we are no longer smashing, so set the boolean to false
        _smashing = false;
    }
}
