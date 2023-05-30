using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool isGameRunning;
    private float scrollSpeed = 70;
    [SerializeField] private float xSpawn;
    [SerializeField] private float xTrigger;
    [SerializeField] private float xDestroy;

    public float groundHeight;
    private BoxCollider2D col;

    private bool calledGround = false;

    #region Events
    private void OnEnable()
    {
        GameManager.StartGame += StartGround;
        GameManager.StopGame += StopGround;
    }
    private void OnDisable()
    {
        GameManager.StartGame -= StartGround;
        GameManager.StopGame -= StopGround;
    }
    private void StartGround()
    {
        isGameRunning = true;
    }
    private void StopGround()
    {
        isGameRunning = false;
    }
    #endregion

    private void Awake()
    {
        if (transform.position.x == xSpawn) isGameRunning = true;

        col = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + col.bounds.extents.y;
    }

    private void Update()
    {
        if (isGameRunning)
        {
            float velocity = scrollSpeed / 10;
            Vector2 pos = transform.position;
            pos.x -= velocity * Time.deltaTime;
            transform.position = pos;

            if (transform.position.x <= xDestroy)
            {
                Destroy(gameObject);
                return;
            }

            if (!calledGround)
            {
                if (transform.position.x <= xTrigger) callGround();
            }
        }
    }

    private void callGround()
    {
        calledGround = true;
        GameObject gr = Instantiate(gameObject);
        gr.name = this.name;
        Vector2 pos = new(xSpawn, transform.position.y);
        gr.transform.position = pos;
    }
}
