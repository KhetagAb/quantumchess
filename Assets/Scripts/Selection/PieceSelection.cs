using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelection : StepAndPieceSelection {
    private void Awake() {
        this.enabled = false;
    }

    private void Update() {
        if (Input.GetMouseButton(1))
            return;

        Ray rayToBoard = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToBoard, out RaycastHit hitPlace) && isCorrectHit(hitPlace)) {
            Vector2Int gridPoint = getGridFromHit(hitPlace);
            Display.instance.setSelector(gridPoint);

            if (Input.GetMouseButtonDown(0)) {
                if (isFriendlyPieceAtGrid(gridPoint))
                    Exit(gridPoint);
            }
        } else {
            Display.instance.setSelector(null);
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

        SimpleSelection step = GetComponent<SimpleSelection>();
        step.Activate(gridPoint);
    }
}
