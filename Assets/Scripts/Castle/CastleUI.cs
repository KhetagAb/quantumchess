using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CastleUI : CastleArrow, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public void OnPointerClick(PointerEventData eventData) {
        if (!isDeny[index]) {
            GetComponentInParent<Castling>().Castle(index);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isDeny[index]) {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            mesh.material = Prefabs.instance.allowCastle;

            showAlphaAndTile();
        }
    }
    public void OnPointerExit(PointerEventData eventData) {
        if (!isDeny[index]) {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            mesh.material = Prefabs.instance.defaultCastle;

            hideAlphaAndTile();
        }
    }
}
