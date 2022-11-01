using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //This template can be customized at C:\Program Files\Unity\Hub\Editor\2021.3.8f1\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt
using System;
using TMPro;
using Firebase;
using Firebase.Messaging;

public class MessageController : MonoBehaviour
{
    FirebaseApp _app;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;
            }
            else
            {
                Debug.LogError("[FIREBASE] Could not resolve all dependencies: " + task.Result);
            }
        });
    }

    void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        if (e!= null)
        {
            Debug.LogError("[FIREBASE] Tocken: " + e.Token);            
        }
    }

    void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if (e != null && e.Message != null && e.Message.Notification != null)
        {
            Debug.LogError("[FIREBASE] From: "+ e.Message.From+", Title: +"+ e.Message.Notification.Title+", Text: "+ e.Message.Notification.Body);
        }
    }

  
}