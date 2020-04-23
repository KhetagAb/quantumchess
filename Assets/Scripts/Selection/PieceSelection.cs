using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelection : Selection {
    private void Awake() {
        this.enabled = false;

        selectTile = Instantiate(Prefabs.instance.selectTile);
        hideObj(selectTile);
    }

    private void Update() {
        if (Input.GetMouseButton(1))
            return;

        Ray rayToBoard = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToBoard, out RaycastHit hitPlace) && isCorrectHit(hitPlace)) {
            Vector2Int gridPoint = getGridFromHit(hitPlace);
            showObjOnGrid(selectTile, gridPoint);

            if (Input.GetMouseButtonDown(0)) {
                if (isFriendlyPieceAtGrid(gridPoint))
                    Exit(gridPoint);
            }
        } else {
            hideObj(selectTile);
        }
    }

    public void Activate() {
        this.enabled = true;
    }

    private void Disactivate() {
        this.enabled = false;
    }

    private void Exit(Vector2Int gridPoint) {
        Disactivate();

        hideObj(selectTile);

        StepSimpleSelection step = GetComponent<StepSimpleSelection>();
        step.Activate(gridPoint);
    }
}
