using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public static int Level
    {
        set { PlayerPrefs.SetInt("LEVEL", value); }
        get { return PlayerPrefs.GetInt("LEVEL", 1); }
    }
    public static int Coin
    {
        set { PlayerPrefs.SetInt("COIN", value); }
        get { return PlayerPrefs.GetInt("COIN", 0); }
    }
    public static int Tutorial
    {
        set { PlayerPrefs.SetInt("TUTORIAL", value); }
        get { return PlayerPrefs.GetInt("TUTORIAL", 0); }
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
