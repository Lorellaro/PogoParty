using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    private Transform[] points;

    private int _pointIndex;

    private Transform _currentPoint;

    [Range(1, 20)]
    [SerializeField] private float speed = 5f;

    [SerializeField] private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Activate(Transform[] pathPoints)
    {
        points = pathPoints;
        transform.position = points[0].position;
        GoToNextPos();
    }

    // Update is called once per frame
    void Update()
    {
        if(_pointIndex == -1) return;
        if (Vector3.Distance(_currentPoint.position, transform.position) < 5)
        {
            GoToNextPos();
        }
        else
        {
            var step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, _currentPoint.position, step);
            transform.position += offset;
            
            var targetRotation = Quaternion.LookRotation(_currentPoint.position - transform.position);
            targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y + 90,
                targetRotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (speed / 2) * Time.deltaTime);
            // transform.Rotate(0,90,0);
        }
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

    }
}
