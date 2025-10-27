using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public static int Level
    {
        set { PlayerPrefs.SetInt("LEVEL", value); }
        get { return PlayerPrefs.GetInt("LEVEL", 1); }
    }
    public static int Score
    {
        set { PlayerPrefs.SetInt("SCORE", value); }
        get { return PlayerPrefs.GetInt("SCORE", 0); }
    }
    [SerializeField] LevelController[] levels;
    int counter = 0;
    public void Initialize()
    {
        foreach (var level in levels)
        {
            level.gameObject.SetActive(false);
        }
        if (Level <= levels.Length)
        {
            counter = Level - 1;
        }
        else
        {
            counter = Random.Range(0, levels.Length);
        }
        levels[counter].gameObject.SetActive(true);
    }
}
