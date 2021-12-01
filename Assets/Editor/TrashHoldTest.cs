using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;


public class TrashHoldTest
{
    [Test]
    public void TrashHoldTest_Test()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject gameObject1 = new GameObject();
        Rigidbody gameObjectsRigidBody = gameObject1.AddComponent<Rigidbody>(); 
        var trashHold = gameObject1.AddComponent<TrashHold>();
        trashHold.holding = false;
        try{
            trashHold.hold_control();

        }catch(Exception e){
            Console.WriteLine(e.Message);
        }
        Assert.AreEqual(0, trashHold.collideCounter);
    }
}

