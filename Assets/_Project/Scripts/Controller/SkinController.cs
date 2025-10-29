using System;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    public Skin[] skins;
    public Skin currentSkin;
    int index;
    public int SkinWearing
    {
        set { PlayerPrefs.SetInt("SkinWearing", value); }
        get { return PlayerPrefs.GetInt("SkinWearing", 0); }
    }
    public void Initialize()
    {
        skins[0].Owned = 1; // Default skin is owned
        LoadSkin();
        index = Array.IndexOf(skins, currentSkin);
    }
    public void Right(ref int cost, ref bool owned)
    {
        if (index < skins.Length - 1) index++;
        else index = 0;
        Select(ref cost, ref owned);
    }
    private void Update()
    {
        Debug.Log(index);
    }
    public void Left(ref int cost, ref bool owned)
    {
        if (index > 0) index--;
        else index = skins.Length - 1;
        Select(ref cost, ref owned);
    }
    private void Select(ref int cost, ref bool owned)
    {
        foreach (Skin skin in skins)
        {
            skin.SetActive(false);
        }
        currentSkin = skins[index];
        cost = currentSkin.cost;
        currentSkin.SetActive(true);

        if (currentSkin.Owned > 0)
        {
            SkinWearing = currentSkin.id;
        }

        owned = currentSkin.Owned > 0;
    }
    public void Buy(ref bool isOK)
    {
        if (currentSkin.cost > LevelManager.Coin)
        {
            isOK = false;
            return;
        }
        LevelManager.Coin -= currentSkin.cost;
        isOK = true;
        currentSkin.Owned++;
        SkinWearing = currentSkin.id;
        UIManager.Instance.GetScreen<InGameUI>().UpdateCoin();
    }
    public void LoadSkin()
    {
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].SetActive(false);
            if (skins[i].id == SkinWearing)
            {
                skins[i].SetActive(true);
                currentSkin = skins[i];
            }
        }
    }
}
[Serializable]
public class Skin
{
    public int id;
    public GameObject[] parts;
    public int cost;
    public void SetActive(bool isActive)
    {
        foreach (var part in parts)
        {
            part.SetActive(isActive);
        }
    }
    public int Owned
    {
        set { PlayerPrefs.SetInt($"Skin_{id}", value); }
        get { return PlayerPrefs.GetInt($"Skin_{id}", 0); }
    }

}
