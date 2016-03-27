using UnityEngine;
using System.Collections;

public class SlideScript : MonoBehaviour
{
    public float speed = 1;  TODO: sliding devblog
    private PlayerControl playerControl;

    private RailInformation railInformation;
    public RailInformation RailInformation
    {
        set
        {
            this.railInformation = value;
            railInformation.SetCurrentWaypointToClosest(transform.position, transform.forward);
        }
    }
    public Vector3 Velocity { get; private set; }
    private float acceleration = .8f;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        Velocity = Vector3.zero;
    }

    void Update()
    {
        if (railInformation.IsAtEnd())
        {
            playerControl.StopSliding();
            return;
        }

        Vector3 current = railInformation.GetCurrentWaypoint();
        Velocity = (current - transform.position).normalized * speed * acceleration * Time.deltaTime * 15 * 1.5f;

        if (Velocity.y < .05f) acceleration *= (1 + Time.deltaTime / 2);
        else if (Velocity.y > .05f) acceleration /= (1 + Time.deltaTime / 2);

        IncreaseWaypoints(transform.position, Velocity);
        transform.position += Velocity;
    }

    private void IncreaseWaypoints(Vector3 start, Vector3 delta)
    {
        Vector3 current = railInformation.GetCurrentWaypoint();
        Vector3 currentPos = start;
        Vector3 end = start + delta;
        Vector3 dir = delta.normalized / 10;

        for (int i = 0; i < 20; i++)
        {
            currentPos += dir;

            if (Vector3.Distance(currentPos, current) < .1f)
            {
                railInformation.IncreaseWaypoint(transform.position, transform.forward);
            }
            if (Vector3.Distance(currentPos, end) < .1f) return;
        }
    }
}
