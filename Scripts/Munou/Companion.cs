using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompanionUI : MonoBehaviour
{
    [Header("UI参照")]
    public Text dialogueText;     // セリフテキスト
    public GameObject bubble;     // 吹き出しオブジェクト

    void Start()
    {
        bubble.SetActive(false);
    }

    // 外部から呼ぶ：SayLine("セリフ内容", 表示秒数)
    public void SayLine(string line, float duration = 3f)
    {
        StopAllCoroutines();
        StartCoroutine(ShowLine(line, duration));
    }

    IEnumerator ShowLine(string line, float duration)
    {
        bubble.SetActive(true);
        dialogueText.text = line;
        yield return new WaitForSeconds(duration);
        bubble.SetActive(false);
    }
}