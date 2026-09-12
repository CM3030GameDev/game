using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D box;
    //Index for walls layer
    private int wallsLayer;
    //Index for ground layer
    private int groundLayer;
    //Door open state (True represent open, false represent closed)
    public bool open;
    [SerializeField] private Sprite doorOpen;
    [SerializeField] private Sprite doorClose;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        open = false;
        wallsLayer = LayerMask.NameToLayer("Walls");
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            //Show open door sprite
            sr.sprite = doorOpen;
            //Allow player to walk through
            box.isTrigger = true;
            //Change layer to ground layer
            gameObject.layer = groundLayer;
            //Change tag to floor tag
            gameObject.tag = "Floor";
        }
        else
        {
            //Show closed door sprite
            sr.sprite = doorClose;
            //Do not allow player to walk through
            box.isTrigger = false;
            //Change layer to walls layer
            gameObject.layer = wallsLayer;
            //Change tag to wall tag
            gameObject.tag = "Wall";
        }
    }
}
