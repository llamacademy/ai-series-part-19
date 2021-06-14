using UnityEngine;

public class FloorEndTrigger : MonoBehaviour
{
    public bool IsEnd = false;
    [SerializeField]
    private FloorSection Parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (IsEnd)
            {
                Parent?.OnReachEnd?.Invoke(Parent);
            }
            else
            {
                Parent?.OnReachBeginning?.Invoke(Parent);
            }
        }
    }
}
