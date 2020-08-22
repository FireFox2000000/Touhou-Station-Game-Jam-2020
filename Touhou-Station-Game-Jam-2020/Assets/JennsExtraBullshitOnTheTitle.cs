using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JennsExtraBullshitOnTheTitle : MonoBehaviour
{
    public GameObject LogoObj;
    public GameObject[] ObjectsToCheck;

    // Didn't want to mess with Firefox's code on the title screen just for effects so i figured i'd do this instead ^^;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool HaveExtraStuffOn = false;
        for (int a=0; a<ObjectsToCheck.Length; a = a + 1)
        {
            if (ObjectsToCheck[a].activeSelf == true) HaveExtraStuffOn = true;
        }
        LogoObj.SetActive(HaveExtraStuffOn);
    }
}
