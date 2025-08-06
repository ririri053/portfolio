using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LegacyNameInputFilter : MonoBehaviour
{
    [SerializeField] private InputField nameInputField;

    // 許可：ひらがな、カタカナ、英字（半角のA-Z/a-z）、長音記号（ー）
    private static readonly Regex allowedChars = new Regex(@"[ぁ-んァ-ヶーa-zA-Z]", RegexOptions.Compiled);

    private void Start()
    {
        nameInputField.characterLimit = 6;
        nameInputField.onValueChanged.AddListener(FilterInput);
    }

    private void FilterInput(string input)
    {
        string filtered = "";
        foreach (char c in input)
        {
            if (allowedChars.IsMatch(c.ToString()))
            {
                filtered += c;
            }
        }

        if (filtered != input)
        {
            int caretPos = nameInputField.caretPosition;
            nameInputField.text = filtered;
            nameInputField.caretPosition = Mathf.Min(caretPos, filtered.Length);
        }
    }
}
