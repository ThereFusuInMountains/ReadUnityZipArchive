using UnityEngine;

/// <summary>
/// UI适配处理类
/// </summary>
public static class UIPosAdaptiveHanlder
{
    /// <summary>
    /// <see cref="RectTransform"/>悬停位置类型
    /// </summary>
    public enum RectTransfrom_HoverType : short
    {
        None = 0,
        /// <summary>
        /// 左下
        /// </summary>
        BottomLeft = 1 << 0,
        /// <summary>
        /// 靠左
        /// </summary>
        Left = 1 << 1,
        /// <summary>
        /// 左上
        /// </summary>
        TopLeft = 1 << 2,
        /// <summary>
        /// 顶部
        /// </summary>
        Top = 1 << 3,
        /// <summary>
        /// 右上
        /// </summary>
        TopRight = 1 << 4,
        /// <summary>
        /// 靠右
        /// </summary>
        Right = 1 << 5,
        /// <summary>
        /// 右下
        /// </summary>
        BottomRight = 1 << 6,
        /// <summary>
        /// 底部
        /// </summary>
        Bottom = 1 << 8,
        /// <summary>
        /// 居中
        /// </summary>
        Middle = 1 << 9,
    }

    /// <summary>
    /// 四周边距
    /// </summary>
    [System.Serializable]
    public struct Padding
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
    }

    public static void CalculateAdaptive(this RectTransform root, RectTransform child, Vector2 hoverPos, Padding padding = default)
    {
        var rootM_Rect = new M_Rect(root);
        var childM_Rect = new M_Rect(child);
        var center = rootM_Rect.bounds.center - new Vector3(padding.Right - padding.Left, padding.Bottom - padding.Top) * 0.5f;
        var size = rootM_Rect.bounds.size - new Vector3(padding.Left + padding.Right, padding.Top + padding.Bottom) - childM_Rect.bounds.size;
        //子物体 pos 区域
        var area = new Bounds(center, size);
        var areaM_Rect = new M_Rect(area);

        var pos = child.position;
        var percent = hoverPos;
        pos.x = Mathf.Lerp(areaM_Rect.bottomLfet.x, areaM_Rect.topRight.x, percent.x);
        pos.y = Mathf.Lerp(areaM_Rect.bottomLfet.y, areaM_Rect.topRight.y, percent.y);
        if (pos != child.position)
        {
            child.position = pos;
        }

        if (!area.Contains(pos))
        {
            child.position = area.ClosestPoint(pos);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="root">根物体</param>
    /// <param name="child">子物体</param>
    /// <param name="hoverType"></param>
    /// <param name="padding">边距</param>
    public static void CalculateAdaptive(this RectTransform root, RectTransform child, RectTransfrom_HoverType hoverType = RectTransfrom_HoverType.None, Padding padding = default)
    {
        var hoverPos = SwitchPosByHoverType(hoverType);
        CalculateAdaptive(root, child, hoverPos, padding);
    }

    /// <summary>
    /// 让<paramref name="child"/>在屏幕的某个方位
    /// </summary>
    /// <param name="child"></param>
    /// <param name="hoverType"></param>
    /// <param name="padding"></param>
    public static void CalculateAdaptiveWithScreen(this RectTransform child, RectTransfrom_HoverType hoverType = RectTransfrom_HoverType.Middle, Padding padding = default)
    {
        RectTransform root = GameObject.Find("UIRoot").GetComponent<RectTransform>();
        if (root == null)
        {
#if UNITY_EDITOR
            Debug.Log($"{nameof(CalculateAdaptiveWithScreen)}-{nameof(root)} IS NULL");
#endif
            return;
        }
        root.CalculateAdaptive(child, hoverType, padding);
    }

    private static Vector2 SwitchPosByHoverType(RectTransfrom_HoverType hoverType)
    {
        switch (hoverType)
        {
            case RectTransfrom_HoverType.BottomLeft:
                return new Vector2(0, 0);
            case RectTransfrom_HoverType.Left:
                return new Vector2(0, 0.5f);
            case RectTransfrom_HoverType.TopLeft:
                return new Vector2(0, 1);
            case RectTransfrom_HoverType.Top:
                return new Vector2(0.5f, 1);
            case RectTransfrom_HoverType.TopRight:
                return new Vector2(1, 1);
            case RectTransfrom_HoverType.Right:
                return new Vector2(1, 0.5f);
            case RectTransfrom_HoverType.BottomRight:
                return new Vector2(1, 0);
            case RectTransfrom_HoverType.Bottom:
                return new Vector2(0.5f, 0);
            case RectTransfrom_HoverType.Middle:
                return new Vector2(0.5f, 0.5f);
            default:
                return default;
        }
    }

    public static bool ForceAreaBounds(this RectTransform rectTransform, RectTransform root)
    {
        Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(root, rectTransform);
        Rect area = root.rect;
        Vector2 delta = Vector2.zero;
        if (bounds.center.x - bounds.extents.x < area.x)//左
        {
            delta.x += Mathf.Abs(bounds.center.x - bounds.extents.x - area.x);
        }
        else if (bounds.center.x + bounds.extents.x > area.width / 2)//右
        {
            delta.x -= Mathf.Abs(bounds.center.x + bounds.extents.x - area.width / 2);
        }
        if (bounds.center.y - bounds.extents.y < area.y)//上
        {
            delta.y += Mathf.Abs(bounds.center.y - bounds.extents.y - area.y);
        }
        else if (bounds.center.y + bounds.extents.y > area.height / 2)//下
        {
            delta.y -= Mathf.Abs(bounds.center.y + bounds.extents.y - area.height / 2);
        }
        rectTransform.anchoredPosition += delta;
        DrawBounds(bounds);
        DrawRange(area);
        return delta != Vector2.zero;
    }

    public static void DrawRect(RectTransform rect)
    {
        var pos = rect.position;
        var size = rect.sizeDelta * rect.lossyScale.x;

        Bounds bounds = new Bounds(pos, size);
        DrawBounds(bounds);
    }

    private static void DrawRange(Rect rect)
    {
        var topLfet = rect.center + new Vector2(-rect.width / 2, rect.height / 2);
        var bottomLfet = rect.center + new Vector2(-rect.width / 2, -rect.height / 2);
        var bottomRight = rect.center + new Vector2(rect.width / 2, -rect.height / 2);
        var topRight = rect.center + new Vector2(rect.width / 2, rect.height / 2);
        Debug.DrawLine(topLfet, bottomLfet, Color.green);
        Debug.DrawLine(bottomLfet, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, topRight, Color.green);
        Debug.DrawLine(topRight, topLfet, Color.green);
    }

    private static void DrawBounds(Bounds bounds)
    {
        var topLfet = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, 0);
        var bottomLfet = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, 0);
        var bottomRight = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, 0);
        var topRight = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, 0);
        Debug.DrawLine(topLfet, bottomLfet, Color.red);
        Debug.DrawLine(bottomLfet, bottomRight, Color.red);
        Debug.DrawLine(bottomRight, topRight, Color.red);
        Debug.DrawLine(topRight, topLfet, Color.red);
    }

    private static void DrawM_Rect(M_Rect m_Rect)
    {
        DrawBounds(m_Rect.bounds);
    }

    private struct M_Rect
    {
        private Bounds m_bounds;
        private Vector2 m_topLfet;
        private Vector2 m_bottomLfet;
        private Vector2 m_bottomRight;
        private Vector2 m_topRight;

        public Bounds bounds
        {
            get
            {
                return m_bounds;
            }
            set
            {
                m_bounds = value;
                this.m_topLfet = m_bounds.center + new Vector3(-m_bounds.extents.x, m_bounds.extents.y, 0);
                this.m_bottomLfet = m_bounds.center + new Vector3(-m_bounds.extents.x, -m_bounds.extents.y, 0);
                this.m_bottomRight = m_bounds.center + new Vector3(m_bounds.extents.x, -m_bounds.extents.y, 0);
                this.m_topRight = m_bounds.center + new Vector3(m_bounds.extents.x, m_bounds.extents.y, 0);
            }
        }
        public Vector2 topLfet
        {
            get
            {
                return m_topLfet;
            }
        }
        public Vector2 bottomLfet
        {
            get
            {
                return m_bottomLfet;
            }
        }
        public Vector2 bottomRight
        {
            get
            {
                return m_bottomRight;
            }
        }
        public Vector2 topRight
        {
            get
            {
                return m_topRight;
            }
        }

        public M_Rect(Bounds bounds)
        {
            this.m_bounds = bounds;
            this.m_topLfet = m_bounds.center + new Vector3(-m_bounds.extents.x, m_bounds.extents.y, 0);
            this.m_bottomLfet = m_bounds.center + new Vector3(-m_bounds.extents.x, -m_bounds.extents.y, 0);
            this.m_bottomRight = m_bounds.center + new Vector3(m_bounds.extents.x, -m_bounds.extents.y, 0);
            this.m_topRight = m_bounds.center + new Vector3(m_bounds.extents.x, m_bounds.extents.y, 0);
        }

        public M_Rect(RectTransform rect)
        {
            var pos = rect.position;
            var size = rect.sizeDelta * rect.lossyScale.x;

            this.m_bounds = new Bounds(pos, size);
            this.m_topLfet = m_bounds.center + new Vector3(-m_bounds.extents.x, m_bounds.extents.y, 0);
            this.m_bottomLfet = m_bounds.center + new Vector3(-m_bounds.extents.x, -m_bounds.extents.y, 0);
            this.m_bottomRight = m_bounds.center + new Vector3(m_bounds.extents.x, -m_bounds.extents.y, 0);
            this.m_topRight = m_bounds.center + new Vector3(m_bounds.extents.x, m_bounds.extents.y, 0);
        }
    }
}