using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highscoreText, greenText, blueText, pinkText, redText, orangeText, yellowText, coinsText;
    private AudioManager audioManager;
    private GameController controller;
    private Slider musicSlider, sfxSlider;
    private Toggle vToggle;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        controller = FindObjectOfType<GameController>();
        audioManager.sounds[0].source.volume = controller.MusicVolume;
        audioManager.sounds[1].source.volume = controller.SfxVolume;
        audioManager.sounds[2].source.volume = controller.SfxVolume;
        audioManager.sounds[3].source.volume = controller.SfxVolume;
        audioManager.Play("song");
        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.gameObject.name == "MusicVolume")
                musicSlider = s;
            else
                sfxSlider = s;
        }
        vToggle = FindObjectOfType<Toggle>();
        highscoreText.text = "Highscore: " + controller.Highscore;
        SetSettings();
        UpdateShop();
    }

    private void SetSettings()
    {
        musicSlider.value = controller.MusicVolume;
        sfxSlider.value = controller.SfxVolume;
        vToggle.isOn = controller.Vibrate;
    }

    private void UpdateShop()
    {
        greenText.text = (controller.SkinsUnlocked[0]) ? "" : "1000";
        blueText.text = (controller.SkinsUnlocked[1]) ? "" : "5000";
        pinkText.text = (controller.SkinsUnlocked[2]) ? "" : "10000";
        redText.text = (controller.SkinsUnlocked[3]) ? "" : "50000";
        orangeText.text = (controller.SkinsUnlocked[4]) ? "" : "100000";
        yellowText.text = (controller.SkinsUnlocked[5]) ? "" : "1000000";
        if (controller.Skin != 0) new TextMeshProUGUI[] { greenText, blueText, pinkText, redText, orangeText, yellowText }[controller.Skin - 1].text = "O";
        coinsText.text = "Coins: " + controller.Coins;
    }

    public void VolumeChange(Slider s)
    {
        if (s.gameObject.name == "MusicVolume")
        {
            controller.MusicVolume = s.value;
            audioManager.sounds[0].source.volume = controller.MusicVolume;
        }
        else if (s.gameObject.name == "SfxVolume")
        {
            controller.SfxVolume = s.value;
            audioManager.sounds[1].source.volume = controller.SfxVolume;
            audioManager.sounds[2].source.volume = controller.SfxVolume;
            audioManager.sounds[3].source.volume = controller.SfxVolume;
        }
    }

    public void ToggleChange(Toggle t)
    {
        controller.Vibrate = t.isOn;
    }

    public void PlayButtonAudio()
    {
        audioManager.Play("select");
        if (controller.Vibrate) Vibration.Vibrate(30);
    }

    public void LoadGame()
    {
        controller.LoadLevel(1);
    }

    public void QuitGame()
    {
        StartCoroutine(controller.Quit());
    }

    public void SelectColour(int i)
    {
        if (i == controller.Skin)
        {
            PlayButtonAudio();
            controller.Skin = 0;
            UpdateShop();
            return;
        }
        if (!controller.SkinsUnlocked[i - 1])
        {
            int[] skinsCost = new int[] { 1000, 5000, 10000, 50000, 100000, 1000000 };
            if (controller.Coins > skinsCost[i - 1])
            {
                PlayButtonAudio();
                controller.Coins -= skinsCost[i - 1];
                controller.SkinsUnlocked[i - 1] = true;
                controller.Skin = i;
                UpdateShop();
            }
        }
        else
        {
            PlayButtonAudio();
            controller.Skin = i;
            UpdateShop();
        }
    }
}
