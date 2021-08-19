using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject ObjToSpawn;
    public GameObject LeftToSpawn;
    public GameObject RightToSpawn;
    public GameObject WarningPrefab;
    public GameObject WarningREDPrefab;
    public GameObject WarningSpawn;
    GameController gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
    }

    internal Mover SpawnNormalAt(Transform transform, bool IsFirst = false, bool IsLeft=true)
    {
        Vector3 pos = transform.position;
        if (IsFirst)
        {
            if (IsLeft) pos += new Vector3(0.5f, 0, 0);
            else pos -= new Vector3(0.5f, 0, 0);
        }
        var normalObj = Instantiate(ObjToSpawn, pos, ObjToSpawn.transform.rotation, gameController.InstanciationPlace);
        normalObj.FindComponentInChildWithTag<ObstacleCollider>("Obstacle").SetupWarnings(WarningPrefab, WarningSpawn.transform.position, WarningPrefab.transform.rotation);
        return normalObj.GetComponent<Mover>();
    }

    internal Mover SpawnLeftAt(Transform transform)
    {
        Instantiate(WarningREDPrefab, WarningSpawn.transform.position, LeftToSpawn.transform.rotation, gameController.InstanciationPlace);
        var normalObj = Instantiate(LeftToSpawn, transform.position, LeftToSpawn.transform.rotation, gameController.InstanciationPlace);
        return normalObj.GetComponent<Mover>();
    }

    internal Mover SpawnRightAt(Transform transform)
    {
        Instantiate(WarningREDPrefab, WarningSpawn.transform.position, LeftToSpawn.transform.rotation, gameController.InstanciationPlace);
        var normalObj = Instantiate(RightToSpawn, transform.position, RightToSpawn.transform.rotation, gameController.InstanciationPlace);
        return normalObj.GetComponent<Mover>();
    }
}
