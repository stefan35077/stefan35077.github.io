using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [HideInInspector] public float Score;

    [SerializeField] List<float> ScoreFloor = new();
    [SerializeField] List<Image> starsAmount = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        CalculateScore();
    }

    public void AddScore(float receivedPoints)
    {
        Score += receivedPoints;
        CalculateScore();
    }

    public void RemoveScore(float receivedPoints)
    {
        Score -= receivedPoints;
        Score = Mathf.Max(0, Score);
        CalculateScore();
    }

    public void CalculateScore()
    {
        if (ScoreFloor.Count != starsAmount.Count)
        {
            return;
        }

        for (int i = 0; i < ScoreFloor.Count; i++)
        {
            var img = starsAmount[i];
            if (img == null)
            {
                continue;
            }

            img.color = Color.black;

            if (Score >= ScoreFloor[i])
            {
                img.color = Color.white;
            }
        }   
    }
}
