using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RailInformation
{
    private List<Vector3> waypoints = new List<Vector3>();
    private int current = 0;
    private int direction = 1; //um den richtigen Waypoint zu bestimmen: closest to player und danach richtung wp - player normalisiern und den mit weniger abweichung von camera direction nehmen
    private bool setDirection;

    public RailInformation(GameObject rail)
    {
        foreach(Transform t in rail.transform)
        {
            Vector3 pos = t.position;
            pos.y -= 3f;
            waypoints.Add(pos);
        }
    }

    public void SetCurrentWaypointToClosest(Vector3 position, Vector3 camDir)
    {
        float minDistance =  waypoints.Min(wp => Vector3.Distance(position, wp));  //TODO: testen

        Vector3[] closests = GetClosestWaypoints(position);
        Vector3[] dirs = new Vector3[] { (closests[0] - position).normalized, (closests[1] - position).normalized };
        Vector3 final = Vector3.Distance(dirs[0], camDir) < Vector3.Distance(dirs[1], camDir) ? closests[0] : closests[1];
        current = waypoints.IndexOf(final);
        setDirection = true;
    }

    private Vector3[] GetClosestWaypoints(Vector3 pos)
    {
        Vector3 closest = Vector3.zero;
        Vector3 secondClosest = Vector3.zero;

        foreach(Vector3 vec in waypoints)
        {
            if(Vector3.Distance(pos, vec) < Vector3.Distance(pos, closest))
            {
                closest = vec;
            }
            else if(Vector3.Distance(pos, vec) < Vector3.Distance(pos, secondClosest))
            {
                secondClosest = vec;
            }
        }

        return new Vector3[] { closest, secondClosest };
    }

    public Vector3 GetCurrentWaypoint()
    {
        return waypoints[current];
    }

    private void SetDirection(Vector3 pos, Vector3 camDir)
    {
        Vector3 priorDir = (waypoints[current - 1] - pos).normalized;
        Vector3 nextDir = (waypoints[current + 1] - pos).normalized;
        direction = Vector3.Distance(priorDir, camDir) < Vector3.Distance(nextDir, camDir) ? -1 : 1;
        setDirection = false;
    }

    public void IncreaseWaypoint(Vector3 pos, Vector3 camDir)
    {
        if (setDirection) SetDirection(pos, camDir);
        current += direction;
    }
}

