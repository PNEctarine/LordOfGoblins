using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Match3MiniGame : MonoBehaviour
{
    public List<SwapTiles> Tiles = new List<SwapTiles>();

    [SerializeField] private int _index;
    [SerializeField] private int _countToWin;

    [SerializeField] private GameObject[] _points;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private GameObject _object;
    [SerializeField] private float _animationDuration;

    [SerializeField] private List<GameObject> _resources = new List<GameObject>();

    private int _count;

    private void Start()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            GameObject resource = Instantiate(_object, _points[i].transform);
            SwapTiles swapTiles = _points[i].GetComponent<SwapTiles>();

            int random;

            do
            {
                random = Random.Range(0, _sprites.Length);
                resource.GetComponent<SpriteRenderer>().sprite = _sprites[random];
            }
            while (i - 1 >= 0 && _resources[i - 1].GetComponent<SpriteRenderer>().sprite == _sprites[random] ||
            i - 5 >= 0 && _resources[i - 5].GetComponent<SpriteRenderer>().sprite == _sprites[random]);

            swapTiles.TileIndex = random;
            swapTiles.Tile = resource;
            swapTiles.Index = i;
            swapTiles.Match3MiniGame = this;
            swapTiles.IsResource = true;

            Tiles.Add(swapTiles);
            _resources.Add(resource);
        }

        _resources.Clear();
    }

    public void SwapTiles(int index, int index2)
    {
        GameObject tile = Tiles[index + index2].Tile;
        int tileIndex = Tiles[index + index2].TileIndex;

        Tiles[index + index2].Tile.transform.DOMove(Tiles[index].gameObject.transform.position, _animationDuration);
        Tiles[index + index2].Tile.transform.parent = Tiles[index].gameObject.transform;
        Tiles[index + index2].Tile = Tiles[index].Tile;
        Tiles[index + index2].TileIndex = Tiles[index].TileIndex;


        Tiles[index].Tile.transform.parent = Tiles[index + index2].gameObject.transform;
        Tiles[index].Tile.transform.DOMove(Tiles[index + index2].gameObject.transform.position, _animationDuration);
        Tiles[index].Tile = tile;
        Tiles[index].TileIndex = tileIndex;
    }

    public void CountToWin(int index, int count)
    {
        if (index == _index )
        {
            _count += count;
            if (_count >= _countToWin)
            {
                Debug.Log("Win");
            }
        }
    }

    public IEnumerator SpawnTiles()
    {
        yield return new WaitForFixedUpdate();

        for (int lines = 0; lines < 7; lines++)
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i].IsResource == false)
                {
                    if (i - 5 >= 0)
                    {
                        Tiles[i].Tile = Tiles[i - 5].Tile;
                        Tiles[i].TileIndex = Tiles[i - 5].TileIndex;
                        Tiles[i].IsResource = true;

                        Tiles[i - 5].Tile.transform.position = Tiles[i].gameObject.transform.position;
                        Tiles[i - 5].Tile.transform.parent = Tiles[i].gameObject.transform;
                        Tiles[i - 5].IsResource = false;
                        Tiles[i - 5].Tile = null;
                    }

                    else
                    {
                        GameObject resource = Instantiate(_object, _points[i].transform);
                        List<Sprite> sprites = _sprites.ToList();
                        int random;

                        do
                        {
                            random = Random.Range(0, sprites.Count);
                            resource.GetComponent<SpriteRenderer>().sprite = _sprites[random];
                            //sprites.RemoveAt(random);
                        }
                        while (i - 1 >= 0 && Tiles[i - 1].Sprite == _sprites[random] ||
                        i - 5 >= 0 && Tiles[i - 5].Sprite == _sprites[random]);

                    Tiles[i].TileIndex = random;
                        Tiles[i].Tile = resource;
                        Tiles[i].Match3MiniGame = this;
                        Tiles[i].IsResource = true;
                    }
                }

                Tiles[i].GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        for (int i = 0; i < Tiles.Count; i++)
        {
            Tiles[i].ClearAllMatches();
            _resources.Clear();
        }
    }
}
