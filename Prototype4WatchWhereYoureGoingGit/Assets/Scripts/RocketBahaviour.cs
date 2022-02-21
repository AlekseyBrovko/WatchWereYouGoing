using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBahaviour : MonoBehaviour
{
    private GameObject[] _enemies;
    private Transform _target;

    private float _speed = 15.0f;
    private float _rocketStrength = 15.0f;
    private float _aliveTimer = 5.0f;

    private bool _homing;


    public void Fire(Transform newTarget)
    {        
        _target = newTarget;
        _homing = true;
        Destroy(gameObject, _aliveTimer);
    }

    private void Update()
    {        
        if(_homing && _target !=null)
        {
            Vector3 moveDirection = (_target.transform.position - transform.position).normalized;
            transform.position += moveDirection * _speed * Time.deltaTime;
            transform.LookAt(_target);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (_target != null)
        {
            if(col.gameObject.CompareTag(_target.tag))
            {
                Rigidbody targetRigidbody = col.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -col.contacts[0].normal;
                targetRigidbody.AddForce(away * _rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }
}
