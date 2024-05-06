using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetConfiner : MonoBehaviour
{
    public static GetConfiner instance;
    private Cinemachine.CinemachineConfiner2D cine;
    private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null){ instance = this; }

        camera = GameObject.Find("mainCamera");
        cine = camera.GetComponent<Cinemachine.CinemachineConfiner2D>();

        AssignConfiner();
    }

    private void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            AssignConfiner();
        }
    }

    public void AssignConfiner()
    {
        cine.m_BoundingShape2D = GameObject.Find("Confiner").GetComponent<PolygonCollider2D>();
    }
}
