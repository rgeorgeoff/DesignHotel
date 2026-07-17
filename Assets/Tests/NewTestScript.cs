using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
using Managers;

public class NewTestScript {

    [Test]
    public void NewTestScriptSimplePasses() {

    }

    [Test]
    public void TestEventManager() { 
        // Use the Assert class to test conditions.
        EventManager em = new EventManager();
        EventTest et = new EventTest();
        et.Awake();
        et.OnEnable();
        em.TriggerEvent(this, "test", EventArgs.Empty);
        LogAssert.Expect(LogType.Log, "Some Function was called!");
        et.OnDisable();
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
