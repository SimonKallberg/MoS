using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct Point
{
    Vector3 pos;
    Vector3 vel;
    Vector3 acc;
}


public class Jelly : MonoBehaviour {

    public int size = 10;
    public float gravity = 0.2f;

    public float R = 1f; //Damepr
    public float M = 5f; //Mass
    public float K = 1f; //Spring
    public float distance = 1.0f;

    Point points;

    int[] triangles;    //triangles for the mesh

    Mesh mesh;          //The mesh.

    MeshFilter mf;      
    MeshRenderer mr;    
    public Material mat;

    // Use this for initialization
    void Start () {
        //Initierar alla variabler.
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();

        mesh = new Mesh();
        mf.mesh = mesh;
        mr.material = mat;
        //Plaserar pos på rätt ställe så det blir en kub.

        //Fixar trianglarna.


    }
	
	// Update is called once per frame
	void Update () {
		//Ändrar på points värden enligt vårt sysstem.
	}

    
    public Vector3 spring_damper(float distance, Vector3 pos_from, Vector3 pos_to, Vector3 v_from, Vector3 v_to)
    {

        Vector3 direction = pos_to - pos_from;
        Vector3 velocity = v_to - v_from;

        //ACCELERATION FROM SPRING.
        //Absolute value from direction.
        float abs = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y + direction.z * direction.z);
        //Normalized direction vector.
        Vector3 normalized_vector;
        normalized_vector.x = direction.x / abs;
        normalized_vector.y = direction.y / abs;
        normalized_vector.z = direction.z / abs;
        //Adds the distance to our direction vector.
        Vector3 desired_distance;
        desired_distance.x = distance * normalized_vector.x;
        desired_distance.y = distance * normalized_vector.y;
        desired_distance.z = distance * normalized_vector.z;
        //Adds spring constant to the length difference for uor spring.
        Vector3 acc_spring = K * (direction - desired_distance);

        //ACCELERATION FROM DAMPER.
        Vector3 acc_damper = R * velocity;

        //FINAL ACCELERATION.
        return (acc_spring + acc_damper);
    }
}
