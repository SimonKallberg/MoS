using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Point
{
    public Vector3[] pos;
    public Vector3[] vel;
    public Vector3[] acc;
}

public class Jelly : MonoBehaviour {

    public int size = 10;
    public float gravity = 0.2f;

    public float R = 1f; //Damepr
    public float M = 5f; //Mass
    public float K = 1f; //Spring
    public float distance = 1.0f;

    Point points;



    Mesh mesh;          //The mesh.

    MeshFilter mf;
    MeshRenderer mr;
    public Material mat;

    // Use this for initialization
    void Start () {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();

        mesh = new Mesh();
        mf.mesh = mesh;
        mr.material = mat;

        placePos();

        calcTriangles();
    }

	// Update is called once per frame
	void Update () {
		// Ändrar på points värden enligt vårt sysstem.
	}

    void placePos()
    {
		//Allocate size for each vertex point

		points.pos = new Vector3[size * size * size];
		points.vel = new Vector3[size * size * size];
		points.acc = new Vector3[size * size * size];

		//Loop through all verticies and give them its value.
		for (int y = 0; y < size; y++) {

			for (int z = 0; z < size; z++) {

				for (int x = 0; x < size; x++) {
					
					points.pos[x + y * size * size + z * size] = new Vector3 (x, y, z);
					points.vel[x + y * size * size + z * size] = new Vector3 (0, 0, 0);
					points.acc[x + y * size * size + z * size] = new Vector3 (0, 0, 0);
				}
			}
		}

		mesh.vertices = points.pos;

    }


    void calcTriangles()
    {
        int noTriang = (size - 1) * (size - 1) * 2 * 6;  //Amount of triangles that is needed

        int[] triangles = new int[noTriang*3];    // Triangle-array for the mesh

        int triCounter = 0; // Counter used for keeping track of the array

        // Implement all triangles for the mesh
        for(int z = 0; z<size-1; z++)   //Bottom layer of the cube, i.e. y = 0
        {
            for (int x = 0; x < size-1; x++)
            {
               int currentPoint = size * z + x;  //Cornerpoint of the square to start calculating the triangles
                triangles[triCounter + 0] = currentPoint;
                triangles[triCounter + 1] = currentPoint + size + 1;
                triangles[triCounter + 2] = currentPoint + size;
                
                triangles[triCounter + 3] = currentPoint;
                triangles[triCounter + 4] = currentPoint + 1;
                triangles[triCounter + 5] = currentPoint + size + 1;
                triCounter += 6;
            }
        }
        for (int z = 0; z < size - 1; z++)      // Top layer of the cube, y = size - 1
        {
            for (int x = 0; x < size - 1; x++)
            {

                int currentPoint = (size*size*(size - 1)) + size * z + x;
                triangles[triCounter + 0] = currentPoint;
                triangles[triCounter + 1] = currentPoint + size;
                triangles[triCounter + 2] = currentPoint + size + 1;

                triangles[triCounter + 3] = currentPoint;
                triangles[triCounter + 4] = currentPoint + size + 1;
                triangles[triCounter + 5] = currentPoint + 1;
                triCounter += 6;
            }
        }

        for (int y = 0; y < size - 1; y++)          // Side plane, x = 0
        {
            for (int z = 0; z < size - 1; z++)
            {

                int currentPoint = size * z + size*size*y; 
                triangles[triCounter + 0] = currentPoint;
                triangles[triCounter + 1] = currentPoint + size;
                triangles[triCounter + 2] = currentPoint + size + size*size;

                triangles[triCounter + 3] = currentPoint;
                triangles[triCounter + 4] = currentPoint + size + size * size;
                triangles[triCounter + 5] = currentPoint + size*size;
                triCounter += 6;
            }
        }

        for (int y = 0; y < size - 1; y++)      // Side plane, x = size -1 
        {
            for (int z = 0; z < size - 1; z++)
            {

                int currentPoint =  (size - 1) + size * z + size * size * y; 
                triangles[triCounter + 0] = currentPoint;
                triangles[triCounter + 1] = currentPoint + size + size * size;
                triangles[triCounter + 2] = currentPoint + size ;

                triangles[triCounter + 3] = currentPoint;
                triangles[triCounter + 4] = currentPoint + size * size;
                triangles[triCounter + 5] = currentPoint + size +  size * size;
                triCounter += 6;
            }
        }

        for (int y = 0; y < size - 1; y++)      // Side plane, z = 0 
        {
            for (int x = 0; x < size - 1; x++)
            {

                int currentPoint = x + size * size * y; 
                triangles[triCounter + 0] = currentPoint;
                triangles[triCounter + 1] = currentPoint + size * size;
                triangles[triCounter + 2] = currentPoint + size * size + 1;

                triangles[triCounter + 3] = currentPoint;
                triangles[triCounter + 4] = currentPoint + size * size + 1;
                triangles[triCounter + 5] = currentPoint + 1;
                triCounter += 6;
            }
        }

        for (int y = 0; y < size - 1; y++)      // Side plane, z = size - 1
        {
            for (int x = 0; x < size - 1; x++)
            {

                int currentPoint = size*(size-1) + x + size * size * y; 
                triangles[triCounter + 0] = currentPoint;
                triangles[triCounter + 1] = currentPoint + size * size + 1;
                triangles[triCounter + 2] = currentPoint + size * size;

                triangles[triCounter + 3] = currentPoint;
                triangles[triCounter + 4] = currentPoint + 1;
                triangles[triCounter + 5] = currentPoint + size * size + 1;
                triCounter += 6;
            }
        }
        mesh.triangles = triangles;     // Assign meshes triangles
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
