using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public AudioSource audioSourceBGM; // BGMのスピーカー
    public AudioClip[] audioClipsBGM; // 再生する素材
   public AudioSource audioSourceSE; // SEのスピーカー
   public AudioClip[] audioClipsSE; // 再生する素材

    public void StopBGM()
    {
        audioSourceBGM.Stop(); // BGMを止める
    }

    public void PlayBGM(int sceneName)
    {
        audioSourceBGM.Stop(); // BGMを止める
        switch (sceneName)
        {
            default:
            case 0:
                audioSourceBGM.clip = audioClipsBGM[0];
                break;
            case 1:
                audioSourceBGM.clip = audioClipsBGM[1];
                break;
            case 2:
                audioSourceBGM.clip = audioClipsBGM[2];
                break;
            case 3:
                audioSourceBGM.clip = audioClipsBGM[3];
                break;
        }
        audioSourceBGM.Play(); // BGMを再生
    }

    public void PlaySE(int index)
{
    if (audioSourceSE == null)
    {
        Debug.LogError("audioSourceSE が null です！SoundManager の AudioSource を確認してください。");
        return;
    }
    if (index < 0 || index >= audioClipsSE.Length)
    {
        Debug.LogError("指定された SE のインデックスが範囲外です: " + index);
        return;
    }
    
    audioSourceSE.PlayOneShot(audioClipsSE[index]); // SEを一度だけ再生
}

    // --- セーブデータ連携 ---
    public void ApplySaveData(SystemSaveData data)
    {
        if (audioSourceBGM != null) audioSourceBGM.volume = data.bgmVolume;
        if (audioSourceSE != null) audioSourceSE.volume = data.seVolume;
    }

    public void FillSaveData(SystemSaveData data)
    {
        if (audioSourceBGM != null) data.bgmVolume = audioSourceBGM.volume;
        if (audioSourceSE != null) data.seVolume = audioSourceSE.volume;
    }
}
