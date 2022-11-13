
using UnityEngine;

public class BooMechanic : MonoBehaviour
{

    Camera playerCamera;
    public bool isSpotted;

    Plane[] planes;

    private void Awake()
    {
        playerCamera = Camera.main;
    }

    // ToDo: Racast to see if object is inbetween Player and Boo
    private bool IsSpotted(Camera playerCamera)
    {
        planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);

        foreach(var plane in planes)
        {
            if (plane.GetDistanceToPoint(transform.position) < 0)
            {
                return false; // NOT SPOTTED
            }
        }
        return true; // SPOTTED
    }

    private void Update()
    {
        if(IsSpotted(playerCamera))
        {
            isSpotted = true;
        }
        else
        {
            isSpotted = false;
        }

    }

}
