using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToRoque : MonoBehaviour {
    [SerializeField] private GameObject[] alphaRoque; // whiteShort, whiteLong, blackShort, blackLong
    [SerializeField] private GameObject[] roques; // whiteShort, whiteLong, blackShort, blackLong

    private GameObject[] alphaObjs = new GameObject[4];

    private static List<Vector2Int>[] roqueFromTo = {
        new List<Vector2Int>() { new Vector2Int(7, 0), new Vector2Int(5, 0), new Vector2Int(4, 0), new Vector2Int(6, 0) },      // 0
        new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(3, 0), new Vector2Int(4, 0), new Vector2Int(2, 0) },      // 1
        new List<Vector2Int>() { new Vector2Int(7, 7), new Vector2Int(5, 7), new Vector2Int(4, 7), new Vector2Int(6, 7) },      // 2
        new List<Vector2Int>() { new Vector2Int(0, 7), new Vector2Int(3, 7), new Vector2Int(4, 7), new Vector2Int(2, 7) }};     // 3
    private static List<Vector2Int> alphaRoqueGrids = new List<Vector2Int>() { roqueFromTo[0][3], roqueFromTo[1][3], roqueFromTo[2][3], roqueFromTo[3][3] };     // 3
    
    private bool[] roquesPossib = new bool[] { false, false, false, false };

    private void Awake() {
        disactivateAllRoques();

        for (int i = 0; i < alphaRoque.Length; i++) {
            alphaObjs[i] = Instantiate(alphaRoque[i], Geometry.PointFromGrid(alphaRoqueGrids[i]), Quaternion.identity);
            alphaObjs[i].SetActive(false);
        }

        this.enabled = false;
    }

    private void Update() {
        Ray rayToBoard = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToBoard, out RaycastHit hitPlace)) {
            for (int i = 0; i < roquesPossib.Length; i++) {
                if (roquesPossib[i]) {
                    if (hitPlace.collider.gameObject == roques[i]) {
                        setAllowedRoque(i);

                        if (Input.GetMouseButtonDown(0))
                            toRoque((i == 1 || i == 3));
                    } else {
                        setDefaultRoque(i);
                    }
                }
            }
        } else {
            for (int i = 0; i < roquesPossib.Length; i++) {
                if (roquesPossib[i])
                    setDefaultRoque(i);
            }
        }
    }

    public void toRoque(bool isLong) {
        StepSimpleSelection SSS = GetComponent<StepSimpleSelection>();
        SSS.Disactivate();

        int curPlayer = (GameManager.instance.currentPlayer.name == PlayerType.White ? 0 : 2) + (isLong ? 1 : 0);
        foreach (Layer curLayer in GameManager.layers) {
            if (curLayer.getRoqueStatus()[curPlayer]) {
                for (int i = 0; i < roqueFromTo[curPlayer].Count; i += 2)
                    curLayer.setFromTo(roqueFromTo[curPlayer][i], roqueFromTo[curPlayer][i + 1]);
            }
        }
        GameManager.instance.quantumNormalize();

        Disactivate();
    }

    public void TryActivate() {
        // [0]WhiteShort, [1]WhiteLong, [2]BlackShort, [3]BlackLong;
        bool[] roquesPieces = new bool[] { false, false, false, false };
        roquesPossib = new bool[] { false, false, false, false };

        for (int i = 0; i < GameManager.layers.Count; i++) {
            bool[] inCurLayer = GameManager.layers[i].getRoqueStatus();

            for (int j = 0; j < roquesPieces.Length; j++) {
                roquesPieces[j] = roquesPieces[j] || GameManager.layers[i].roques[j];
                roquesPossib[j] = roquesPossib[j] || inCurLayer[j];
            }
        }

        int playerIndex = (GameManager.instance.currentPlayer.name == PlayerType.White ? 0 : 2);

        for (int i = 0; i < 2; i++) {
            if (roquesPieces[playerIndex + i]) {
                if (roquesPossib[playerIndex + i])
                    setDefaultRoque(playerIndex + i);
                else
                    setDenyRoque(playerIndex + i);

                activateRoque(playerIndex + i, true);
            }
        }

        this.enabled = true;
    }

    public void Disactivate() {
        this.enabled = false;

        for (int i = 0; i < roquesPossib.Length; i++) {
            if (roquesPossib[i])
                setDefaultRoque(i);
        }
        disactivateAllRoques();

        StepAndBoardDisplay goTo = GetComponent<StepAndBoardDisplay>();
        goTo.EnterState();
    }

    public void activateRoque(int index, bool status) {
        roques[index].SetActive(status);
    }

    public void disactivateAllRoques() {
        for (int i = 0; i < roques.Length; i++)
            roques[i].SetActive(false);
    }

    public void setAllowedRoque(int index) {
        alphaObjs[index].SetActive(true);

        MeshRenderer mesh = roques[index].GetComponentInChildren<MeshRenderer>();
        mesh.material = PrefabIndexing.instance.allowedTile;
    }

    public void setDenyRoque(int index) {
        MeshRenderer mesh = roques[index].GetComponentInChildren<MeshRenderer>();
        mesh.material = PrefabIndexing.instance.enemyTile;
    }

    public void setDefaultRoque(int index) {
        alphaObjs[index].SetActive(false);

        MeshRenderer mesh = roques[index].GetComponentInChildren<MeshRenderer>();
        mesh.material = PrefabIndexing.instance.unselectRoque;
    }
}
