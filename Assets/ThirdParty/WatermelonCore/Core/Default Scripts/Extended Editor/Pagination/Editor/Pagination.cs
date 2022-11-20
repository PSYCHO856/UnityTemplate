using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    /// <summary>
    ///     Editor ui pagination extension
    /// </summary>
    public class Pagination
    {
        public delegate void PaginationCallback();

        private int m_CurrentPage;

        private readonly int m_ElementsPerPage;

        private int m_PagesCount;
        private readonly int m_PaginationMaxElements;

        public PaginationCallback onPageChanged;

        public Pagination(int pageElements, int paginationMaxElements)
        {
            m_ElementsPerPage = pageElements;
            m_PaginationMaxElements = paginationMaxElements;
        }

        public int arrayLenth { get; private set; }

        public void Init(SerializedProperty array)
        {
            arrayLenth = array.arraySize;

            m_PagesCount = arrayLenth / m_ElementsPerPage + (arrayLenth % m_ElementsPerPage > 0 ? 1 : 0);

            InitCurrentPage();
        }

        public void Init(int arraySize)
        {
            arrayLenth = arraySize;

            m_PagesCount = arrayLenth / m_ElementsPerPage + (arrayLenth % m_ElementsPerPage > 0 ? 1 : 0);

            InitCurrentPage();
        }

        private void InitCurrentPage()
        {
            if (m_CurrentPage >= m_PagesCount)
                SetPage(0);
        }

        public void SetPage(int id)
        {
            if (m_CurrentPage == id)
                return;

            if (onPageChanged != null)
                onPageChanged.Invoke();

            m_CurrentPage = id;
        }

        public int GetPage(int index)
        {
            return index / m_ElementsPerPage;
        }

        public int GetMinElementNumber()
        {
            return m_CurrentPage * m_ElementsPerPage;
        }

        public int GetMaxElementNumber()
        {
            var maxElementNumber = (m_CurrentPage + 1) * m_ElementsPerPage;

            if (maxElementNumber > arrayLenth)
                maxElementNumber = maxElementNumber - (maxElementNumber - arrayLenth);

            return maxElementNumber;
        }

        public void DrawPagination()
        {
            if (arrayLenth <= m_ElementsPerPage)
                return;

            var firstPage = m_CurrentPage == 0;
            var lastPage = m_CurrentPage == m_PagesCount - 1;
            var cutPositions = m_PaginationMaxElements < m_PagesCount;

            var minPos = 0;
            var maxPos = m_PagesCount;

            if (cutPositions)
            {
                var plusPos = m_PaginationMaxElements / 2;
                if (m_PaginationMaxElements % 2 == 0)
                    plusPos = 0;

                minPos = m_CurrentPage - plusPos >= 0 ? m_CurrentPage - plusPos : 0;
                maxPos = minPos + m_PaginationMaxElements > m_PagesCount
                    ? m_PagesCount
                    : minPos + m_PaginationMaxElements;
                minPos = maxPos - m_PaginationMaxElements >= 0 ? maxPos - m_PaginationMaxElements : 0;
            }

            EditorGUILayout.BeginHorizontal();
            if (firstPage)
                GUI.enabled = false;

            if (GUILayout.Button("<<", EditorStyles.miniButtonLeft)) SetPage(0);
            if (GUILayout.Button("<", EditorStyles.miniButtonMid)) SetPage(m_CurrentPage - 1);
            if (firstPage)
                GUI.enabled = true;

            for (var i = minPos; i < maxPos; i++)
            {
                var isCurrent = m_CurrentPage == i;
                if (isCurrent)
                    GUI.enabled = false;

                if (GUILayout.Button((i + 1).ToString(), EditorStyles.miniButtonMid)) SetPage(i);

                if (isCurrent)
                    GUI.enabled = true;
            }

            if (lastPage)
                GUI.enabled = false;
            if (GUILayout.Button(">", EditorStyles.miniButtonMid)) SetPage(m_CurrentPage + 1);
            if (GUILayout.Button(">>", EditorStyles.miniButtonRight)) SetPage(m_PagesCount - 1);

            if (lastPage)
                GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
    }
}