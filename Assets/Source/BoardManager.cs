using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Vector3Int origin;
    [SerializeField] private Vector2Int size;
    [SerializeField] private Tile[] tileVariants;
    [SerializeField] private Structure[] structureVariants;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask tileLayerMask;
    [SerializeField] private LayerMask structureLayerMask;
    [SerializeField] private VisualEffect tileVFX;

    private static BoardManager instance;

    private const float noiseScale = 0.2f;
    private const float startCurrencyUnits = 200f;
    private const float startResearchUnits = 0f;
    private const float tileHeight = 1f;
    private const float updatePeriod = 1f;

    private List<Tile> tiles = new();
    private List<Structure> structures = new();

    private bool isBuilding = false;
    private bool isDemolishing = false;
    private string activeStructureID = "base";

    private float lastUpdateTime = 0f;

    private void Awake()
    {
        // Ensuring only one instance of the class exists in the scene:
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Getting parameters from memory:
        bool isNewGame = SerializeManager.GetNewGameState();

        float tileGenOffset = SerializeManager.GetTileGenOffset();

        if (isNewGame)
        {
            SerializeManager.SetNewGameState(false);

            SerializeManager.SetCurrencyUnits(startCurrencyUnits);
            SerializeManager.SetResearchUnits(startResearchUnits);

            foreach (Structure structure in structureVariants)
            {
                SerializeManager.SetStructureLockedState(structure.ID, true);
            }

            tileGenOffset = Random.Range(-99999f, 99999f);
            SerializeManager.SetTileGenOffset(tileGenOffset);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    SerializeManager.SetStructureID(new Vector2(x, y), "none");
                }
            }
        }

        // Looping through grid coordinates:
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                // Generating terrain:
                float perlinValue = Mathf.Clamp(Mathf.PerlinNoise(x * Mathf.Clamp(noiseScale, 0f, 1f) + tileGenOffset,
                                    y * Mathf.Clamp(noiseScale, 0f, 1f) + tileGenOffset), 0f, 1f);

                Tile tile = null;
                float closestSurfaceLevel = float.MaxValue;

                foreach (Tile t in tileVariants)
                {
                    if (perlinValue <= t.SurfaceLevel && t.SurfaceLevel <= closestSurfaceLevel)
                    {
                        tile = t;
                        closestSurfaceLevel = t.SurfaceLevel;
                    }
                }

                if (tile == null)
                    tile = tileVariants[0];

                Tile newTile = Instantiate(tile.gameObject, new Vector3(origin.x + x, origin.y, origin.z + y), Quaternion.identity, transform).GetComponent<Tile>();
                tiles.Add(newTile);

                // Generating structures:
                string structureID = SerializeManager.GetStructureID(new Vector2(x, y));

                foreach (Structure structure in structureVariants)
                {
                    if (structure.ID == structureID)
                    {
                        Structure newStructure = Instantiate(structure.gameObject, new Vector3(origin.x + x, origin.y + tileHeight, origin.z + y), Quaternion.identity, transform).GetComponent<Structure>();
                        structures.Add(newStructure);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (lastUpdateTime < Time.time - updatePeriod)
        {
            foreach (Structure structure in structures)
            {
                SerializeManager.SetCurrencyUnits(SerializeManager.GetCurrencyUnits() + (structure.CurrencyUnitsPerMinute / 60f));
                SerializeManager.SetResearchUnits(SerializeManager.GetResearchUnits() + (structure.ResearchUnitsPerMinute / 60f));

                lastUpdateTime = Time.time;
            }
        }

        tileVFX.transform.position = new Vector3(1000f, 0f, 0f);

        if (isBuilding)
            Build();
        else if (isDemolishing)
            Demolish();
    }

    private void Build()
    {
        tileVFX.pause = true;

        Structure active = null;

        foreach (Structure structure in structureVariants)
        {
            if (structure.ID == activeStructureID)
            {
                active = structure;
                break;
            }
        }

        if (active == null)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, tileLayerMask))
            return;

        if (!hit.collider.gameObject.TryGetComponent(out Tile tile))
            return;

        tileVFX.pause = false;
        tileVFX.SetBool("IsSuitable", false);
        tileVFX.transform.position = tile.transform.position + new Vector3(0.5f, 0.5f, 0.5f);

        if (!tile.IsSuitableForBuilding)
            return;

        if (Physics.Raycast(tile.transform.position + new Vector3(0.5f, 0.5f, 0.5f), Vector3.up, 10f, structureLayerMask))
            return;

        if (SerializeManager.GetCurrencyUnits() < active.BuildPrice)
            return;

        tileVFX.SetBool("IsSuitable", true);

        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;

        Vector3 localPosition = tile.transform.position - origin;
        Vector2Int gridPosition = new((int)Mathf.Floor(localPosition.x), (int)Mathf.Floor(localPosition.z));
        SerializeManager.SetStructureID(gridPosition, active.ID);

        SerializeManager.SetCurrencyUnits(SerializeManager.GetCurrencyUnits() - active.BuildPrice);

        Structure newStructure = Instantiate(active.gameObject, tile.transform.position + Vector3.up, Quaternion.identity, transform).GetComponent<Structure>();

        structures.Add(newStructure);

        newStructure.Initialize();
    }

    private void Demolish()
    {
        tileVFX.pause = true;

        Ray ray1 = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray1, out RaycastHit hit1, 100f, tileLayerMask))
        {
            if (hit1.collider.gameObject.TryGetComponent(out Tile tile))
            {
                tileVFX.pause = false;
                tileVFX.SetBool("IsSuitable", false);
                tileVFX.transform.position = tile.transform.position + new Vector3(0.5f, 0.5f, 0.5f);
            }
        }

        Ray ray2 = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray2, out RaycastHit hit2, 100f, structureLayerMask))
            return;

        if (!hit2.collider.gameObject.TryGetComponent(out Structure structure))
            return;

        if (!structures.Contains(structure)) return;

        tileVFX.SetBool("IsSuitable", true);

        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;

        Vector3 localPosition = structure.transform.position - origin;
        Vector2Int gridPosition = new((int)Mathf.Floor(localPosition.x), (int)Mathf.Floor(localPosition.z));
        SerializeManager.SetStructureID(gridPosition, "none");

        SerializeManager.SetCurrencyUnits(SerializeManager.GetCurrencyUnits() + structure.BuildPrice);

        structures.Remove(structure);

        structure.Terminate();
    }

    public static void SetBuildingState(bool value)
    {
        instance.isBuilding = value;
    }

    public static bool GetBuildingState()
    {
        return instance.isBuilding;
    }

    public static void SetDemolishingState(bool value)
    {
        instance.isDemolishing = value;
    }

    public static bool GetDemolishingState()
    {
        return instance.isDemolishing;
    }

    public static void SetActiveStructureID(string value)
    {
        instance.activeStructureID = value;
    }

    public static string GetActiveStructureID()
    {
        return instance.activeStructureID;
    }
}