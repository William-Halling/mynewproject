using System;
using System.Collections.Generic;
using UnityEngine;

namespace Transporter.Data
{
[Serializable]
    public class WorldData
    {
        public int seed;
        public List<LocationData> locations = new();
    }


    [Serializable]
    public class LocationData
    {
        public string id;
        public string name;
        public Vector3 position;
    }
}       