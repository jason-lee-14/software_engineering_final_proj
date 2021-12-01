using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;


public class SwitchFlipTest
{
    [Test]
    public void SwitchFlipTest_Test()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject gameObject = new GameObject();
        
        Rigidbody gameObjectsRigidBody = gameObject.AddComponent<Rigidbody>();
        var switchFlip = gameObject.AddComponent<SwitchFlip>();
        try
        {
            switchFlip.indicator = new GameObject();
            switchFlip.Start();           
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Assert.AreEqual(-18.5f, switchFlip.GetFlipAngle);
        Assert.AreEqual(0f, switchFlip.GetStartAngle);      
    }
}
