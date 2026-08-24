using System.Collections.Generic;
using UnityEngine;

public class scr_AnimalSpawns : MonoBehaviour
{
    [SerializeField] public GameObject[] Animals;
    [SerializeField] public scr_AnimalEnvironment[] Interactions;
    [SerializeField] private scr_PlaceObject PlaceObjectController;
    private readonly Dictionary<scr_AnimalEnvironment, float> SpawnTimes = new();
    private readonly Dictionary<EnvironmentName, int> LastLoggedObjectCounts = new();
    private readonly Dictionary<scr_AnimalEnvironment, int> SpawnedAnimalCounts = new();
    private float GameTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlaceObjectController == null)
            PlaceObjectController = FindFirstObjectByType<scr_PlaceObject>();

        foreach (scr_AnimalEnvironment interaction in Interactions)
        {
            if (interaction == null)
                continue;

            SpawnTimes[interaction] = Random.Range(
                Mathf.Min(interaction.TimeLowerBound, interaction.TimeUpperBound),
                Mathf.Max(interaction.TimeLowerBound, interaction.TimeUpperBound));
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
        foreach (scr_AnimalEnvironment Interaction in Interactions)
            CheckSpawns(Interaction);
    }

    private void CheckSpawns(scr_AnimalEnvironment interaction)
    {
        if (interaction == null || PlaceObjectController == null)
            return;

        int associatedEnvironmentCount = interaction.EName switch
        {
            EnvironmentName.pre_Tree => PlaceObjectController.TreeCount,
            EnvironmentName.pre_Bush => PlaceObjectController.BushCount,
            EnvironmentName.pre_Flower => PlaceObjectController.FlowerCount,
            _ => 0
        };

        if (!LastLoggedObjectCounts.TryGetValue(interaction.EName, out int lastLoggedCount) ||
            lastLoggedCount != associatedEnvironmentCount)
        {
            LastLoggedObjectCounts[interaction.EName] = associatedEnvironmentCount;
        }

        GameObject associatedAnimal = null;
        foreach (GameObject animal in Animals)
        {
            if (animal != null && animal.name == interaction.AName.ToString())
            {
                associatedAnimal = animal;
                break;
            }
        }

        if (associatedAnimal == null || interaction.ObjectCount <= 0)
            return;

        if (!SpawnedAnimalCounts.TryGetValue(interaction, out int spawnedAnimalCount))
            spawnedAnimalCount = 0;

        int nextSpawnThreshold = interaction.ObjectCount;
        for (int spawnIndex = 0; spawnIndex < spawnedAnimalCount; spawnIndex++)
            nextSpawnThreshold = Mathf.Min(nextSpawnThreshold * 2, int.MaxValue);

        bool hasEnoughEnvironment = associatedEnvironmentCount >= nextSpawnThreshold;
        bool hasReachedSpawnTime = GameTime >= SpawnTimes[interaction];
        if (hasEnoughEnvironment && hasReachedSpawnTime)
        {
            Spawn(associatedAnimal);
            SpawnedAnimalCounts[interaction] = spawnedAnimalCount + 1;
        }  
    }

    private void Spawn(GameObject animal)
    {
        System.Random rand = new System.Random();
        Vector3 spawnPosition = new Vector3(Random.Range(-10, 11), 5f, rand.Next(-50, 50));
        Instantiate(animal, spawnPosition, Quaternion.identity);
    }
}
