using System.Collections.Generic;
using UnityEngine;
using Transporter.Data;
using Transporter.World;

public class WorldManager : MonoBehaviour
{
public static WorldManager Instance { get; private set; }

    [SerializeField] private int defaultSeed = 12345;
    private int currentSeed;

    private readonly List<Location> locations = new();
    public IReadOnlyList<Location> Locations => locations;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void GenerateWorld(int seed)
    {
        currentSeed = seed;
        Random.InitState(seed);

        locations.Clear();
        locations.Add(new Location("EAST", "Eastport", new Vector3(800, 0, 0)));
        locations.Add(new Location("WEST", "Westport", new Vector3(-800, 0, 0)));
        locations.Add(new Location("NORTH", "Northbay", new Vector3(0, 0, 900)));
        locations.Add(new Location("SOUTH", "Southreef", new Vector3(0, 0, -900)));
    }


    public void GenerateDefault()
    {
        GenerateWorld(defaultSeed);
    }


    // ✅ SRP snapshot
    public WorldData SaveState()
    {
        WorldData data = new WorldData
        {
            seed = currentSeed
        };

        foreach (var loc in locations)
        {
            data.locations.Add(new LocationData
            {
                id = loc.Id,
                name = loc.Name,
                position = loc.WorldPosition
            });
        }

        return data;
    }


    // ✅ SRP restore
    public void LoadState(WorldData data)
    {
        currentSeed = data.seed;
        locations.Clear();

        foreach (var loc in data.locations)
        {
            locations.Add(new Location(loc.id, loc.name, loc.position));
        }
    }
}
