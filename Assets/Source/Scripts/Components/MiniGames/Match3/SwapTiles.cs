using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTiles : MonoBehaviour
{
    public GameObject Tile;
    public Sprite Sprite { get; set; }
    public Match3MiniGame Match3MiniGame;
    public int Index;
    public int TileIndex;

    public bool IsResource;

    private bool _isSwap;
    private bool _matchFound;
    private const int _deadZone = 5;
    private Vector2 _mousePosition;
    private BoxCollider2D _boxCollider2D;

    List<SwapTiles> _matchingTiles = new List<SwapTiles>();

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void OnMouseDown()
    {
        _boxCollider2D.enabled = false;
        _mousePosition = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        Vector2 swipe = new Vector2(_mousePosition.x - Input.mousePosition.x, _mousePosition.y - Input.mousePosition.y);

        if (swipe.y < 0 && swipe.y < - _deadZone)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);

            if (hit.collider != null)
            {
                Match3MiniGame.SwapTiles(Index - 5, 5);
                Debug.Log("Up");
            }

            _isSwap = true;
            hit.collider.GetComponent<SwapTiles>().ClearAllMatches();
        }

        else if (swipe.y > 0 && swipe.y > _deadZone)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

            if (hit.collider != null)
            {
                Match3MiniGame.SwapTiles(Index + 5, -5);
                Debug.Log("Down");
            }

            _isSwap = true;
            hit.collider.GetComponent<SwapTiles>().ClearAllMatches();
        }

        else if (swipe.x > 0 && swipe.x > _deadZone)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left);

            if (hit.collider != null)
            {
                Match3MiniGame.SwapTiles(Index - 1, 1);
                Debug.Log("Left");
            }

            _isSwap = true;
            hit.collider.GetComponent<SwapTiles>().ClearAllMatches();
        }

        else if (swipe.x < 0 && swipe.x < -_deadZone)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right);

            if (hit.collider != null)
            {
                Match3MiniGame.SwapTiles(Index + 1, -1);
                Debug.Log("Right");
            }

            _isSwap = true;
            hit.collider.GetComponent<SwapTiles>().ClearAllMatches();
        }

        _boxCollider2D.enabled = true;
        ClearAllMatches();
    }

    private List<SwapTiles> FindMatch(Vector2 castDir)
    {
        List<SwapTiles> matchingTiles = new List<SwapTiles>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);

        while (hit.collider != null && IsResource && hit.collider.GetComponent<SwapTiles>().TileIndex == TileIndex)
        {
            hit.collider.enabled = false;
            matchingTiles.Add(hit.collider.GetComponent<SwapTiles>());
            hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
        }

        return matchingTiles;
    }

    private void ClearMatch(Vector2[] paths)
    {
        List<SwapTiles> matchingTiles = new List<SwapTiles>();

        for (int i = 0; i < paths.Length; i++)
        {
            matchingTiles.AddRange(FindMatch(paths[i]));
        }

        for (int i = 0; i < matchingTiles.Count; i++)
        {
            matchingTiles[i].GetComponent<BoxCollider2D>().enabled = true;
        }

        if (matchingTiles.Count > 2)
        {
            for (int i = 0; i < matchingTiles.Count; i++)
            {
                _matchingTiles.Add(matchingTiles[i]);
            }

            _matchFound = true;
        }
    }

    public void ClearAllMatches()
    {
        StopCoroutine(Match3MiniGame.SpawnTiles());

        if (IsResource == false)
            return;

        ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });

        if (_matchFound)
        {
            _matchFound = false;
            StopCoroutine(DestroyTiles());
            StartCoroutine(DestroyTiles());
        }
    }

    private IEnumerator DestroyTiles()
    {
        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < _matchingTiles.Count; i++)
        {
            _matchingTiles[i].IsResource = false;
            Destroy(_matchingTiles[i].Tile);
        }

        _matchingTiles.Clear();
        Match3MiniGame.CountToWin(TileIndex, _matchingTiles.Count);
        StartCoroutine(Match3MiniGame.SpawnTiles());
    }
}
