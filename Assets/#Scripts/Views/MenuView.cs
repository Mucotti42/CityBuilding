using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MenuView : MonoBehaviour
{
    public static MenuView instance;
    
    public int _gold;
    public int _gem;

    private int oldGold;
    private int oldGem;
    public int gold 
    { 
        get => _gold; 
        set { oldGold = _gold; _gold = value;
            GemGoldFeedBack( true,value - oldGold);
        } 
    }
    public int gem 
    { 
        get => _gem; 
        set { 
            oldGem = _gem;  _gem = value;
            GemGoldFeedBack( false,value - oldGem);} 
    }

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI goldFeedbackText;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private TextMeshProUGUI gemFeedbackText;
    private Coroutine goldCoroutine, gemCoroutine;

    private WaitForSeconds waitSecond = new WaitForSeconds(0.001f);
    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Start()
    {
        GetCurrencyData();
    }

    public void Restart()
    {
        //TODO: Button listener   
    }

    private void GetCurrencyData()
    {
        Vector2Int currency = Utils.instance.LoadCurrencyData();
        _gold = currency.x;
        _gem = currency.y;
        goldText.text = gold.ToString();
        gemText.text = gem.ToString();
        EventManager.instance.UpdateCurrency();
    }

    private void GemGoldFeedBack(bool gold, int value)
    {
        if (gold)
        {
            goldText.text = this.gold.ToString();
            if (value != 0)
            {
                if (goldCoroutine != null) StopCoroutine(goldCoroutine);
                goldCoroutine = StartCoroutine(IEFeedback(goldFeedbackText, value));
            }
        }
        else
        {
            gemText.text = gem.ToString();

            if (value != 0)
            {
                if (gemCoroutine != null) StopCoroutine(gemCoroutine);
                gemCoroutine = StartCoroutine(IEFeedback(gemFeedbackText, value));
            }
        }
        EventManager.instance.UpdateCurrency();
    }

    private IEnumerator IEFeedback(TextMeshProUGUI text,int value)
    {
        var pos = text.transform.localPosition;
        pos.y = -2.7f;
        text.text = (value > 0 ? "+" : "") + value.ToString();
        text.color = value > 0 ? new Color(0.380877f, 0.9686275f, 0.2392157f, 1) : new Color(0.9490196f, .3f, 0.1764706f, 1);
        for (int i = 0; i < 1000; i++)
        {
            pos.y += 0.065f;
            text.transform.localPosition = pos;
            yield return waitSecond;
        }

        text.text = "";
    }
}