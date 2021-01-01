using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour {
    private BaseBoard<SingleGrid> map;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void Save(string filename) { }

    void Load(string filename) {
        BoardEntity boardEntity = BoardEntity.FromJson(
            Resources.Load<TextAsset>(filename).text
        );
        foreach (SingleMapGridEntity singleMapGridEntity in boardEntity.map) {
            map.Add(
                new Vector2Int(singleMapGridEntity.x, singleMapGridEntity.y),
                new SingleGrid(true)
            );
        }

        foreach (SingleSpecialEntity singleSpecialEntity in boardEntity.special) {
            SingleGrid singleGrid = map.GetData(
                new Vector2Int(singleSpecialEntity.x, singleSpecialEntity.y)
            );
            if (singleSpecialEntity.effect == "brokenBridge")
                singleGrid.SpecialEffect = SingleGrid.Effect.BrokenBridge;
            else if (singleSpecialEntity.effect == "doubleStep")
                singleGrid.SpecialEffect = SingleGrid.Effect.DoubleStep;
            else if (singleSpecialEntity.effect == "portal")
                singleGrid.SpecialEffect = SingleGrid.Effect.Portal;
            else
                singleGrid.SpecialEffect = SingleGrid.Effect.None;

            map.Add(
                new Vector2Int(singleSpecialEntity.x, singleSpecialEntity.y),
                new SingleGrid(true)
            );
        }

        foreach (SinglePortalEntity singlePortalEntity in boardEntity.portal) { }
    }
}