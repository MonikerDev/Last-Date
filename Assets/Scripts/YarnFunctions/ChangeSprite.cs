using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    public Sprite angry;
    public Sprite sad;
    public Sprite pissed;
    public Sprite neutral;

    public Image target;

    private void Awake()
    {
        target = this.GetComponentInParent<Image>();
    }

    [YarnCommand("MakeAngry")]
    public void MakeAngry()
    {
        target.sprite = angry;
    }
    
    [YarnCommand("MakeSad")]
    public void MakeSad()
    {
        target.sprite = sad;
    }

    [YarnCommand("MakePissed")]
    public void MakePissed()
    {
        target.sprite = pissed;
    }

    [YarnCommand("MakeNeutral")]
    public void MakeNeutral()
    {
        target.sprite = neutral;
    }
}
