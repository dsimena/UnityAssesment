using UnityEngine;
public class NewBehaviourScript : MonoBehaviour
{
    // Reference to the Prefabm. Drag a Prefab into this field in the Inspector.
    public GameObject _myPrefab1;
    public GameObject _myPrefab2;
    public Material source;
    public Color color;
    private Material cubesMaterial;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        // apply the same mnaterial to the both of the GameObject
        string path = "Prefabs/MyPrefab1";
        _myPrefab1 = Resources.Load(path) as GameObject;

        path = "Prefabs/MyPrefab2";
        _myPrefab2 = Resources.Load(path) as GameObject;

        path = "Materials/cubesMaterial";
        source = (Material)Resources.Load(path);

        _myPrefab1.GetComponent<MeshRenderer>().material = source;
        _myPrefab2.GetComponent<MeshRenderer>().material = source;


        // Instantiate at position (0, 0, 0) and zero rotation.
        var mp1  = Instantiate(_myPrefab1, new Vector3(0, 0, 0), Quaternion.identity); 
        var mp2  = Instantiate(_myPrefab2, new Vector3(-5, 0, 0), Quaternion.identity); 
    }

    void Update(){
        //color = new Color(255, 10, 56, 1);
        _myPrefab1.GetComponent<MeshRenderer>().sharedMaterial.color = color;

    }
}
