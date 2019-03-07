using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        var obj = this.transform.Find("hex").gameObject;
        obj.SetActive(false);

        obj = this.transform.Find("hex_hover").gameObject;
        obj.SetActive(true);
    }

    void OnMouseExit()
    {
        var obj = this.transform.Find("hex").gameObject;
        obj.SetActive(true);

        obj = this.transform.Find("hex_hover").gameObject;
        obj.SetActive(false);
    }
}
