using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Structure_old;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PortalPainter : MonoBehaviour {
    public Tilemap tilemap;

    public Portal Draw(Vector2Int from, Vector2Int to) {
        Portal portal = new Portal(tilemap);
        portal.from = from;
        portal.to = to;
        return portal;
    }
}

public class Portal {
    private LineRenderer mRenderer;
    private GameObject mObject;
    private Tilemap mTilemap;
    private Vector2Int mFrom;
    private Vector2Int mTo;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Vector2Int from {
        get => mFrom;
        set {
            mFrom = value;
            SetPosition(0, (Vector3Int) value);
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Vector2Int to {
        get => mTo;
        set {
            mTo = value;
            SetPosition(1, (Vector3Int) value);
        }
    }

    public Portal(Tilemap tilemap) {
        mObject = new GameObject("Portal");
        mObject.AddComponent<LineRenderer>();
        mRenderer = mObject.GetComponent<LineRenderer>();
        mRenderer.startWidth = (float) 0.1;
        mRenderer.endWidth = (float) 0.1;
        mRenderer.material = new Material(Shader.Find("Sprites/Default"));
        mRenderer.startColor = Color.cyan;
        mRenderer.endColor = Color.white;
        mRenderer.sortingLayerID = SortingLayer.NameToID("PortalArrows");
        mTilemap = tilemap;
    }

    private void SetPosition(int idx, Vector3Int cell) {
        mRenderer.SetPosition(idx, mTilemap.CellToLocal(new Vector3Int(cell.x, cell.y, 0)));
    }

    public void Destroy() {
        Object.Destroy(mObject);
    }
}