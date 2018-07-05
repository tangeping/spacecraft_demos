using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettleAccount : connectstate
{

	// Use this for initialization
	void Start () {
        Button btn = Trans.FindObj( gameObject,"ReturnButton").GetComponent<Button>();
        if (btn == null)
        {
            Debug.Log("can not found ReturnButton!");
        }
        btn.onClick.AddListener(OnClick);
    }

    public void reqTransAvatar()
    {
        KBEngine.Event.fireIn("transAvatar", new object[] { });
    }

    void OnClick()
    {
        reqTransAvatar();

        SceneManager.LoadScene("pickscene");
    }
    // Update is called once per frame

}
