using UnityEngine;

public class scr_AnimalSpawns : MonoBehaviour
{
    [SerializeField] public GameObject[] Animals;
    [SerializeField] public scr_AnimalEnvironment[] Interactions;
    [SerializeField] private scr_PlaceObject PlaceObjectController;
    private float GameTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlaceObjectController == null)
            PlaceObjectController = FindFirstObjectByType<scr_PlaceObject>();
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
        foreach (scr_AnimalEnvironment Interaction in Interactions)
            CheckSpawns(Interaction);
    }

    private GameObject CheckSpawns(scr_AnimalEnvironment Interaction)
    {
        if (Interaction == null || PlaceObjectController == null)
            return null;

        GameObject associatedAnimal = null;
        foreach (GameObject animal in Animals)
        {
            if (animal != null && animal.name == Interaction.AName.ToString())
            {
                associatedAnimal = animal;
                break;
            }
        }

        int placedObjectCount = Interaction.EName switch
        {
            EnvironmentName.pre_Tree => PlaceObjectController.TreeCount,
            EnvironmentName.pre_Bush => PlaceObjectController.BushCount,
            EnvironmentName.pre_Flower => PlaceObjectController.FlowerCount,
            _ => 0
        };

        return placedObjectCount >= Interaction.ObjectCount ? associatedAnimal : null;
    }
    private void Spawn()
    {

    }
}
