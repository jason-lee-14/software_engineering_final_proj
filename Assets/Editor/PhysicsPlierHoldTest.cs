using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class PhysicsPlierHoldTest
{
    [Test]
    public void PhysicsPlierHoldTest_Test()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject gameObject1 = new GameObject();
        Rigidbody gameObjectsRigidBody = gameObject1.AddComponent<Rigidbody>();
        var pliersHold = gameObject1.AddComponent<physicsPlierHold>();

        try
        {
            pliersHold.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }    
        Assert.AreEqual(false, pliersHold.holding);
       
    }
}
