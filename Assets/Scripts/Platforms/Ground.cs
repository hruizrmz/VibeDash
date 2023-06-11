using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool isGameRunning;
    [SerializeField] private float xDestroy;

    public float groundHeight;
    private BoxCollider2D boxCol;
    private EdgeCollider2D edgeCol;

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
        if (GetComponent<BoxCollider2D>() != null)
        {
            boxCol = GetComponent<BoxCollider2D>();
            groundHeight = transform.position.y + boxCol.bounds.extents.y;
        }
        else
        {
            edgeCol = GetComponent<EdgeCollider2D>();
            groundHeight = transform.position.y + edgeCol.bounds.extents.y;
        }    
    }

    private void Update()
    {
        if (isGameRunning)
        {
            if (transform.position.x <= xDestroy)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
