using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Grid : MonoBehaviour
    {
        public abstract class Item : Brain
        {
            private Tile                m_tile;

            #region Properties

            public Tile Tile
            {
                get => m_tile;
                set
                {
                    if (m_tile != value)
                    {
                        OnLeaveTile(m_tile);
                        m_tile = value;
                        OnEnterTile(m_tile);
                    }
                }
            }

            #endregion

            protected virtual void OnDestroy()
            {
                OnLeaveTile(m_tile);
            }

            protected virtual void OnLeaveTile(Tile tile) 
            {
                tile?.RemoveItem(this);
            }

            protected virtual void OnEnterTile(Tile tile) 
            {
                tile?.AddItem(this);
            }
        }

        public class Tile
        {
            private Vector2Int  m_vPosition;
            private List<Item>  m_items = new List<Item>();

            #region Properties

            public Vector2Int GridPosition => m_vPosition;

            public Vector3 WorldPosition => new Vector3(m_vPosition.x, 0, m_vPosition.y);

            public IEnumerable<Item> Items => m_items;

            public bool Empty => m_items.Count == 0;

            public IEnumerable<Tile> Neighbors
            {
                get
                {
                    if (sm_instance != null)
                    {
                        for (int z = -1; z <= 1; ++z)
                        {
                            for (int x = -1; x <= 1; ++x)
                            {
                                if (x != 0 || z != 0)
                                {
                                    Vector2Int v = m_vPosition + new Vector2Int(x, z);
                                    if (v.x >= 0 && v.x < sm_instance.m_iSize &&
                                        v.y >= 0 && v.y < sm_instance.m_iSize)
                                    {
                                        Tile tile = sm_instance.m_grid[v.x, v.y];
                                        if(tile != null)
                                        {
                                            yield return tile;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            public Tile(Vector2Int vPosition)
            {
                m_vPosition = vPosition;
            }

            public void AddItem(Item item)
            {
                if (item != null && !m_items.Contains(item))
                {
                    m_items.Add(item);
                }
            }

            public void RemoveItem(Item item)
            {
                if (item != null && m_items.Contains(item))
                {
                    m_items.Remove(item);
                }
            }

            public T GetItem<T>() where T : Item
            {
                return m_items.Find(itm => itm is T) as T;
            }

            public bool HasItem<T>() where T : Item
            {
                return GetItem<T>() != null;
            }
        }

        [SerializeField]
        public GameObject       m_tilePrefab;

        [SerializeField]
        public GameObject[]     m_blockedTilePrefabs;

        [SerializeField]
        public GameObject       m_carrotPrefab;

        [SerializeField]
        public GameObject       m_bunnyPrefab;
        
        [SerializeField]
        public GameObject       m_wolfPrefab;

        [SerializeField]
        public int              m_iSize = 5;

        private Tile[,]         m_grid;
        private static Grid     sm_instance;

        #region Properties

        public static Grid Instance => sm_instance;

        #endregion

        private void OnEnable()
        {
            sm_instance = this;
        }

        private void Start()
        {
            // create grid
            m_grid = new Tile[m_iSize, m_iSize];
            for (int z = 0; z < m_iSize; z++)
            {
                for (int x = 0; x < m_iSize; x++)
                {
                    // spawn gameobject
                    bool bBlocked = Random.value < 0.1f;
                    GameObject go = Instantiate(bBlocked ? m_blockedTilePrefabs[Random.Range(0, m_blockedTilePrefabs.Length)] : m_tilePrefab, transform);
                    go.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
                    go.transform.position = new Vector3(x, 0, z);

                    // create tile?
                    if (!bBlocked)
                    {
                        m_grid[x, z] = new Tile(new Vector2Int(x, z));
                    }
                }
            }

            // create a few starting items
            for (int i = 0; i < 10; ++i)
            {
                CreateItemAt(m_carrotPrefab, GetRandomEmptyTile());                
            }

            for (int i = 0; i < 5; ++i)
            {
                CreateItemAt(m_bunnyPrefab, GetRandomEmptyTile());
            }
            
            for (int i = 0; i < 2; ++i)
            {
                CreateItemAt(m_wolfPrefab, GetRandomEmptyTile());
            }
        }

        public Tile GetRandomEmptyTile()
        {
            List<Tile> emptyTiles = new List<Tile>();
            foreach (Tile tile in m_grid)
            {
                if (tile != null && tile.Empty)
                {
                    emptyTiles.Add(tile);
                }
            }

            return emptyTiles.Count > 0 ? emptyTiles[Random.Range(0, emptyTiles.Count)] : null;
        }

        public Item CreateItemAt(GameObject prefab, Tile tile)
        {
            GameObject go = Instantiate(prefab, transform);
            go.name = prefab.name;
            go.transform.position = tile.WorldPosition;
            Item item = go.GetComponent<Item>();
            if (item != null)
            {
                item.Tile = tile;
            }

            return item;
        }

        public T GetClosestItem<T>(Vector3 vPosition, Item itemToIgnore = null, float fMaxDistance = float.MaxValue) where T : Item
        {
            T closestItem = null;
            foreach (T item in GetComponentsInChildren<T>())
            {
                if (itemToIgnore != null)
                {
                    if (item != itemToIgnore)
                    {
                        float fDistance = Vector3.Distance(vPosition, item.transform.position);
                        if (fDistance < fMaxDistance)
                        {
                            fMaxDistance = fDistance;
                            closestItem = item;
                        }
                    }
                }
                else
                {
                    float fDistance = Vector3.Distance(vPosition, item.transform.position);
                    if (fDistance < fMaxDistance)
                    {
                        fMaxDistance = fDistance;
                        closestItem = item;
                    }
                }
            }

            return closestItem;
        }
    }
}