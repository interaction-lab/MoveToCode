using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Firebase;
using Firebase.Storage;


public class FirebaseInit : MonoBehaviour
{
    public UnityEvent OnFirebaseInitialized = new UnityEvent();
    private async void Start()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            OnFirebaseInitialized.Invoke();
        }
    }
}
