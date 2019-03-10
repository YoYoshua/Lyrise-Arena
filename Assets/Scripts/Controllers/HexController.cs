using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexController : MonoBehaviour
{
    [HideInInspector]
    public Hex HexObject { get; set; }
    private Stack<GameObject> spriteStack;

    private bool isHoverStateChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteStack = new Stack<GameObject>();
        PushSprite(this.transform.Find("hex").gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        #region Hover
        Vector3 mousePosition = MouseHelper.GetMouseWorldPoint(Camera.main);
        Vector3 roundedHexPosition = HexHelper.WorldPositionToAxial(mousePosition, HexObject.Radius);

        if(!isHoverStateChanged && roundedHexPosition == HexObject.AxialPosition)
        {
            SetHover(true);
            isHoverStateChanged = true;
        }
        else if(isHoverStateChanged && (roundedHexPosition != HexObject.AxialPosition))
        {
            SetHover(false);
            isHoverStateChanged = false;
        }
        #endregion
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            gameObject.GetComponentInParent<HexGrid>().CheckField(HexObject);
        }
    }

    public void SetReach(bool reach)
    {
        if (reach)
        {
            PushSprite(this.transform.Find("hex_reach").gameObject);
        }
        else
        {
            PopSprite();
        }
    }

    public void SetHover(bool hover)
    {
        if (hover)
        {
            PushSprite(this.transform.Find("hex_hover").gameObject);
        }
        else
        {
            PopSprite();
        }
    }

    #region SpriteStack
    private void PushSprite(GameObject sprite)
    {
        if(spriteStack.Any())
        {
            spriteStack.Peek().SetActive(false);
        }

        spriteStack.Push(sprite);
        sprite.SetActive(true);
    }

    private GameObject PopSprite()
    {
        GameObject removedObject = null;

        if(spriteStack.Count > 1)
        {
            removedObject = spriteStack.Pop();
            removedObject.SetActive(false);
            spriteStack.Peek().SetActive(true);
        }
        else if(spriteStack.Count == 1)
        {
            removedObject = spriteStack.Pop();
            removedObject.SetActive(false);
        }

        return removedObject;
    }
    #endregion
}
