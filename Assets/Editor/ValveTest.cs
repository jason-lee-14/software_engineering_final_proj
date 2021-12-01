using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class ValveTest
{
    [Test]
    public void TestValveTurn_Test()
    {
        var valveTurn = new ValveTurn();
        valveTurn.TurnValve();
        Assert.AreEqual(true, valveTurn.IsClosed);
    }
}
