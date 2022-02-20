using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour


{
    // variable from MenuUIHandler for player name
    public string s;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text NewScoreText;
    public Text BestScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    public int m_PointsHigh;

    private bool m_GameOver = false;

    private void Awake()

    {

        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_PointsHigh = data.m_PointsHigh;

            // pulling variable from another static script
            s = MenuUIHandler.Instance.playerName;

            BestScoreText.text = $"Best Score by " + s + ":  " + data.m_PointsHigh;

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //LoadScore();
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {

                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);

            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }


    void AddPoint(int point)
    {

        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

    }
    public void GameOver()
    {

        m_GameOver = true;
        GameOverText.SetActive(true);
        // Debug.Log("GameOver m_Points = " + m_Points);
        // Debug.Log("GameOver m_PointsHigh = " + m_PointsHigh);
        if (m_Points > m_PointsHigh)
        {

            SaveScore();
        }
        else
        {
            KeepScore();
        }

    }

    // Session data persistence
    [System.Serializable]
    class SaveData
    {
        public int m_PointsHigh;
        public int m_Points;

    }

    // Save score in json file when game over
    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.m_PointsHigh = m_Points;
        data.m_Points = m_Points;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("json saved as SaveScore " + json);


    }
    public void KeepScore()
    {
        SaveData data = new SaveData();
        data.m_PointsHigh = m_PointsHigh;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        
    }

    // Retrieve score from json file
    public void LoadScore()
    {

        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);


            // Debug.Log("ending m_PointsHigh = " + data.m_PointsHigh);
            // Debug.Log("ending m_Points = " + data.m_Points);

            // pulling variable from another static script
            s = MenuUIHandler.Instance.playerName;

            BestScoreText.text = $"Best Score: " + s + $": {json}";
            
        }
    }
}
