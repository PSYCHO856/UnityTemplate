using MobileKit;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MainPage : UIBasePage
{
    // Start is called before the first frame update
    public Button input1;
    //public TextMeshProUGUI codeText;
    public TMP_InputField codeText;
    private void Start()
    {
        input1.onClick.AddListener(RecordTest);
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
}