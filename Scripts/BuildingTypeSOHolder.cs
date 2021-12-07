using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingTypeSOHolder : MonoBehaviour
{
    public BuildingTypeSO buidlingTypeSO;

    private bool _isConnectToCenter;

    public List<Worker> _workers;











    public bool GetIsConnectToCenter()
    {
        return _isConnectToCenter;
    }

    public void SetIsConnectToCenter(bool connect)
    {
        _isConnectToCenter = connect;
    }
}
