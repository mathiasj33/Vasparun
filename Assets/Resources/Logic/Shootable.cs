public class Shootable
{
    private int numShotsNeeded;
    private int numShot;
    public bool Shot { get { return numShotsNeeded == numShot; } }

    public Shootable(int numShotsNeeded)
    {
        this.numShotsNeeded = numShotsNeeded;
    }

    public void ShootAt()
    {
        numShot++;
    }

    public void Revert()
    {
        numShot = 0;
    }
}

