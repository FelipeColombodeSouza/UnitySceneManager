using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;
    private static bool _loadFromDoor;

    private GameObject _player;
    private Collider2D _playerColl;
    private Collider2D _doorColl;
    private Vector3 _playerSpawnPosition;

    private DoorTrigger.DoorToSpawnAt _doorToSpawnTo;

    public GetConfiner getConf;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerColl = _player.GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    public static void SwapSceneFromDoorUse(SceneField myScene, DoorTrigger.DoorToSpawnAt doorToSpawnAt)
    {
        _loadFromDoor = true;
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTrigger.DoorToSpawnAt doorToSpawnAt = DoorTrigger.DoorToSpawnAt.None)
    {
        SceneFadeManager.instance.StartFadeOut();

        while (SceneFadeManager.instance.IsFadingOut)
        {
            yield return null;
        }

        _doorToSpawnTo = doorToSpawnAt;
        SceneManager.LoadScene(myScene);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.instance.StartFadeIn();

        if (_loadFromDoor)
        {
            FindDoor(_doorToSpawnTo);
            _player.transform.position = _playerSpawnPosition;

            _loadFromDoor = false;
        }
    }

    private void FindDoor(DoorTrigger.DoorToSpawnAt doorSpawnNumber)
    {
        DoorTrigger[] doors = FindObjectsOfType<DoorTrigger>();
        getConf.AssignConfiner();

        for(int i = 0; i < doors.Length; i++)
        {   
            if(doors[i].CurrentDoorPosition == doorSpawnNumber)
            {
                _doorColl = doors[i].gameObject.GetComponent<Collider2D>();

                CalculateSpawnPosition();

                return;
            }
        }
        
    }

    private void CalculateSpawnPosition()
    {
        float colliderHeight = _playerColl.bounds.extents.y;
        Vector3 spawnPos = new Vector3(_doorColl.transform.position.x, _doorColl.transform.position.y, _doorColl.transform.position.z) ;
        _playerSpawnPosition = spawnPos;
    }
}
