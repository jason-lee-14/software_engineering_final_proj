using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class LightTriggerTest
{
    [Test]
    public void LightTriggerTest_Test()
    {
        GameObject gameObject1 = new GameObject();
        Rigidbody gameObjectsRigidBody = gameObject1.AddComponent<Rigidbody>(); 
        var lightTrigger = gameObject1.AddComponent<LightTrigger>();
        Collider collider = gameObject1.AddComponent<BoxCollider>();
        collider.gameObject.tag = "MainCamera";
        lightTrigger.OnTriggerEnter(collider);
        Assert.AreEqual(1.5f, lightTrigger.GetLight.intensity);
        Assert.AreEqual(5,lightTrigger.GetLight.range);
    }
}

