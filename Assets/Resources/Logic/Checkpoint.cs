using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Checkpoint
{
    public Vector3 Position { get; set; }
    public bool AlreadyFinished { get; set; }
    private HashSet<ShootWallControl> revertSet = new HashSet<ShootWallControl>();

    public Checkpoint(Vector3 position)
    {
        this.Position = position;
    }

    public void AddToRevertSet(ShootWallControl control)
    {
        revertSet.Add(control);
    }

    public void Revert()
    {
        revertSet.ToList().ForEach(c => c.Revert());
        revertSet.Clear();
    }

    public void DestroyAllWalls()
    {
        revertSet.ToList().ForEach(c =>
        {
            if (c.Shot) GameObject.Destroy(c.gameObject);
        });
        revertSet.Clear();
    }

    public Checkpoint Clone()
    {
        Checkpoint clone = new Checkpoint(new Vector3(Position.x, Position.y, Position.z));
        clone.revertSet = revertSet;
        return clone;
    }

    public static bool operator ==(Checkpoint a, Checkpoint b)
    {
        if (System.Object.ReferenceEquals(a, b)) return true;
        if (((object)a) == null || ((object)b) == null) return false;
        return a.Position == b.Position;
    }

    public static bool operator !=(Checkpoint a, Checkpoint b)
    {
        return !(a == b);
    }
}

