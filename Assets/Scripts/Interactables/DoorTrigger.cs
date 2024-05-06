using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : InteractBase
{
    public enum DoorToSpawnAt
    {
        None, One, Two, Three, Four,
    }

    [Header("Spawn TO")]
    [SerializeField] private DoorToSpawnAt DoorToSpawnTo;
    [SerializeField] private SceneField _sceneToLoad;

    [Space(10f)]
    [Header("This door")]
    public DoorToSpawnAt CurrentDoorPosition;

    public override void Interact()
    {
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, DoorToSpawnTo);
    }
}
