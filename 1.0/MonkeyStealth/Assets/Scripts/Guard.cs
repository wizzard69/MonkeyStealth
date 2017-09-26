using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;

    public Transform player;
    public Transform pathHolder;
    public float speed = 5f;
    public float waitTime = 0.3f;
    public float turnSpeed = 90f;
    public Light spotLight;
    public float viewDistance;
    public LayerMask viewMask;
    public float spottedCountDown = 0.5F;
    float viewAngle;
    Color originalSpotlightColour;
    float countDowntimer;

    void Start()
    {
        viewAngle = spotLight.spotAngle;
        originalSpotlightColour = spotLight.color;

        Vector3[] waypoints = new Vector3[pathHolder.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);

        }

        StartCoroutine(FollowPath(waypoints));
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            countDowntimer += Time.deltaTime;
        }
        else
        {
            countDowntimer -= Time.deltaTime;
        }

        countDowntimer = Mathf.Clamp(countDowntimer, 0, spottedCountDown);
        spotLight.color = Color.Lerp(originalSpotlightColour, Color.red, countDowntimer / spottedCountDown);

        if (countDowntimer >= spottedCountDown)
        {
            if (OnGuardHasSpottedPlayer != null)
            {
                OnGuardHasSpottedPlayer();
            }
        }
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);

                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }

            yield return null;
        }        
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);

            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            float angleBetween = Vector3.Angle(transform.forward, dirToPlayer);

            if (angleBetween < viewAngle / 2F)
            {
                if (!Physics.Linecast(transform.position,player.position, viewMask))
                {
                    return true;
                }                
            }
        }

        return false;
    }
}
