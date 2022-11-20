using UnityEngine;
using Peoples;

public class GameController : Singleton<GameController>
{
    public bool isMovingCalled;

    public Transform Root3DObj;
    public People1 peo;
    // Start is called before the first frame update
    private void Start()
    {
        UIController.Open(UIPageId.Main);
        peo = new People1(Instance.Root3DObj.GetChild(0));
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}

namespace Peoples
{
    public class People1
    {

        Transform peopleTrans;

        public People1(Transform t)
        {
            peopleTrans = t;
        }
        public void Move()
        {
            peopleTrans.Translate(new Vector3(5, 0, 0));
            Debug.Log("dynamic call people move!");
        }

    }
}