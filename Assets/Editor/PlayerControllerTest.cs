using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;


public class PlayerControllerTest
{
    [Test]
    public void PlayerStart_Test()
    {
        GameObject gameObject1 = new GameObject();
        Rigidbody gameObjectsRigidBody = gameObject1.AddComponent<Rigidbody>(); 
        var playerController = gameObject1.AddComponent<PlayerController>();
        try{
            playerController.Start();
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }
        Assert.AreEqual(playerController.vecSpeed*Vector3.forward, playerController.GetStepVector);
        Assert.AreEqual(playerController.vecSpeed*Vector3.right, playerController.GetSideVector);
        Assert.AreEqual(playerController.vecSpeed*Vector3.up, playerController.GetUpDownVector);
        Assert.AreEqual(false, playerController.rotateLock);

    }
}

