using MobileKit;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MainPage : UIBasePage
{
    // Start is called before the first frame update
    public Button input1;
    public Button btnNextPage;
    //public TextMeshProUGUI codeText;
    public TMP_InputField codeText;
    private void Start()
    {
        input1.onClick.AddListener(RecordTest);
        btnNextPage.onClick.AddListener(NextPage);
        UIManager.Instance.Init();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    void RecordTest()
    {
        RecordManager.Data.Cash += 10000;
        codeText.text = RecordManager.Data.Cash.ToString();
    }

    void NextPage()
    {
        if (UIController.IsOpened(UIPageId.Info))
        {
            UIController.Close(UIPageId.Info);
        }
        else
        {
            UIController.Open(UIPageId.Info);
        }

    }
}