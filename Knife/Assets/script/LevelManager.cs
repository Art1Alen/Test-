using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Runtime.InteropServices.WindowsRuntime;


public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    

    
    public static LevelManager Instance;

    public Wheel[] wheels;
    public Boss[] bosses;

    [SerializeField] private GameObject knifePrefab;

    [Header(header: "Wheel setting")]
    [SerializeField] private Transform wheelSpawPosition;
    [Range(0, 1)] [SerializeField] private float wheelScale;

    [Header(header: "Knife setting")]
    [SerializeField] private Transform knifeSpawPosition;
    [Range(0, 1)] [SerializeField] private float knifeScale;

    private string bossName;
    private Wheel currentWheel;
    private Knife currentKnife;

    public int TotalSpawnKnife { get; set; }

    public string BossName => bossName;
   

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            
        }       
    }
    private void Start()
    {
        InitializedGame();
    }

    private void Update()
    {
        if (currentKnife == null)
        {
            return;
        }
         
        if (Input.GetMouseButtonDown(0) && !currentKnife.IsReleased)
        {
            KnifeCounter.Instance.KnifeHit(TotalSpawnKnife);
            currentKnife.FireKnife();
            StartCoroutine(GenerateKnife());
        }
    }

    private void InitializedGame()
    {
        GameManager.Instance.IsGameOver = false;
        GameManager.Instance.Score = 0;
        GameManager.Instance.Stage = 1;

        SetupGame();
    }

    private void SetupGame()
    {
       SpawnWheel();
        KnifeCounter.Instance.SetupKnife(currentWheel.AvailableKnifes);

        TotalSpawnKnife = 0;
        StartCoroutine(routine: GenerateKnife());
    }
    private void SpawnWheel()
    {
        GameObject tmpWheel = new GameObject();
        
        if (GameManager.Instance.Stage * 5 == 0)
        {
            Boss newBoss = bosses[Random.Range(0, bosses.Length)];
            tmpWheel = Instantiate(newBoss.bossPrefab, wheelSpawPosition.position, Quaternion.identity, wheelSpawPosition).gameObject;
            bossName = "Boss" + newBoss.bossName;
        }
        else
        {
            tmpWheel = Instantiate(wheels[GameManager.Instance.Stage = 1], wheelSpawPosition.position, 
                Quaternion.identity, wheelSpawPosition).gameObject;
        }

        float wheelScaleInScreen = GameManager.Instance.ScreenWidth * wheelScale / tmpWheel.GetComponent<SpriteRenderer>().bounds.size.x;
        tmpWheel.transform.localScale = Vector3.one * wheelScaleInScreen;
        currentWheel = tmpWheel.GetComponent<Wheel>();
    }

    private IEnumerator GenerateKnife()
    {
        yield return new WaitUntil(predicate: (() => knifeSpawPosition.childCount == 0));

        if (currentWheel.AvailableKnifes > TotalSpawnKnife && !GameManager.Instance.IsGameOver)
        {
            TotalSpawnKnife++;
            GameObject tmpKnife = new GameObject();

            if (GameManager.Instance.SelectedKnifePrefab == null)
            {
                tmpKnife = Instantiate(knifePrefab, knifeSpawPosition.position, Quaternion.identity, knifeSpawPosition).gameObject;
            }
            else
            {
                tmpKnife = Instantiate(GameManager.Instance.SelectedKnifePrefab, knifeSpawPosition.position, Quaternion.identity, knifeSpawPosition).gameObject;
            }

            float KnifeScaleInScreen = GameManager.Instance.ScreenHeight * knifeScale / tmpKnife.GetComponent<SpriteRenderer>().bounds.size.y;
            tmpKnife.transform.localScale = Vector3.one * KnifeScaleInScreen;
            currentKnife = tmpKnife.GetComponent<Knife>();
        }
    }

    public void NexеtLevel()
    {
        if (currentWheel != null)
        {
            currentWheel.DestroyKnife();
        }
        if (GameManager.Instance.Stage * 5 == 0)
        {
            GameManager.Instance.Stage++;
            StartCoroutine(BossDefeated());
        }
        else
        {
            GameManager.Instance.Stage++;
            if( GameManager.Instance.Stage * 5 == 0)
            {
                StartCoroutine(BossFight());
            }
            else
            {
                Invoke(nameof(SetupGame), 0.3f);
            }
        }
    }

    private IEnumerator BossFight()
    {
        StartCoroutine(UIManager.Instance.BossStart());
        yield return new WaitForSeconds(2f);
        SetupGame();
    }

    private IEnumerator BossDefeated()
    {
        StartCoroutine(UIManager.Instance.BossDefeated());
        yield return new WaitForSeconds(2f);
        SetupGame();
    }
    [Serializable]
public class Boss
    {
        public GameObject bossPrefab;
        public string bossName;
    }
    
}

