using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReuseScroll
{
    public class UIReuseGrid : MonoBehaviour, LoopScrollPrefabSource, LoopScrollMultiDataSource
    {
        [SerializeField]
        private LoopScrollRectMulti m_LoopScrollRect;

        //재사용 데이터를 보관하는 변수
        [HideInInspector]
        public ReuseBankBase m_ReuseBank;
        //private UIReuseItemCell[] m_cellList;

        public LoopScrollRectMulti m_ScrollRect { get; private set; }

        // Is Use MulitiPrefab
        public bool m_IsUseMultiPrefabs = false;
        // Cell Prefab
        public GameObject m_Item;
        // Cell MulitiPrefab
        public List<GameObject> m_ItemList = new List<GameObject>();

        // Implement your own Cache Pool here. The following is just for example.
        private Stack<Transform> pool = new Stack<Transform>();
        private Dictionary<string, Stack<Transform>> m_Pool_Type = new Dictionary<string, Stack<Transform>>();


        private void Awake()
        {
            if(GetComponentInChildren<LoopScrollRectBase>() == null)
            {
                Debug.LogError("m_ReuseScrollView == null");
                return;
            }
            InitData();
            m_ReuseBank.m_CellSizes.Add(new Vector2(120, 120));
        }
  
        private void InitData()
        {
            if (m_ReuseBank == null)
                m_ReuseBank = new ReuseBankBase();

            if (m_ScrollRect == null)
            {
                if (m_LoopScrollRect != null)
                    m_ScrollRect = m_LoopScrollRect;
                else
                    m_ScrollRect = GetComponent<LoopScrollRectMulti>();
            }
            m_ScrollRect.prefabSource = this;
            m_ScrollRect.dataSource = this;
        }

        #region Data Function

        public void AddItem(IReuseCellData cellData, bool Update = false)
        {
            if (!m_ReuseBank.IsInit())
                Debug.LogWarning("m_listData null");

            m_ReuseBank.AddCellData(cellData);
            if (Update)
                UpdateAllCellData();
        }
        public void InsertItem(int idx, IReuseCellData cellData, bool Update = false)
        {
            m_ReuseBank.InsertCellData(idx, cellData);
            if (Update)
                UpdateAllCellData();
        }
        public void SetItem(List<IReuseCellData> ListData, bool Update = false)
        {
            m_ReuseBank.SetListData(ListData);
            if (Update)
                UpdateAllCellData();
        }
        public void RemoveItem(IReuseCellData ListData, bool Update = false)
        {
            if (ListData == null)
                return;

            m_ReuseBank.DeleteCellData(ListData);
            if (Update)
            {
                m_ScrollRect.ClearCells();
                UpdateAllCellData();
            }
        }
        public void RemoveFirstItem()
        {
            m_ReuseBank.DelCellDataByIndex(0);

            float offset;
            int LeftIndex = m_ScrollRect.GetFirstItem(out offset);

            m_ScrollRect.ClearCells();
            m_ScrollRect.totalCount = m_ReuseBank.GetListDataLength();
            if (LeftIndex > 0)
                // try keep the same position
                m_ScrollRect.RefillCells(LeftIndex - 1, false, offset);
            else
                m_ScrollRect.RefillCells();
        }

        public void ClearItem(bool Update = false)
        {
            if (!m_ReuseBank.IsInit())
                return;

            m_ReuseBank.ClearListData();
            if (Update)
            {
                m_ScrollRect.ClearCells();
                UpdateAllCellData();
            }
        }
        #endregion

        public void UpdateAllCellData()
        {
            m_ScrollRect.totalCount = m_ReuseBank.GetListDataLength();
            m_ScrollRect.RefreshCells();
        }

        public void SortCellData_IndexOrder(bool isReverse = false)
        {
            // 람다식으로 정렬 구현
            var TempContent = m_ReuseBank.GetListData();
            if(!isReverse)
                TempContent.Sort((x, y) => x.index.CompareTo(y.index));
            else
                TempContent.Sort((x, y) => -x.index.CompareTo(y.index));

            //m_ScrollRect.ClearCells();
            UpdateAllCellData();
        }

        public void SrollToCell(int targetIndx, int moveSpeed = -1)
        {
            m_ScrollRect.SrollToCell(targetIndx, moveSpeed);
        }




        #region interface Method
        public virtual GameObject GetObject(int index)
        {
            Transform candidate = null;
            // Is Use MulitiPrefab
            if (!m_IsUseMultiPrefabs)
            {
                if (pool.Count == 0)
                {
                    candidate = Instantiate(m_Item.transform);
                }
                else
                {
                    candidate = pool.Pop();
                }

                // One Cell Prefab, Set PreferredSize as runtime.
                UIReuseItemCell TempScrollIndexCallbackBase = candidate.GetComponent<UIReuseItemCell>();
                if (null != TempScrollIndexCallbackBase)
                {
                    if (m_ScrollRect.horizontal)
                    {
                        float RandomWidth = m_ReuseBank.GetCellPreferredSize(index).x;
                        TempScrollIndexCallbackBase.SetLayoutElementPreferredWidth(RandomWidth);
                    }

                    if (m_ScrollRect.vertical)
                    {
                        float RandomHeight = m_ReuseBank.GetCellPreferredSize(index).y;
                        TempScrollIndexCallbackBase.SetLayoutElementPreferredHeight(RandomHeight);
                    }
                }
            }
            else
            {
                // Cell MulitiPrefab, Get Cell Preferred Type by custom data
                int CellTypeIndex = m_ReuseBank.GetCellPreferredTypeIndex(index);
                if (m_ItemList.Count <= CellTypeIndex)
                {
                    Debug.LogWarningFormat("TempPrefab is null! CellTypeIndex: {0}", CellTypeIndex);
                    return null;
                }
                var TempPrefab = m_ItemList[CellTypeIndex];

                Stack<Transform> TempStack = null;
                if (!m_Pool_Type.TryGetValue(TempPrefab.name, out TempStack))
                {
                    TempStack = new Stack<Transform>();
                    m_Pool_Type.Add(TempPrefab.name, TempStack);
                }

                if (TempStack.Count == 0)
                {
                    candidate = Instantiate(TempPrefab).GetComponent<Transform>();
                    UIReuseItemCell TempScrollIndexCallbackBase = candidate.GetComponent<UIReuseItemCell>();
                    if (null != TempScrollIndexCallbackBase)
                    {
                        TempScrollIndexCallbackBase.prefabName =TempPrefab.name;
                    }
                }
                else
                {
                    candidate = TempStack.Pop();
                    candidate.gameObject.SetActive(true);
                }
            }

            return candidate.gameObject;
        }

        public virtual void ReturnObject(Transform trans)
        {
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            // Is Use MulitiPrefab
            if (!m_IsUseMultiPrefabs)
            {
                pool.Push(trans);
            }
            else
            {
                // Use PrefabName as Key for Pool Manager
                UIReuseItemCell TempScrollIndexCallbackBase = trans.GetComponent<UIReuseItemCell>();
                if (null == TempScrollIndexCallbackBase)
                {
                    // Use `DestroyImmediate` here if you don't need Pool
                    DestroyImmediate(trans.gameObject);
                    return;
                }

                Stack<Transform> TempStack = null;
                if (m_Pool_Type.TryGetValue(TempScrollIndexCallbackBase.prefabName, out TempStack))
                {
                    TempStack.Push(trans);
                }
                else
                {
                    TempStack = new Stack<Transform>();
                    TempStack.Push(trans);

                    m_Pool_Type.Add(TempScrollIndexCallbackBase.prefabName, TempStack);
                }
            }
        }

        public virtual void ProvideData(Transform transform, int idx)
        {
            //??
            //transform.SendMessage("UpdateData", idx);

            // Use direct call for better performance
            transform.GetComponent<UIReuseItemCell>()?.UpdateData(idx, m_ReuseBank.GetCellData(idx));
        }

        #endregion
    }
}
