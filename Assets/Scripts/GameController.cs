using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    // Game Settings
    private CanvasGroup sceneFade;
    private float musicVolume, sfxVolume;
    private bool vibrate = true;
    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; SaveData(); } }
    public float SfxVolume { get { return sfxVolume; } set { sfxVolume = value; SaveData(); } }
    public bool Vibrate { get { return vibrate; } set { vibrate = value; SaveData(); } }

    // Player Save Data
    private int highscore, coins, skin;
    private bool[] skinsUnlocked;
    public int Highscore { get { return highscore; } set { highscore = value; } }
    public int Coins { get { return coins; } set { coins = value; } }
    public int Skin { get { return skin; } set { skin = value; } }
    public bool[] SkinsUnlocked { get { return skinsUnlocked; } set { skinsUnlocked = value; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Reset();
        LoadData();
        StartCoroutine(SceneFadeOut());
    }

    private void Reset()
    {
        highscore = 0;
        coins = 0;
        skin = 0;
        skinsUnlocked = new bool[] { false, false, false, false, false, false };
    }

    private void LoadData()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        vibrate = PlayerPrefs.GetFloat("Vibrate", 1f) == 1f;

        SaveData save = SaveSystem.LoadPlayer();
        if (save == null) return;
        highscore = save.highscore;
        coins = save.coins;
        skin = save.skin;
        skinsUnlocked = save.skinsUnlocked;
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
        PlayerPrefs.SetFloat("Vibrate", vibrate ? 1f : 0f);
        PlayerPrefs.Save();

        SaveSystem.SavePlayer(this);
    }

    private IEnumerator SceneFadeIn()
    {
        sceneFade = FindObjectOfType<CanvasGroup>();
        sceneFade.gameObject.SetActive(true);
        sceneFade.alpha = 0f;
        while (sceneFade.alpha < 1f)
        {
            if (Time.deltaTime * 2f <= 1f - sceneFade.alpha)
                sceneFade.alpha += Time.deltaTime * 2f;
            else
                sceneFade.alpha = 1f;
            yield return null;
        }
    }

    private IEnumerator SceneFadeOut()
    {
        sceneFade = FindObjectOfType<CanvasGroup>();
        sceneFade.gameObject.SetActive(true);
        sceneFade.alpha = 1f;
        while (sceneFade.alpha > 0f)
        {
            if (Time.deltaTime * 2f <= sceneFade.alpha)
                sceneFade.alpha -= Time.deltaTime * 2f;
            else
                sceneFade.alpha = 0f;
            yield return null;
        }
    }

    public IEnumerator Quit()
    {
        Debug.Log("Quitting...");
        StartCoroutine(SceneFadeIn());
        SaveData();
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(AsyncLoadLevel(index));
    }

    private IEnumerator AsyncLoadLevel(int index)
    {
        StartCoroutine(SceneFadeIn());
        yield return new WaitForSeconds(0.5f);
        var asyncLoadLevel = SceneManager.LoadSceneAsync(index);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        StartCoroutine(SceneFadeOut());
    }
}
