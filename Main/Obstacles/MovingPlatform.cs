using System;
using System.Collections;
using UnityEngine;
// [DefaultExecutionOrder(1)]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField]Transform[] points;
    private Transform _currentPoint;

    private int _pointIndex = -1;

    private float _journeyLength;
    private float _startTime;

    [Range(0.001f, 0.1f)]
    [SerializeField] private float _speed = 0.03f;

    private PogoStickPhysics _player;
    private bool _waiting = false;
    [SerializeField] private float waitingTime = 1.0f;
    private bool _isPlayerOnPlatform;
    [SerializeField] private bool _singleUse = false;

    // Start is called before the first frame update
    void Start()
    {
        // _player = GameObject.FindWithTag(Tags.Player).GetComponent<PogoStickPhysics>();
        _player = FindObjectOfType<PogoStickPhysics>();
        // _player = GameObject.Find("GameControllers").GetComponent<GameController>().player;
        //Debug.Log(_player.name);
        GoToNextPos();
    }

    void GoToNextPos()
    {
        if (points.Length == 0)
        {
            Debug.LogError("No points set!", transform);
        }

        if (_pointIndex == -1)
        {
            _pointIndex = 0;
        }
        

        _currentPoint = points[_pointIndex];
        ++_pointIndex;
        _pointIndex %= points.Length;
        
        _journeyLength = Vector3.Distance(_currentPoint.position, transform.position);
        _startTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if(_pointIndex == -1) return;
        if (Vector3.Distance(_currentPoint.position, transform.position) < 0.5f)
        {
            if (!_waiting)
            {
                StartCoroutine(WaitToGoToNextPos(waitingTime));
            }
        }
        else
        {
            float distCovered = (Time.time - _startTime) * _speed;
            float fractionOfJourney = distCovered / _journeyLength;
            transform.position = Vector3.Lerp(transform.position, _currentPoint.position, fractionOfJourney);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Debug.Log("Player on platform");
            _player.transform.parent = transform;
            if (_pointIndex == -1 && _singleUse)
            {
                StartCoroutine(WaitToGoToNextPos(waitingTime));
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            _player.transform.parent = _player.pogoParent.transform;
        }
    }

    private IEnumerator WaitToGoToNextPos(float waitTime)
    {
        _waiting = true;
        yield return new WaitForSeconds(waitTime);
        GoToNextPos();
        _waiting = false;
    }

    public void StartPlatform()
    {
        Debug.Log("Started platform");
        // StartCoroutine(WaitToGoToNextPos(waitingTime));
    }

}
