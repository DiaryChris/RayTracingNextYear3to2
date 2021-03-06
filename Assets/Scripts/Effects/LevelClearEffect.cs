﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelClearEffect : MonoBehaviour
{
    public float emitSpeed = 100f;
    public float emitRadius = 1000f;

    public PlayableDirector levelCleanDirector;
    public PlayableDirector lookBackDirector;

    private SphereCollider trigger;
    private bool isEmitting;

    private void Start()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.radius = 0;
    }

    private void Update()
    {
        if (GameManager.instance.isVictory || Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q Effect");
            isEmitting = true;
            levelCleanDirector.Play();
            lookBackDirector.Play();
        }
        if (isEmitting)
        {
            
            EmitEffect(Time.deltaTime);
        }
    }

    private void EmitEffect(float dt)
    {
        trigger.radius += emitSpeed * dt;
    }




    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("new trigger enter, name:" + other.gameObject.name);

        LevelClearReceiver receiver = other.GetComponent<LevelClearReceiver>();
        if (receiver == null)
        {
            return;
        }

        receiver.OnLevelClear();
    }

}
