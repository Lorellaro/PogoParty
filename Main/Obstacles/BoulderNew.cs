using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BoulderNew : MonoBehaviour
{
     [SerializeField] float pelletForce = 1000f;
    [SerializeField] float forceApplicationTimer;
    [SerializeField] float knockupForce = 1000f;
    [SerializeField] private float destroyTime = 5f;
    [SerializeField] private float minThrowerBoostTime = 0.2f;
    [SerializeField] private float maxThrowerBoostTime = 0.8f;
    private float _throwerBoostTime;

    private Vector3 _direction;
    private Vector3 _knockBackDirection;
    private Rigidbody _rb;
    private float _currentTime;
    public GameObject destructible;
    private bool _destroyed = false;
    private bool _canPlay = true;
    private bool _audioPlaying = false;
    public ParticleSystem pSystem;
    private AudioSource _rockColliding;
    [FormerlySerializedAs("_boulderThrower")] public BoulderThrower boulderThrower;
    public Vector3 throwerBoostDir;

    private void Awake()
    {
        // _boulderThrower = BoulderThrower.Instance;
        
    }

    private void Start()
    {
        _direction = boulderThrower.GetTarget();
        _knockBackDirection = boulderThrower.GetKnockBackDir();
        _rockColliding = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        // direction = transform.parent.gameObject.GetComponent<pelletThrower>().getTarget();//set pellet throwing direction to parent's target
        StartCoroutine(destroyBoulder());
        // knockBackDirection = transform.parent.GetChild(1).forward;
        
        _throwerBoostTime = Random.Range(minThrowerBoostTime, maxThrowerBoostTime);
        
        
    }

    public void SetDirections(Vector3 direction, Vector3 knockBackDirection)
    {
        _direction = direction;
        _knockBackDirection = knockBackDirection;
    }
    
    // Start is called before the first frame update
    void FixedUpdate()
    {
        if(!_destroyed){
            // _rb.AddTorque(Vector3.right * 20);
            // transform.Rotate(Vector3.back, 5f * 20 * Time.deltaTime);
            if (_currentTime < _throwerBoostTime)
            {
                _rb.velocity += throwerBoostDir * (pelletForce * Time.deltaTime);
            }
            // if time is less than the max time 
            else if (_currentTime > _throwerBoostTime && _currentTime < forceApplicationTimer + _throwerBoostTime)
            {
                // apply force to rigidbody
                _rb.velocity += _direction * (pelletForce * Time.deltaTime);
            }
        }
        
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!_destroyed){
            if (collision.gameObject.CompareTag("Player"))
            {
                GameObject playerObj = collision.gameObject;
                Rigidbody playerRB = playerObj.GetComponent<Rigidbody>();
                playerRB.velocity += _knockBackDirection * knockupForce * Time.deltaTime;
            }
           
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground" && _canPlay){
                StartCoroutine(playParticles());
                pSystem.Play();
            }
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground" && !_audioPlaying){
                StartCoroutine(playSound());
                _rockColliding.Play();
            }
        }
        
    }
    private IEnumerator destroyBoulder(){
        // wait then destroy
        yield return new WaitForSeconds(destroyTime);
        boulderThrower.BoulderDestroyed();
        Destroy(gameObject);
    }
    private IEnumerator playParticles(){
        _canPlay = false;
        yield return new WaitForSeconds(0.3f);
        _canPlay = true;
    }
    private IEnumerator playSound(){
        _audioPlaying = true;
        yield return new WaitForSeconds(_rockColliding.clip.length);
        _audioPlaying = false;
    }
}
