using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAnim : MonoBehaviour
{
    [SerializeField] private float time = 0;
    [SerializeField] private float ampltude = 0.25f;
    [SerializeField] private float frequency = 2;
    [SerializeField] private float offset = 0;
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        time = time + Time.deltaTime;
        offset = ampltude * Mathf.Sin(time * frequency);
        transform.position = startPos + new Vector3(0, offset, 0);
        transform.Rotate(new Vector3(0, rotationSpeed, 0));
    }
}
