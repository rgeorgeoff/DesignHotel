using UnityEngine;
using System.Collections;
using System;
using Managers;

public class EventTest : MonoBehaviour
{
    private EventHandler<EventArgs> someListener1;
    private EventHandler<EventArgs> someListener2;
    private EventHandler<EventArgs> someListener3;
    EventManager em = new EventManager();

    public void Awake()
    {
        someListener1 = new EventHandler<EventArgs>(SomeFunction);
        someListener2 = new EventHandler<EventArgs>(SomeOtherFunction);
        someListener3 = new EventHandler<EventArgs>(SomeThirdFunction);

        StartCoroutine(invokeTest());
    }

    IEnumerator invokeTest()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);

        //Create parameter to pass to the event
        EventArgs EventArgs = new EventArgs();

        while (true)
        {
            yield return waitTime;
            em.TriggerEvent(this, "test", EventArgs);
            yield return waitTime;
            em.TriggerEvent(this, "Spawn", EventArgs);
            yield return waitTime;
            em.TriggerEvent(this, "Destroy", EventArgs);
        }
    }

    public void OnEnable()
    {
        //Register With EventHandler variable
        em.StartListening("test", someListener1);
        em.StartListening("Spawn", someListener2);
        em.StartListening("Destroy", someListener3);

        //OR Register Directly to function
        em.StartListening("test", SomeFunction);
        em.StartListening("Spawn", SomeOtherFunction);
        em.StartListening("Destroy", SomeThirdFunction);
    }

    public void OnDisable()
    {
        //Un-Register With EventHandler variable
        em.StopListening("test", someListener1);
        em.StopListening("Spawn", someListener2);
        em.StopListening("Destroy", someListener3);

        //OR Un-Register Directly to function
        em.StopListening("test", SomeFunction);
        em.StopListening("Spawn", SomeOtherFunction);
        em.StopListening("Destroy", SomeThirdFunction);
    }

    void SomeFunction(object sender, EventArgs e)
    {
        Debug.Log("Some Function was called!");
    }

    void SomeOtherFunction(object sender, EventArgs e)
    {
        Debug.Log("Some Other Function was called!");
    }

    void SomeThirdFunction(object sender, EventArgs e)
    {
        Debug.Log("Some Third Function was called!");
    }
}