using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController ins;

    [Header("Dead Panel")]
    public RectTransform deadPanel;
    public RectTransform diedTxt;

    public float pnlFadeUptime, txtFadeUptime;

    [Header("Hit Image")]
    public RectTransform hitUI;


    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        hitUI.DOScale(0, 0f);

        deadPanel.DOScale(0, 0);
        deadPanel.GetComponent<CanvasGroup>().alpha = 0;

        diedTxt.DOScale(1, 0);
        diedTxt.GetComponent<CanvasGroup>().alpha = 0;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    public void YouDied()
    {
        deadPanel.DOScale(1, 0);
        deadPanel.GetComponent<CanvasGroup>().DOFade(1, pnlFadeUptime);

        diedTxt.GetComponent<CanvasGroup>().DOFade(1, txtFadeUptime).SetDelay(pnlFadeUptime + 0.5f);
        diedTxt.DOScale(1.1f, txtFadeUptime).SetDelay(pnlFadeUptime+0.5f);
    }
    public void ResetGameBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowHitUI()
    {
        hitUI.DOScale(1, 0.05f);
        hitUI.DOScale(0, 0.05f).SetDelay(0.2f);
    }
}
