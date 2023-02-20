using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.GameHandlers.Teams
{
    [System.Serializable]
    public class playersInTeam
    {
        public List<GameObject> players;
        public int score;
    }

    [System.Serializable]
    public class Teams
    {
        public List<playersInTeam> team;
    }
}

