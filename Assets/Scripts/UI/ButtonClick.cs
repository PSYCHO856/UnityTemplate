using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    private GameObject _activeState;
    private GameObject _forbidState;
    private Button _thisBtn;

    private void Awake()
    {
        _thisBtn = transform.GetComponent<Button>();

//Todo 多条件扩展
        if (transform.childCount >= 2)
        {
            _activeState = transform.GetChild(0).gameObject;
            _forbidState = transform.GetChild(1).gameObject;
        }
    }

    public void BtnClick()
    {
        //AudioManager.PlaySound(ConfigManager.AudioConfig.Click);
        GetComponent<Animator>().SetTrigger("Click");
    }

    public void BtnActive()
    {
        _thisBtn.enabled = true;
        _activeState.SetActive(true);
        _forbidState.SetActive(false);
    }

    public void BtnForbid()
    {
        _thisBtn.enabled = false;
        _activeState.SetActive(false);
        _forbidState.SetActive(true);
    }
}