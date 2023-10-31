
using UnityEngine;

[CreateAssetMenu(fileName = "New BlockData", menuName = "Pi/Block Data")]
public class BlockData : ScriptableObject
{

    [SerializeField]
    public BlockRotateParams blockRotateParams;


    


}

[System.Serializable]
public class BlockRotateParams
{
    public Vector3 rotateAxis;
    public float rotateAngle;
    public float rotateSpeed;

    
    public BlockRotateParams()
    {

        rotateSpeed = 0.6f;
        rotateAxis = new Vector3(0.5f, 1, 0).normalized;
        rotateAngle = 90;

    }
    
}
