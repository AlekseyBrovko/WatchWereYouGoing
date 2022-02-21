using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpAnim : MonoBehaviour
{
    [SerializeField] private float time = 0;
    [SerializeField] private float ampltude = 0.25f;
    [SerializeField] private float frequency = 2;
    [SerializeField] private float offset = 0;
    [SerializeField] private float rotationSpeed = 0.5f;

    private void Update()
    {
        time = time + Time.deltaTime;
        offset = ampltude * Mathf.Sin(time * frequency);     
        transform.position = transform.position + new Vector3(0, offset, 0);
        transform.Rotate(new Vector3(0, rotationSpeed, 0));
    }
}
