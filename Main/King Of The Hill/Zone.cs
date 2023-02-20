using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] LayerMask defaultLayer;
    [SerializeField] GameObject beam;
    [SerializeField] GameObject zone;
    [SerializeField] Transform icosphereTop;
    [SerializeField] float BeamYScaling;
    [SerializeField] float beamOffsetMultiplier;
    [SerializeField] float pointsGainedPerSecond;

    float beamOffset;


    private void Update()
    {
        RaycastHit raycastHit;

        Ray beamRay = new Ray(beam.transform.position, Vector3.down);

        Debug.DrawRay(beam.transform.position, Vector3.down * 1000f);

        if (Physics.Raycast(beamRay, out raycastHit, 1000f, ~defaultLayer))
        {
            zone.transform.localPosition = raycastHit.point;

            //calc dist btw raycast hit and beam pos and adjust scaling based off of this
            float distFromHit = Mathf.Round(Vector3.Distance(icosphereTop.position, raycastHit.point));

            //as distance gets higher increase beam offset

            Vector3 newBeamScale = new Vector3(beam.transform.localScale.x, distFromHit * BeamYScaling, beam.transform.localScale.z);

            beam.transform.localScale = newBeamScale;
        }

        // Debug.DrawRay(beam.transform.position, -transform.up);


        //calc dist btw raycast hit and beam pos and adjust scaling based off of this
        //float distFromHit = Vector3.Distance(beam.transform.position, raycastHit.collider.transform.position);
        //Vector3 newBeamScale = new Vector3(beam.transform.localScale.x, distFromHit * BeamYScaling, beam.transform.localScale.z);

        // beam.transform.localScale = newBeamScale;
        //zone.transform.localPosition = raycastHit.collider.transform.position;
    }

    //Each player needs their own point value
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }

        //Get ZoneScorer component
        PlayerZoneScorer otherZoneScorer = other.transform.root.GetChild(2).GetChild(0).GetComponent<PlayerZoneScorer>();

        //Add points
        otherZoneScorer.SetPoints(Mathf.RoundToInt(otherZoneScorer.GetPoints() + (pointsGainedPerSecond * Time.deltaTime)));

    }
}
