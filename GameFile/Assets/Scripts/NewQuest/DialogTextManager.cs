using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class DialogTextManager : MonoBehaviour
{
    [SerializeField] Text uiText;
    [SerializeField] float intervalForCharacterDisplay = 0.05f;

    private string[] scenarios;
    private int currentLine = 0;
    private string currentText = "";
    private bool isTextComplete = false;
    private UnityAction onCompleteCallback;

    private Coroutine displayCoroutine;

    // 外部から isTextComplete を参照できるようにする
    public bool IsTextComplete => isTextComplete;

public void ShowMessage(string message, UnityAction onComplete = null, bool autoProceed = false)
{
    SetScenarios(new string[] { message }, onComplete, autoProceed);
}

public void SetScenarios(string[] lines, UnityAction onComplete = null, bool autoProceed = false)
{
    StopAllCoroutines();
    scenarios = lines;
    currentLine = 0;
    onCompleteCallback = onComplete;
    this.autoProceed = autoProceed;
    displayCoroutine = StartCoroutine(DisplayNextLine());
}
private bool autoProceed = false;
        private bool skipToFullText = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isTextComplete)
            {
                skipToFullText = true;
            }
        }
    }
    IEnumerator DisplayNextLine()
    {
    while (currentLine < scenarios.Length)
    {
        currentText = scenarios[currentLine];
        uiText.text = "";
        isTextComplete = false;
        skipToFullText = false;

        for (int i = 0; i < currentText.Length; i++)
        {
            if (skipToFullText)
            {
                uiText.text = currentText;
                break;
            }
            uiText.text += currentText[i];
            yield return new WaitForSeconds(intervalForCharacterDisplay);
        }

        isTextComplete = true;

        if (autoProceed)
        {
            // テキストが流れ終わったらすぐ次へ
            yield return new WaitForSeconds(0.5f); // 少し待つ場合
        }
        else
        {
            // ユーザーのクリック待ち
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }
        currentLine++;
    }

    onCompleteCallback?.Invoke();
    }

    public void ClearMessage()
    {
    // 例: Text型の場合
        uiText.text = "";
    // TMP_Text型の場合も同様
    }
}
