using UnityEngine;

[System.Serializable]
public class SerializableVector3
{
    private float[] position = new float[3];

    public SerializableVector3(Vector3 newPos)
    {
        position[0] = newPos.x;
        position[1] = newPos.y;   
        position[2] = newPos.z;

    }

    public Vector3 ToVector()
    {
        return new Vector3(position[0], position[1], position[2]);
    }

    public void UpdateVector(Vector3 newPos)
    {
        position[0] = newPos.x;
        position[1] = newPos.y;
        position[2] = newPos.z;
    }

}
