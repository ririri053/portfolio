using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private Text PlayerLevelText;
    [SerializeField] private Text PlayerLevelBGText;
    [SerializeField] private Text GachaPointText;
    [SerializeField] private Text GachaPointBGText;
    [SerializeField] private Text NextLevelText;
    [SerializeField] private Text NextLevelBGText;
    [SerializeField] public InputField nameInputField;
    [SerializeField] private QuestSelectionUI questSelectionUI;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Slider seVolume;
    [SerializeField] private Slider bgmVolume;

    public void AudioSetUp(SystemSaveData loaded)
    {
        // --- セーブデータのロードと反映 ---
        if (loaded != null)
        {
            SaveManager.LoadSystem();
            // ここでSoundManagerにもロードしたデータを適用
            if (SoundManager.instance != null) // SoundManagerがAwakeで初期化されていることを確認
            {
                SoundManager.instance.ApplySaveData(loaded);
            }
            // Sliderの値をセーブデータから読み込んだ値に設定
            seVolume.value = loaded.seVolume;
            bgmVolume.value = loaded.bgmVolume;
        }
        else
        {
            // セーブデータがない場合の初期値設定（オプション）
            // 例: Sliderの現在の値をSoundManagerに適用しておく
            if (SoundManager.instance != null)
            {
                // Sliderの初期値をSoundManagerに適用
                loaded.seVolume = seVolume.value;
                loaded.bgmVolume = bgmVolume.value;
                SaveManager.SaveSystem(loaded);
                SoundManager.instance.audioSourceSE.volume = loaded.seVolume;
                SoundManager.instance.audioSourceBGM.volume = loaded.bgmVolume;
            }
        }
    }

    public void PlayerNameSetUp(PlayerNameSaveData loaded)
    {
        if (loaded != null)
        {
            SaveManager.LoadPlayerName();
            nameInputField.text = loaded.playerName;
        }
        else
        {

            loaded.playerName = "プレイヤー";
            SaveManager.SavePlayerName(loaded);
        }
    }

    public void PlayerSettings()
    {
        PlayerLevelText.text = "Lv." + playerManager.currentPlayer.Level.ToString();
        PlayerLevelBGText.text = "Lv." + playerManager.currentPlayer.Level.ToString();
        GachaPointText.text = "ガチャP\n" + playerManager.currentPlayer.GachaPoint.ToString();
        GachaPointBGText.text = "ガチャP\n" + playerManager.currentPlayer.GachaPoint.ToString();
        NextLevelText.text = "NextLevel\n" + playerManager.currentPlayer.EXP.ToString() + "/" + playerManager.currentPlayer.LevelUpEXP.ToString();
        NextLevelBGText.text = "NextLevel\n" + playerManager.currentPlayer.EXP.ToString() + "/" + playerManager.currentPlayer.LevelUpEXP.ToString();
    }

    public void ShowMenuPanel() 
    {
        PlayerSettings();
        MenuPanel.SetActive(true);
        questSelectionUI.ShowQuestSelectPanel();
    }
    public void HideMenuPanel() => MenuPanel.SetActive(false);
    public void ShowSettingPanel() => settingPanel.SetActive(true);
    public void HideSettingPanel() => settingPanel.SetActive(false);

    public void OnSEVolumeChanged(float value)
    {
        if (SoundManager.instance != null && SoundManager.instance.audioSourceSE != null)
        {
            SoundManager.instance.audioSourceSE.volume = value;
            // --- セーブ処理 ---
            SaveAudio();
        }
    }

    public void OnBGMVolumeChanged(float value)
    {
        if (SoundManager.instance != null && SoundManager.instance.audioSourceBGM != null)
        {
            SoundManager.instance.audioSourceBGM.volume = value;
            // --- セーブ処理 ---
            SaveAudio();
        }
    }

    public void AudioUpdate()
    {
        seVolume.value = SoundManager.instance.audioSourceSE.volume;
        bgmVolume.value = SoundManager.instance.audioSourceBGM.volume;
    }

    public void SaveAudio()
    {
        var saveData = new SystemSaveData();
        SoundManager.instance.FillSaveData(saveData); 
        // SoundManager.instance.ApplySaveData(saveData);
        SaveManager.SaveSystem(saveData);
    }

    public void OnPlayerNameChanged(string value)
    {
        nameInputField.text = value;
        Debug.Log("PlayerNameChanged");
        // --- セーブ処理 ---
        var saveData = new PlayerNameSaveData();
        saveData.playerName = nameInputField.text;
        SaveManager.SavePlayerName(saveData);
    }

    public void ApplySaveData(PlayerNameSaveData data)
    {
        nameInputField.text = data.playerName;
    }
}