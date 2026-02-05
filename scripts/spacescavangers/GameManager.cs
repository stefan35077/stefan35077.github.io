using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    PlayerController PC;

    public int credit = 0;
    public int currencyInflation = 10;

    List<ResourceInstance> resources = new List<ResourceInstance>();
    List<ResourceField> fields = new List<ResourceField>();
    List<GameObject> cullObjects = new List<GameObject>();
    float occlusionUpdateTime = 1f;
    float occlusionTimer;

    #region GAME SETTINGS
    [Header("Game Settings")]
    public int volume = 100;
    [Tooltip("Fullscreen enabled")]
    public bool FullScreen = true;
    [Tooltip("the radius around the player where objects should render and simulate")]
    public float occlusionRadius = 100;
    [Tooltip("how large the world should be, only applies if levelbuilder worldRadius is set to 0")]
    public int worldSize = 750;
    [Tooltip("full world load disables occlusion and allows constant physics simulation, only use on small worlds or with a good PC")]
    public bool fullWorldLoad = false;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(GetComponent<GameManager>());
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScore();
    }

    void Start()
    {
        //add random infaltion value
        currencyInflation = Random.Range(7, 18);
        //reset credit on start
        credit = 0;
    }

    void Update()
    {
        Screen.fullScreen = FullScreen;

        if (!PC)
        {
            PC = PlayerController.Instance;
            return;
        }

        occlusionTimer += Time.deltaTime;
        if (occlusionTimer >= occlusionUpdateTime)
        {
            ResourceOcclusion();
            occlusionTimer = 0;
        }
    }

    [BurstCompile]
    void ResourceOcclusion()
    {
        if(fullWorldLoad)
        {
            return;
        }

        foreach (ResourceInstance g in resources)
        {
            if (Vector3.Distance(PC.transform.position, g.transform.position) <= occlusionRadius)
            {
                g.gameObject.SetActive(true);
            }
            else
            {
                g.gameObject.SetActive(false);
            }
        }
        foreach (GameObject g in cullObjects)
        {
            if (Vector3.Distance(PC.transform.position, g.transform.position) <= occlusionRadius)
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
        }

        for (int x = 0; x < fields.Count; x++)
        {
            ResourceField f = fields[x];
            if(Vector3.Distance(PC.transform.position, f.transform.position) > occlusionRadius)
            {
                continue;
            }

            if(f.resourceList.Count <= 0)
            {
                fields.Remove(f);

                //set the ring to fade out
                ParticleSystem ring = f.GetComponentInChildren<ParticleSystem>();
                //ingore the depricated error
                ring.loop = false;
                ring.transform.SetParent(null, true);

                //remove all children
                while (f.transform.childCount > 0)
                {
                    f.transform.GetChild(0).transform.SetParent(transform.parent, true);
                }

                Destroy(f.gameObject);
            }
            else
            {
                for(int i = 0; i < f.resourceList.Count; i++)
                {
                    ResourceInstance r = f.resourceList[i];
                    if(!r.gameObject.activeSelf || Vector3.Distance(f.transform.position, r.transform.position) > f.spawnRadius * 2)
                    {
                        f.resourceList.Remove(r);
                    }
                }
            }
        }
    }

    public void AddCredit(int amount)
    {
        credit += amount;
        PlayerController.Instance.UpdateCreditAmount();
    }
    public void RemoveCredit(int amount)
    {
        credit -= amount;
        PlayerController.Instance.UpdateCreditAmount();
    }

    public void AddOcclusionObject(GameObject toAdd)
    {
        if(!cullObjects.Contains(toAdd))
        {
            cullObjects.Add(toAdd);
        }
    }
    public void RemoveOcclusionResource(ResourceInstance toRemove)
    {
        if(resources.Contains(toRemove))
        {
            resources.Remove(toRemove);
        }
    }

    public ResourceInstance[] GetResourceList()
    {
        return resources.ToArray();
    }

    public string GetItemRarity(int from)
    {
        switch (from)
        {
            case 0:
                return "COMMON";
            case 1:
                return "UNCOMMON";
            case 2:
                return "RARE";
            case 3:
                return "EPIC";
            case 4:
                return "LEGENDARY";
            case 5:
                return "ULTRA";
            default:
                return "UNKNOWN";
        }
    }

    public void LoadResources()
    {
        resources.Clear();
        resources.AddRange(FindObjectsByType<ResourceInstance>(FindObjectsSortMode.None));
        fields.Clear();
        fields.AddRange(FindObjectsByType<ResourceField>(FindObjectsSortMode.None));
    }

    #region SCENE LOADING
    public void StartGame()
    {
        credit = 0;
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        //clear the resource list
        resources.Clear();
        //destroy the player
        Destroy(PlayerController.Instance.gameObject);

        if(TryGetComponent<LevelBuilder>(out LevelBuilder builder))
        {
            Destroy(builder);
        }

        SaveScore();

        Time.timeScale = 1f; // Ensure time scale is reset before loading main menu
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        //clear the resource list
        resources.Clear();
        //destroy the player
        Destroy(PlayerController.Instance.gameObject);

        if (TryGetComponent<LevelBuilder>(out LevelBuilder builder))
        {
            Destroy(builder);
        }

        SaveScore();

        Time.timeScale = 1f; // Ensure time scale is reset before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CloseGame()
    {
        print("Closing");
        SaveScore();
        Application.Quit();
    }

    #endregion

    #region SAVE SYSTEM
    public int[] savedScores = new int[10];
    void SaveScore()
    {
        SetScoreToList();

        for(int i = 0; i < savedScores.Length; i++)
        {
            string key = "SPCSLV-SCORE-" + i.ToString("00");
            PlayerPrefs.SetInt(key, savedScores[i]);
        }

        PlayerPrefs.Save();
        credit = 0;
    }
    void LoadScore()
    {
        for (int i = 0; i < savedScores.Length; i++)
        {
            string key = "SPCSLV-SCORE-" + i.ToString("00");
            savedScores[i] = PlayerPrefs.GetInt(key);
        }
    }
    void SetScoreToList()
    {
        List<int> ordered = new List<int>();
        ordered.AddRange(savedScores);
        ordered.Add(credit);

        for(int x = 0; x < ordered.Count; x++)
        {
            for(int y = 0; y < ordered.Count; y++)
            {
                if (ordered[x] > ordered[y])
                {
                    int temp = ordered[x];
                    ordered[x] = ordered[y];
                    ordered[y] = temp;
                }
            }
        }

        for (int i = 0; i < savedScores.Length; i++)
        {
            savedScores[i] = ordered[i];
        }
    }
    #endregion
}
