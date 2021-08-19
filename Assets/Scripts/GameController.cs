using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject GameOverDisplay;
    public GameObject Score;
    public GameObject MaxScore;
    public TextMeshProUGUI CurrentScoreText;
    public ShopController shop;
    public GameObject TouchLabel;
    public PerkController Perks;
    public GameObject[] spawns;
    public GameObject WallParticleGroup;
    public Transform InstanciationPlace;
    public float spawningWaitTime = 1f;
    public float obstaclesSpeed = 4f;
    public SoundController soundController;

    [HideInInspector]
    public bool isSuperSpeed;
    [HideInInspector]
    public bool isBarrier;
    private bool isPerkSelected;

    List<Mover> currentMovers;
    Spawn[] _spawns;
    TextMeshProUGUI _score;
    TextMeshProUGUI _maxScore;
    int currentScore;
    int currentSpawnIndex = 0;
    bool canGroupSpawn;
    bool[] randomGroupsSpawn;
    bool spawnFirstObstacle;
    bool IsNoobPlaying;
    int MemorySpawnCurrentIndex;
    float distance = 5f;
    float waitTime;
    float obstaclesSpeedTemp;    
    ShakeBehaviour shaker;
    bool randomLooping;
    GameState gameState;
    Player player;
    List<Invisible> invisibleWalls;
    int WasGroupedPrevSpawn;
    HashSet<int> exclusion = new HashSet<int>();
    GameObject stain;
    GooglePlayController googlePlayController;

    void Start()
    {
        gameState = GameState.Standby;
        shaker = GetComponent<ShakeBehaviour>();
        LoadMemoryPattern();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        googlePlayController = gameObject.GetComponent<GooglePlayController>();
        invisibleWalls = GameObject.FindGameObjectsWithTag("Invisible").Select(e=>e.GetComponent<Invisible>()).ToList();
        //Time.timeScale = 1;
        canGroupSpawn = true;
        spawnFirstObstacle = true;
        TouchLabel.SetActive(true);
        GameOverDisplay.SetActive(false);
        _score = Score.GetComponent<TextMeshProUGUI>();
        _maxScore = MaxScore.GetComponent<TextMeshProUGUI>();
        var max = PlayerPrefs.GetInt("MaxScore", 0);
        _score.text = max==0?"0": string.Format("<sprite=1>{0}<sprite=0>", max);
        _maxScore.text = string.Format("<sprite=1>{0}<sprite=0>", max);
        currentMovers = new List<Mover>();
        _spawns = new Spawn[spawns.Length];
        IsNoobPlaying =  (max < 20);

        for (int i = 0; i < spawns.Length; ++i)
        {
            _spawns[i] = spawns[i].GetComponent<Spawn>();
        }
        AddOtherMovers();
    }

    public GameState GetGameState() => gameState;

    public void StartGame()
    {
        soundController.StartMainMusic();
        AddOtherMovers();
        TouchLabel.SetActive(false);
        OptionsController.Instance.Deactivate();
        Perks.Activate(true);
        _score.SetText("0");
        gameState = GameState.Started;
        currentMovers.ForEach(e => e.StartMovement());
        invisibleWalls.ForEach(e => e.StartMovement());
        StartCoroutine(Execute());
        StartCoroutine(ChangeSpeed());
    }

    public IEnumerator Execute()
    {
        while (true)
        {
            while (waitTime > 0.0f)
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }

            if (gameState != GameState.Started) break;

            if (randomLooping)
            {
                waitTime = randomGroupSpawn();
                continue;
            }
            else
            {
                if (currentScore < 40)
                {
                    if (IsNoobPlaying) AlternateSpawn();
                    else RandomSpawn(2);
                }
                else if (currentScore >= 40 && currentScore < 60)
                {
                    if (currentScore == 40) spawningWaitTime = (distance + 2) / obstaclesSpeed;
                    ComplexSpawn();
                }
                else if (currentScore >= 60 && MemorySpawnCurrentIndex <= 31)
                {
                    waitTime = MemorySpawn();
                    continue;
                }
                else
                {
                    randomLooping = true;
                }

                waitTime = spawningWaitTime;
            }
        }
    }

    float randomGroupSpawn()
    {
        if (MemorySpawnCurrentIndex == 32) MemorySpawnCurrentIndex = 0;

        var i = UnityEngine.Random.Range(0, 3);
        switch (i) {
            case 0:
                AlternateSpawn();
                return spawningWaitTime;
            case 1:
                ComplexSpawn();
                return spawningWaitTime;
            case 2:
                return MemorySpawn();
                
            }

        return 0;
    }

    void AlternateSpawn()
    {
        var mover = _spawns[currentSpawnIndex].SpawnNormalAt(spawns[currentSpawnIndex].transform, spawnFirstObstacle);
        spawnFirstObstacle = false;
        mover.gameObject.GetComponent<Obstacle>().SpawnCoin(currentSpawnIndex);
        if (++currentSpawnIndex == 2) currentSpawnIndex = 0;
        currentMovers.Add(mover);
    }

    float RandomSpawn(int maxIndex)
    {
        var i = UnityEngine.Random.Range(0, maxIndex);
        var mover = _spawns[i].SpawnNormalAt(spawns[i].transform, spawnFirstObstacle, i == 0);
        mover.gameObject.GetComponent<Obstacle>().SpawnCoin(i);
        currentMovers.Add(mover);
        spawnFirstObstacle = false;
        return distance / obstaclesSpeed;
    }

    void ComplexSpawn()
    {
        var range = Enumerable.Range(0, 3).Where(i => !exclusion.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(0, 3 - exclusion.Count);
        var rdn = range.ElementAt(index);
        exclusion.Add(rdn);
        if (exclusion.Count == 3) exclusion = new HashSet<int>();

        var mover = _spawns[rdn].SpawnNormalAt(spawns[rdn].transform);
        mover.gameObject.GetComponent<Obstacle>().SpawnCoin(rdn);
        currentMovers.Add(mover);
    }

    float MemorySpawn()
    {
        if (WasGroupedPrevSpawn >= 1 && WasGroupedPrevSpawn <= 3)
        {
            WasGroupedPrevSpawn++;
            return RandomSpawn(3);
        }

        WasGroupedPrevSpawn = 0;
        currentMovers.Add(randomGroupsSpawn[MemorySpawnCurrentIndex++] ? _spawns[2].SpawnRightAt(spawns[2].transform) : _spawns[2].SpawnLeftAt(spawns[2].transform));
        canGroupSpawn = false;
        WasGroupedPrevSpawn++;
        return (15f / obstaclesSpeed);
    }
    
    public void DeleteMover(Mover mover)
    {
        foreach (var o in currentMovers)
        {
            if (o.GetId().Equals(mover.GetId()))
            {
                currentMovers.Remove(o);
                break;
            }
        }
    }

    private void IncrementExistingObstaclesSpeed(float newSpeed)
    => currentMovers.ForEach(e => e.ChangeSpeed(newSpeed));

    private void LoadMemoryPattern()
    {
        var e = PlayerPrefs.GetInt("memoryObstacles",0);
        if (e == 0)
        {
            var arr = new bool[32];
            for(int i = 0; i < 32; i++)
            {
                arr[i] = UnityEngine.Random.Range(0, 2) == 0;
            }

            int[] array = new int[1];
            new BitArray(arr).CopyTo(array, 0);
            PlayerPrefs.SetInt("memoryObstacles",  array[0]);
            randomGroupsSpawn = arr;
            return;
        }
        
        var finalArr = new BitArray(BitConverter.GetBytes(e)).Cast<bool>().ToArray();
        randomGroupsSpawn = finalArr;
    }
    
    public void AddMover(Mover mover)
    {
        currentMovers.Add(mover);
    }

    private void AddOtherMovers()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            currentMovers.Add(wall.GetComponent<Mover>());
        }

        currentMovers.Add(GameObject.FindGameObjectWithTag("Background").GetComponent<Mover>());        
    }

    public float GetSpeed() => obstaclesSpeed;

    public void IncrementScore()
    {
        currentScore++;
        _score.SetText(currentScore.ToString());
    }

    public void PlayDodgeSound()
    {
        soundController.dodgeAudio.Play();
    }

    public void ActivateGroupSpwan() => canGroupSpawn=true;

    IEnumerator ChangeSpeed()
    {
        while (true)
        {
            if (gameState == GameState.Started && !isSuperSpeed)
            {
                if (obstaclesSpeed > 8.4f) break;

                obstaclesSpeed += 0.1f;
                IncrementExistingObstaclesSpeed(obstaclesSpeed);
                spawningWaitTime = distance / obstaclesSpeed;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void MultiplyCurrentSpeed()
    {
        if (isPerkSelected) return;
        isPerkSelected = true;

        if (Perks.CanActivateRage())
        {
            isSuperSpeed = true;
            player.SetRagedAnim(true);
            obstaclesSpeedTemp = obstaclesSpeed;
            var spawningWaitTimeTemp = spawningWaitTime;
            obstaclesSpeed = 15f;
            IncrementExistingObstaclesSpeed(obstaclesSpeed);
            spawningWaitTime = distance / obstaclesSpeed;
            waitTime = waitTime * spawningWaitTime / spawningWaitTimeTemp;
            SoundController.Instance.apitoAudio.Play();
            SoundController.Instance.smokeAudio.Play();

            StartCoroutine(ResetCurrentSpeed());
        }
    }

    private IEnumerator ResetCurrentSpeed()
    {
        yield return new WaitForSeconds(5f);
        player.SetRagedAnim(false);
        obstaclesSpeed = obstaclesSpeedTemp;
        IncrementExistingObstaclesSpeed(obstaclesSpeed);
        var spawningWaitTimeTemp = spawningWaitTime;
        spawningWaitTime = distance / obstaclesSpeed;
        waitTime = waitTime * spawningWaitTime / spawningWaitTimeTemp;
        isSuperSpeed = false;
        isPerkSelected = false;
        SoundController.Instance.smokeAudio.Stop();
    }

    public void SetBarrier()
    {
        if (isPerkSelected) return;
        isPerkSelected = true;

        if (Perks.CanActivateBarrier())
        {
            isBarrier = true;
            player.SetBarrier(true);
            StartCoroutine(ResetBarrier());
        }
    }

    private IEnumerator ResetBarrier()
    {
        yield return new WaitForSeconds(7f);
        if (gameState != GameState.GameOver)
        {
            DeactivateBarrier();
        }
    }

    private void DeactivateBarrier()
    {
        isBarrier = false;
        isPerkSelected = false;
        player.SetBarrier(false);
    }

    public void GameOver()
    {
        soundController.smashAudio.Play();
        player.Die();
        soundController.windAudio.Stop();
        gameState = GameState.GameOver;
        GameObject.FindGameObjectsWithTag("Warning").ToList().ForEach(e=>Destroy(e));
        var warningRed = GameObject.FindGameObjectWithTag("WarningRed");
        if (warningRed != null) Destroy(warningRed);
        currentMovers.ForEach(e => e.StopMovement());
        shaker.Stop();
        SetMaxScore();
        OptionsController.Instance.Activate();
        Perks.Activate(false);
        DeactivateBarrier();
        StopAllCoroutines();
        soundController.FadeOutMusic();
    }

    public void SetGameOverDisplay()
    {
        GameOverDisplay.SetActive(true);
    }

    private void SetMaxScore()
    {
        var maxScore = PlayerPrefs.GetInt("MaxScore", 0);
        CurrentScoreText.SetText(currentScore.ToString());

        if (IsNoobPlaying) IsNoobPlaying = currentScore < 20;

        if (currentScore > maxScore)
        {
            PlayerPrefs.SetInt("MaxScore", currentScore);
            _maxScore.SetText(string.Format("<sprite=1>{0}<sprite=0>", currentScore));
            CurrentScoreText.SetText(string.Format("<sprite=1>{0}<sprite=0>", currentScore));
            googlePlayController.AddScoreToLeaderBoard(GPGSIds.leaderboard_cliff_dive_leaderboard,currentScore);
        }
    }

    internal void SetBigStain(GameObject stain)
    {
        this.stain = stain;
    }

    public void ReloadGame()
    {
        if (stain != null) Destroy(stain);
        WallParticleGroup.SetActive(true);
        WallParticleGroup.GetComponent<Animator>().SetBool("Fall",true);
        player.CleanStains();
        soundController.rocksAudio.Play();
        GameOverDisplay.SetActive(false);
        //player.HidePosition();
        currentMovers.ForEach(e => e.ReloadMovers());
    }

    public void ShowPlayer()
    {
        player.ResetPosition();
        soundController.windAudio.Play();
    }

    public void OnAnimationEnd()
    {
        // rocksAudio.Stop();
        currentMovers.ForEach(e => e.OnAnimationEnd());
        player.EnableCollisions();
        invisibleWalls.ForEach(e => e.ResetPos());
        shaker.Restart();
        //player.ResetPosition();
        TouchLabel.SetActive(true);
        canGroupSpawn = true;
        spawnFirstObstacle = true;
        spawningWaitTime = 1f;
        waitTime = 0;
        obstaclesSpeed = 4f;
        gameState = GameState.Standby;
        currentScore = 0;
        currentSpawnIndex = 0;
        MemorySpawnCurrentIndex = 0;
        distance = 5f;
        randomLooping = false;
        WasGroupedPrevSpawn = 0;
        exclusion = new HashSet<int>();       
        currentMovers = new List<Mover>();
    }

    

   

    public void IncreaseCoins()
    {
        shop.IncreaseOneCoin();
    }

    public void BigShake(bool activate)
    {
        if (activate)
            shaker.TriggerBigShake();
        else
            shaker.StopBigShake();
    }

    ////////////////TEST////////////////////////////
    
    public void SetCurrentScore(int score)
    {
        CurrentScoreText.SetText(score.ToString());
        _score.SetText(score.ToString());
        currentScore = score;
    }

    public void IncreaseCoins(int coins)
    {
        shop.IncreaseCoins(coins);
    }

    public void SetSpeed(float speed)
    {
        obstaclesSpeed = speed;
        spawningWaitTime = distance / obstaclesSpeed;
    }

    ////////////////////////////////////////////////////////
}
