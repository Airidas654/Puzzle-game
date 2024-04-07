using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
//using UnityEngine.InputSystem;

public class PlayerDeathTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void PlayerDeathTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerDeathTestWithEnumeratorPasses()
    {
        
        yield return null;
    }
}
