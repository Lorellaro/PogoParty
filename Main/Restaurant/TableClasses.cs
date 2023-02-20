using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Level.Restaurant.Tables
{
    [System.Serializable]
    public class TableData
    {
        public bool inUse = false;

        public Vector3 tableCentrePosition;
        public Vector3 dish1Pos;
        public Vector3 dish2Pos;
        public Vector3 dish3Pos;
        public Vector3 dish4Pos;
    }

    [System.Serializable]
    public class Table
    {
        public List<TableData> table;
    }
}

