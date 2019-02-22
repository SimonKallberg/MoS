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
        placePos();
        //Fixar trianglarna.
        //calcTriangles();
    }

	// Update is called once per frame
	void Update () {
		//Ändrar på points värden enligt vårt sysstem.
	}

    void placePos()
    {
		//Allocate a vertex point each mass.

		points.pos = new Vector3[size * size * size];
		points.vel = new Vector3[size * size * size];
		points.acc = new Vector3[size * size * size];

		//Loopar igenom alla vecticies och ger dem dess värden.
		for (int y = 0; y < size; y++) {

			for (int z = 0; z < size; z++) {

				for (int x = 0; x < size; x++) {

					pos [x + y * size * size + z * size] = new Vector3 (x, y, z);
					vel [x + y * size * size + z * size] = new Vector3 (0, 0, 0);
					acc [x + y * size * size + z * size] = new Vector3 (0, 0, 0);
				}
			}
		}

		mesh.vertices = pos;

    }


    void calcTriangles()
    {








    }



    void UpdateVertPos3D() //3D!!!
    {

        float diag = Mathf.Sqrt(dist * dist + dist * dist);
        float h = 0.1f;

        //Calculate acceleration for all messes.
        int active; //stores the position which will be connected.

        //Goes through all point which is'nt at the edges.

        for (int y = 1; y < size - 1; y++)
        {
          for (int z = 1; z < size - 1; z++)
          {
              for (int x = 1; x < size - 1; x++)
              {

                  active = x + z * size + y * size * size;
                  acc[active] = spring_damper3D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1])
                              + spring_damper3D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1])
                              + spring_damper3D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size])
                              + spring_damper3D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size])
                              + spring_damper3D(dist, vertices[active], vertices[active - size - 1], vel[active], vel[active - size - 1])
                              + spring_damper3D(dist, vertices[active], vertices[active - size + 1], vel[active], vel[active - size + 1])
                              + spring_damper3D(dist, vertices[active], vertices[active + size - 1], vel[active], vel[active + size - 1])
                              + spring_damper3D(dist, vertices[active], vertices[active + size + 1], vel[active], vel[active + size + 1])
                              + spring_damper3D(dist, vertices[active], vertices[active - size*size], vel[active], vel[active + size*size])
                              + spring_damper3D(dist, vertices[active], vertices[active + size*size], vel[active], vel[active - size*size])
                              + spring_damper3D(dist, vertices[active], vertices[active - size*size - size - 1], vel[active], vel[active - size*size - size - 1])
                              + spring_damper3D(dist, vertices[active], vertices[active - size*size - size], vel[active], vel[active - size*size - size])
                              + spring_damper3D(dist, vertices[active], vertices[active - size*size - size + 1], vel[active], vel[active - size*size - size + 1])
                              + spring_damper3D(dist, vertices[active], vertices[active - size*size - 1], vel[active], vel[active - size*size - 1])
                              + spring_damper3D(dist, vertices[active], vertices[active - size*size + 1], vel[active], vel[active - size*size + 1])
                              + spring_damper3D(dist, vertices[active], vertices[active - size*size + size - 1], vel[active], vel[active - size*size + size - 1])
                              + spring_damper3D(dist, vertices[active], vertices[active - size*size + size], vel[active], vel[active - size*size + size])
                              + spring_damper3D(dist, vertices[active], vertices[active - size*size + size + 1], vel[active], vel[active - size*size + size + 1])
                              + spring_damper3D(dist, vertices[active], vertices[active + size*size - size - 1], vel[active], vel[active + size*size - size - 1])
                              + spring_damper3D(dist, vertices[active], vertices[active + size*size - size], vel[active], vel[active + size*size - size])
                              + spring_damper3D(dist, vertices[active], vertices[active + size*size - size + 1], vel[active], vel[active + size*size - size + 1])
                              + spring_damper3D(dist, vertices[active], vertices[active + size*size - 1], vel[active], vel[active + size*size - 1])
                              + spring_damper3D(dist, vertices[active], vertices[active + size*size + 1], vel[active], vel[active + size*size + 1])
                              + spring_damper3D(dist, vertices[active], vertices[active + size*size + size - 1], vel[active], vel[active + size*size + size - 1])
                              + spring_damper3D(dist, vertices[active], vertices[active + size*size + size], vel[active], vel[active + size*size + size])
                              + spring_damper3D(dist, vertices[active], vertices[active + size*size + size + 1], vel[active], vel[active + size*size + size + 1])
                              - new Vector3( 0,gravity / M,0);
              }
          }
        }

        //position (0,0,0)
        active = 0;
        acc[active] = spring_damper3D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1]) //the connection right
               + spring_damper3D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size])
               + spring_damper3D(dist, vertices[active], vertices[active + size + 1], vel[active], vel[active + size + 1])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size], vel[active], vel[active + size*size])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size + 1], vel[active], vel[active + size*size + 1])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size + size], vel[active], vel[active + size*size + size])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size + size + 1], vel[active], vel[active + size*size + size + 1])
               - new Vector3(0, gravity / M, 0);  //the connection up

        //position (size,0,0)
        active = size - 1;
        acc[active] = spring_damper3D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1]) //the connection left
               + spring_damper3D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size])
               + spring_damper3D(dist, vertices[active], vertices[active + size - 1], vel[active], vel[active + size - 1])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size], vel[active], vel[active + size*size])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size - 1], vel[active], vel[active + size*size - 1])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size + size - 1], vel[active], vel[active + size*size + size - 1])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size + size], vel[active], vel[active + size*size + size])
               - new Vector3(0, gravity / M, 0);  //the connection up

        //position (0,size,0)
        active = size * size - size;
        acc[active] = spring_damper3D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1]) //the connection right
               + spring_damper3D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size])
               + spring_damper3D(dist, vertices[active], vertices[active - size + 1], vel[active], vel[active - size + 1])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size], vel[active], vel[active + size*size])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size - size], vel[active], vel[active + size*size - size])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size - size + 1], vel[active], vel[active + size*size - size + 1])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size + 1], vel[active], vel[active + size*size + 1])
               - new Vector3(0, gravity / M, 0);  //the connection down

        //position (size,size,0)
        active = size * size - 1;
        acc[active] = spring_damper3D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1]) //the connection left
               + spring_damper3D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size])
               + spring_damper3D(dist, vertices[active], vertices[active - size - 1], vel[active], vel[active - size - 1])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size], vel[active], vel[active + size*size])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size - size - 1], vel[active], vel[active + size*size - size - 1])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size - size], vel[active], vel[active + size*size - size])
               + spring_damper3D(dist, vertices[active], vertices[active + size*size - 1], vel[active], vel[active + size*size - 1])  
               - new Vector3(0, gravity / M, 0);  //the connection down


        //the edges
        for (int i = 1; i < size - 1; i++)
        {
            active = i;
            acc[active] = spring_damper3D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size]) - new Vector3(0, gravity / M, 0); //bottom edge.

            active = i * size;
            acc[active] = spring_damper3D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1]) - new Vector3(0, gravity / M, 0); //left edge.

            active = i * size + size - 1;
            acc[active] = spring_damper3D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1]) - new Vector3(0, gravity / M, 0); //right edge.


            active = size * size - 1 - i;
            acc[active] = spring_damper3D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size]) - new Vector3(0, gravity / M, 0); //top edge.
        }


        // calculate the velocity and positions for all masses.
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                active = x + y * size;
                vel[active] = vel[active] + h * acc[active];
                vertices[active] = vertices[active] + h * vel[active];
            }

        }

        mesh.vertices = vertices;
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
