using UnityEngine;

public class SkinController : MonoBehaviour
{
    [SerializeField] Transform[] part_1, part_2, part_3, part_4, part_5, part_6, part_7, part_8, part_9;
    private Transform[][] parts;

    public void Initialize()
    {
        parts = new Transform[][]
        {
            part_1, part_2, part_3, part_4, part_5, part_6, part_7, part_8, part_9
        };
        LoadSkin();

    }
    public void MixRandom()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            var group = parts[i];
            if (group == null || group.Length == 0) continue;

            // Tắt tất cả
            foreach (var part in group)
                part.gameObject.SetActive(false);

            // Bật 1 cái ngẫu nhiên
            int randomIndex = Random.Range(0, group.Length);
            group[randomIndex].gameObject.SetActive(true);

            // Lưu index đã bật
            PlayerPrefs.SetInt($"SkinPart_{i}", randomIndex);
        }

        PlayerPrefs.Save();
    }

    public void LoadSkin()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            var group = parts[i];
            if (group == null || group.Length == 0) continue;

            // Tắt tất cả
            foreach (var part in group)
                part.gameObject.SetActive(false);

            // Lấy index lưu trong PlayerPrefs (nếu chưa có thì mặc định là 0)
            int savedIndex = PlayerPrefs.GetInt($"SkinPart_{i}", 0);
            group[Mathf.Clamp(savedIndex, 0, group.Length - 1)].gameObject.SetActive(true);
        }
    }
}
