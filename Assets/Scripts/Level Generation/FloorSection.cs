public class FloorSection : PoolableObject
{
    public int Index;
    public delegate void ReachEndEvent(FloorSection Section);
    public ReachEndEvent OnReachEnd;
    public ReachEndEvent OnReachBeginning;

    public override void OnDisable()
    {
        OnReachBeginning = null;
        OnReachEnd = null;
        base.OnDisable();
    }
}
