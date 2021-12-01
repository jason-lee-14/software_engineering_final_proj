using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class CapControlTest
{
    [Test]
    public void CapControl_Test()
    {
        var capControl = new CapControl();
        GameObject gameObject = new GameObject("test");
        gameObject.AddComponent<BoxCollider>().name = "cap_1";
        capControl.OnTriggerEnter(gameObject.GetComponent<BoxCollider>());       
        Assert.AreEqual(false, capControl.IsReady);
        Assert.AreEqual(false, capControl.IsHolding);
    }
}

