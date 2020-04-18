using System.Collections.Generic;
using UnityEngine;

public class StepQuantumSelection : StepSelection {
    private GameObject midTile;
    private Vector2Int? midGridPoint;

    private GameObject alphaMidPiece;

    private void Awake() {
        this.enabled = false;

        selectTile = Instantiate(PrefabIndexing.instance.prefSelectTile);
        hideObj(selectTile);
        midTile = Instantiate(PrefabIndexing.instance.prefMidTile);
        hideObj(midTile);
    }

    private void Update() {
        if (Input.GetMouseButton(1))
            return;

        Ray rayToBoard = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToBoard, out RaycastHit hitPlace) && isCorrectHit(hitPlace)) {
            Vector2Int gridPoint = getGridFromHit(hitPlace);
            showObjOnGrid(selectTile, gridPoint);

            updateAlphaStatus(gridPoint, midGridPoint); // хочеца заменить

            if (midGridPoint != null) {
                if (Input.GetMouseButtonDown(0)) {
                    if (allowedGrids.Contains(gridPoint))
                        ExitToStep(gridPoint);
                } else if (Input.GetMouseButtonDown(2)) {
                    deselectMidTile();
                }
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    if (allowedGrids.Contains(gridPoint))
                        selectMidTile(gridPoint);
                } else if (Input.GetMouseButtonDown(2)) {
                    Cancel();
                }
            }
        } else {
            showAllTiles(midGridPoint);
            hideObj(selectTile);
        }
    }

    private void selectMidTile(Vector2Int gridPoint) {
        midGridPoint = gridPoint;

        showObjOnGrid(midTile, (Vector2Int) midGridPoint);
        showObjOnGrid(alphaMidPiece, (Vector2Int) midGridPoint);

        hideAllowedGrids();
        showAllowedGrids(startGridPoint, midGridPoint, true);

        // Не очень нравится
        setActiveTileInGrid((Vector2Int) midGridPoint, false);
    }

    private void deselectMidTile() {
        midGridPoint = null;

        hideObj(midTile);
        hideObj(alphaMidPiece);

        hideAllowedGrids();
        showAllowedGrids(startGridPoint, midGridPoint, true);
    }

    public void Activate(Vector2Int gridPoint) {
        ActiveSelection(gridPoint, true);

        midGridPoint = null;
        alphaMidPiece = Instantiate(PrefabIndexing.getPrefabAlphaByID((int) getPieceIDAtGrid(gridPoint)));
        hideObj(alphaMidPiece);

        GameManager.instance.selectQuantumPieceAtGrid(startGridPoint);
    }

    public void Disactivate() {
        if (!this.enabled)
            return;

        Destroy(alphaMidPiece);
        alphaMidPiece = null;
        hideObj(midTile);

        DisactiveSelection();
    }

    private void ExitToStep(Vector2Int gridPoint) {
        Disactivate();

        GameManager.instance.quantumMove(startGridPoint, (Vector2Int) midGridPoint, gridPoint);

        StepAndBoardDisplay goTo = GetComponent<StepAndBoardDisplay>();
        goTo.Activate(startGridPoint, gridPoint, true);
    }

    private void Cancel() {
        Disactivate();

        StepSimpleSelection goTo = GetComponent<StepSimpleSelection>();
        goTo.Activate(startGridPoint);
    }
}