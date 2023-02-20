using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Level.Restaurant.Tables;
using Photon.Pun;

public class RestaurantTableHandler : MonoBehaviour
{
    [SerializeField] Table tables;

    public int GetAvailableTableIndex()
    {
        //Chronologically finds table
        for(int i = 0; i < tables.table.Count; i++)
        {
            if (!tables.table[i].inUse) { return i; }
        }

        //Failed
        return -1;

    }

    [PunRPC]
    public void SetTableUnavailable(int tableIndex)
    {
        tables.table[tableIndex].inUse = true;
    }

    [PunRPC]
    public void SetTableAvailable(int tableIndex)
    {
        tables.table[tableIndex].inUse = false;
    }

    public Vector3 GetTablePosition(int tableIndex)
    {
        print(tableIndex);
        return tables.table[tableIndex].tableCentrePosition;
    }

    public Vector3 GetTableDishPosition(int tableIndex, int _dishTablePositionIndex)
    {
        if(_dishTablePositionIndex == 0)
        {
            return tables.table[tableIndex].dish1Pos;
        }
        else if(_dishTablePositionIndex == 1)
        {
            return tables.table[tableIndex].dish2Pos;
        }
        else if(_dishTablePositionIndex == 2)
        {
            return tables.table[tableIndex].dish3Pos;
        }
        else 
        {
            return tables.table[tableIndex].dish4Pos;
        }
    }
}
