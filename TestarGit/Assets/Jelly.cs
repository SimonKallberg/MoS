﻿using System.Collections;
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
    public float gravity = 0.02f;

    public float R = 0f; //Damepr
    public float M = 5f; //Mass
    public float K = 0.2f; //Spring
    public float dist = 1.76f;

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
	void FixedUpdate () {
        // Ändrar på points värden enligt vårt sysstem.
        UpdateVertPos3D();
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

					points.pos[x + y * size * size + z * size] = new Vector3 (x*dist, y* dist, z* dist);
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

    void touchFloor(int fall_length)
    {
        for (int i = 0; i < size * size * size; i++)
        {
            if (points.pos[i].y < -fall_length)
            {
                points.pos[i].y = -fall_length;
                points.vel[i] = new Vector3(0, 0, 0);
                points.acc[i] = new Vector3(0, 0, 0);
            }
        }
    }



    void UpdateVertPos3D() //3D!!!
    {

        float diag = Mathf.Sqrt(dist * dist + dist * dist);
        float longDiag = Mathf.Sqrt(dist * dist + dist * dist + dist*dist);
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
                  points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1])
                              + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1])
                              + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size])
                              + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size])
                              + spring_damper3D(diag, points.pos[active], points.pos[active - size - 1], points.vel[active], points.vel[active - size - 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active - size + 1], points.vel[active], points.vel[active - size + 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active + size - 1], points.vel[active], points.vel[active + size - 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active + size + 1], points.vel[active], points.vel[active + size + 1])
                              + spring_damper3D(dist, points.pos[active], points.pos[active - size*size], points.vel[active], points.vel[active + size*size])
                              + spring_damper3D(dist, points.pos[active], points.pos[active + size*size], points.vel[active], points.vel[active - size*size])
                              + spring_damper3D(longDiag, points.pos[active], points.pos[active - size*size - size - 1], points.vel[active], points.vel[active - size*size - size - 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active - size*size - size], points.vel[active], points.vel[active - size*size - size])
                              + spring_damper3D(longDiag, points.pos[active], points.pos[active - size*size - size + 1], points.vel[active], points.vel[active - size*size - size + 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active - size*size - 1], points.vel[active], points.vel[active - size*size - 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active - size*size + 1], points.vel[active], points.vel[active - size*size + 1])
                              + spring_damper3D(longDiag, points.pos[active], points.pos[active - size*size + size - 1], points.vel[active], points.vel[active - size*size + size - 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active - size*size + size], points.vel[active], points.vel[active - size*size + size])
                              + spring_damper3D(longDiag, points.pos[active], points.pos[active - size*size + size + 1], points.vel[active], points.vel[active - size*size + size + 1])
                              + spring_damper3D(longDiag, points.pos[active], points.pos[active + size*size - size - 1], points.vel[active], points.vel[active + size*size - size - 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active + size*size - size], points.vel[active], points.vel[active + size*size - size])
                              + spring_damper3D(longDiag, points.pos[active], points.pos[active + size*size - size + 1], points.vel[active], points.vel[active + size*size - size + 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active + size*size - 1], points.vel[active], points.vel[active + size*size - 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active + size*size + 1], points.vel[active], points.vel[active + size*size + 1])
                              + spring_damper3D(longDiag, points.pos[active], points.pos[active + size*size + size - 1], points.vel[active], points.vel[active + size*size + size - 1])
                              + spring_damper3D(diag, points.pos[active], points.pos[active + size*size + size], points.vel[active], points.vel[active + size*size + size])
                              + spring_damper3D(longDiag, points.pos[active], points.pos[active + size*size + size + 1], points.vel[active], points.vel[active + size*size + size + 1])
                              - new Vector3( 0,gravity / M,0);
              }
          }
        }

        //position (0,0,0)
        active = 0;
        points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) //the connection right
               + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size + 1], points.vel[active], points.vel[active + size + 1])
               + spring_damper3D(dist, points.pos[active], points.pos[active + size*size], points.vel[active], points.vel[active + size*size])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size + 1], points.vel[active], points.vel[active + size*size + 1])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size + size], points.vel[active], points.vel[active + size*size + size])
               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size*size + size + 1], points.vel[active], points.vel[active + size*size + size + 1])
               - new Vector3(0, gravity / M, 0);  //the connection up

        //position (size,0,0)
        active = size - 1;
        points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) //the connection left
               + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size - 1], points.vel[active], points.vel[active + size - 1])
               + spring_damper3D(dist, points.pos[active], points.pos[active + size*size], points.vel[active], points.vel[active + size*size])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size - 1], points.vel[active], points.vel[active + size*size - 1])
               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size*size + size - 1], points.vel[active], points.vel[active + size*size + size - 1])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size + size], points.vel[active], points.vel[active + size*size + size])
               - new Vector3(0, gravity / M, 0);  //the connection up

        //position (0,size,0)
        active = size * size - size;
        points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) //the connection right
               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size])
               + spring_damper3D(diag, points.pos[active], points.pos[active - size + 1], points.vel[active], points.vel[active - size + 1])
               + spring_damper3D(dist, points.pos[active], points.pos[active + size*size], points.vel[active], points.vel[active + size*size])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size - size], points.vel[active], points.vel[active + size*size - size])
               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size*size - size + 1], points.vel[active], points.vel[active + size*size - size + 1])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size + 1], points.vel[active], points.vel[active + size*size + 1])
               - new Vector3(0, gravity / M, 0);  //the connection down

        //position (size,size,0)
        active = size * size - 1;
        points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) //the connection left
               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size])
               + spring_damper3D(diag, points.pos[active], points.pos[active - size - 1], points.vel[active], points.vel[active - size - 1])
               + spring_damper3D(dist, points.pos[active], points.pos[active + size*size], points.vel[active], points.vel[active + size*size])
               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size*size - size - 1], points.vel[active], points.vel[active + size*size - size - 1])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size - size], points.vel[active], points.vel[active + size*size - size])
               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size - 1], points.vel[active], points.vel[active + size*size - 1])
               - new Vector3(0, gravity / M, 0);  //the connection down

               //position (0,size,0)
               active = size*size*(size - 1);
               points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) //the connection right
                      + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size])
                      + spring_damper3D(diag, points.pos[active], points.pos[active + size + 1], points.vel[active], points.vel[active + size + 1])
                      + spring_damper3D(dist, points.pos[active], points.pos[active - size*size], points.vel[active], points.vel[active - size*size])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size*size + 1], points.vel[active], points.vel[active - size*size + 1])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size*size + size], points.vel[active], points.vel[active - size*size + size])
                      + spring_damper3D(longDiag, points.pos[active], points.pos[active - size*size + size + 1], points.vel[active], points.vel[active - size*size + size + 1])
                      - new Vector3(0, gravity / M, 0);  //the connection up

               //position (size,0,size)
               active = size*size*size - size*size + size - 1;
               points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) //the connection left
                      + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size])
                      + spring_damper3D(diag, points.pos[active], points.pos[active + size - 1], points.vel[active], points.vel[active + size - 1])
                      + spring_damper3D(dist, points.pos[active], points.pos[active - size*size], points.vel[active], points.vel[active - size*size])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size*size - 1], points.vel[active], points.vel[active - size*size - 1])
                      + spring_damper3D(longDiag, points.pos[active], points.pos[active - size*size + size - 1], points.vel[active], points.vel[active - size*size + size - 1])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size*size + size], points.vel[active], points.vel[active - size*size + size])
                      - new Vector3(0, gravity / M, 0);  //the connection up

               //position (0,size,size)
               active = size*size*size - size;
               points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) //the connection right
                      + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size + 1], points.vel[active], points.vel[active - size + 1])
                      + spring_damper3D(dist, points.pos[active], points.pos[active - size*size], points.vel[active], points.vel[active - size*size])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size*size - size], points.vel[active], points.vel[active - size*size - size])
                      + spring_damper3D(longDiag, points.pos[active], points.pos[active - size*size - size + 1], points.vel[active], points.vel[active - size*size - size + 1])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size*size + 1], points.vel[active], points.vel[active - size*size + 1])
                      - new Vector3(0, gravity / M, 0);  //the connection down

               //position (size,size,size)
               active = size*size*size - 1;
               points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) //the connection left
                      + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size - 1], points.vel[active], points.vel[active - size - 1])
                      + spring_damper3D(dist, points.pos[active], points.pos[active - size*size], points.vel[active], points.vel[active - size*size])
                      + spring_damper3D(longDiag, points.pos[active], points.pos[active - size*size - size - 1], points.vel[active], points.vel[active - size*size - size - 1])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size*size - size], points.vel[active], points.vel[active - size*size - size])
                      + spring_damper3D(diag, points.pos[active], points.pos[active - size*size - 1], points.vel[active], points.vel[active - size*size - 1])
                      - new Vector3(0, gravity / M, 0);  //the connection down


        // The edges
        for (int i = 1; i < size - 1; i++)
        {
            // The edge at x-axis where z=0 and y=0.
            active = i;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // x+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // x-1
                               + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size + 1], points.vel[active], points.vel[active + size + 1]) // z+1, x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size - 1], points.vel[active], points.vel[active + size - 1]) // z+1, x-1
                               + spring_damper3D(dist, points.pos[active], points.pos[active + size*size], points.vel[active], points.vel[active + size*size]) // y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size + 1], points.vel[active], points.vel[active + size*size + 1]) // y+1, x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size*size - 1], points.vel[active], points.vel[active + size*size - 1]) // y+1, x-1
                               + spring_damper3D(dist, points.pos[active], points.pos[active + size * size + size], points.vel[active], points.vel[active + size * size + size]) // y+1,z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size * size + size + 1], points.vel[active], points.vel[active + size * size + size + 1]) // y+1,z+1, x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size * size + size - 1], points.vel[active], points.vel[active + size * size + size - 1]) // y+1,z+1, x-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at x-axis where z=size and y=0.
            active = size*(size-1) + i;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // x+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // x-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // z-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size + 1], points.vel[active], points.vel[active - size + 1]) // z-size, x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size - 1], points.vel[active], points.vel[active - size - 1]) // z-size, x-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size * size], points.vel[active], points.vel[active + size * size]) // y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size * size + 1], points.vel[active], points.vel[active + size * size + 1]) // y+1, x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size * size - 1], points.vel[active], points.vel[active + size * size - 1]) // y+1, x-1

                               + spring_damper3D(diag, points.pos[active], points.pos[active + size * size - size], points.vel[active], points.vel[active + size * size - size]) // y+1,z-1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size * size - size + 1], points.vel[active], points.vel[active + size * size - size + 1]) // y+1,z-1, x+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size * size - size - 1], points.vel[active], points.vel[active + size * size - size - 1]) // y+1,z-1, x-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at x-axis where z=0 and y=size.
            active = size * size * (size - 1) + i;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // x+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // x-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size + 1], points.vel[active], points.vel[active + size + 1]) // z+1, x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size - 1], points.vel[active], points.vel[active + size - 1]) // z+1, x-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size + 0], points.vel[active], points.vel[active - size * size + 0]) // y-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size * size + 1], points.vel[active], points.vel[active - size * size + 1]) // y-1, x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size * size - 1], points.vel[active], points.vel[active - size * size - 1]) // y-1, x-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size + size + 0], points.vel[active], points.vel[active - size * size + size + 0]) // y-1,z-1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size * size + size + 1], points.vel[active], points.vel[active - size * size + size + 1]) // y-1,z+1, x+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size * size + size - 1], points.vel[active], points.vel[active - size * size + size - 1]) // y-1,z+1, x-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at x-axis where z=size and y=size.
            active = size * size * size - size + i;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // x+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // x-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // z-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size + 1], points.vel[active], points.vel[active - size + 1]) // z-1, x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size - 1], points.vel[active], points.vel[active - size - 1]) // z-1, x-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size], points.vel[active], points.vel[active - size * size]) // y-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size * size + 1], points.vel[active], points.vel[active - size * size + 1]) // y-1, x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size * size - 1], points.vel[active], points.vel[active - size * size - 1]) // y-1, x-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size - size], points.vel[active], points.vel[active - size * size - size]) // y-1,z-1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size * size - size + 1], points.vel[active], points.vel[active - size * size - size + 1]) // y-1,z-1, x+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size * size - size - 1], points.vel[active], points.vel[active - size * size - size - 1]) // y-1,z-1, x-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at z-axis where x=0 and y=0.
            active = size * i;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // z+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size], points.vel[active], points.vel[active + 1 + size]) // x+1, z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size], points.vel[active], points.vel[active + 1 - size]) // x+1, z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size * size + 0], points.vel[active], points.vel[active + size * size + 0]) // y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size * size + size], points.vel[active], points.vel[active + size * size + size]) // y+1, z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size * size - size], points.vel[active], points.vel[active + size * size - size]) // y+1, z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size * size + 1], points.vel[active], points.vel[active + size * size + 1]) // y+1,x+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size * size + 1 + size], points.vel[active], points.vel[active + size * size + 1 + size]) // y+1,x+1, z+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size * size + 1 - size], points.vel[active], points.vel[active + size * size + 1 - size]) // y+1,x+1, z-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at z-axis where x=size and y=0.
            active = (size-1) + size * i;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // z+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // x-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size], points.vel[active], points.vel[active - 1 + size]) // x-1, z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size], points.vel[active], points.vel[active - 1 - size]) // x-1, z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size * size + 0], points.vel[active], points.vel[active + size * size + 0]) // y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size * size + size], points.vel[active], points.vel[active + size * size + size]) // y+1, z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size * size - size], points.vel[active], points.vel[active + size * size - size]) // y+1, z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size * size - 1], points.vel[active], points.vel[active + size * size - 1]) // y+1,x-1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size * size - 1 + size], points.vel[active], points.vel[active + size * size - 1 + size]) // y+1,x-1, z+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size * size - 1 - size], points.vel[active], points.vel[active + size * size - 1 - size]) // y+1,x-1, z-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at z-axis where x=0 and y=size.
            active = size*size*(size-1) + size * i;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // z+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size], points.vel[active], points.vel[active + 1 + size]) // x+1, z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size], points.vel[active], points.vel[active + 1 - size]) // x+1, z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size + 0], points.vel[active], points.vel[active - size * size + 0]) // y-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size * size + size], points.vel[active], points.vel[active - size * size + size]) // y-1, z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size * size - size], points.vel[active], points.vel[active - size * size - size]) // y-1, z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size + 1], points.vel[active], points.vel[active - size * size + 1]) // y-1,x+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size * size + 1 + size], points.vel[active], points.vel[active - size * size + 1 + size]) // y-1,x+1, z+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size * size + 1 - size], points.vel[active], points.vel[active - size * size + 1 - size]) // y-1,x+1, z-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at z-axis where x=size and y=size.
            active = size * size * (size - 1) + (size-1) + size * i;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // z+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // x-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size], points.vel[active], points.vel[active - 1 + size]) // x-1, z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size], points.vel[active], points.vel[active - 1 - size]) // x-1, z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size + 0], points.vel[active], points.vel[active - size * size + 0]) // y-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size * size + size], points.vel[active], points.vel[active - size * size + size]) // y-1, z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size * size - size], points.vel[active], points.vel[active - size * size - size]) // y-1, z-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size - 1], points.vel[active], points.vel[active - size * size - 1]) // y-1,x-1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size * size - 1 + size], points.vel[active], points.vel[active - size * size - 1 + size]) // y-1,x-1, z+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size * size - 1 - size], points.vel[active], points.vel[active - size * size - 1 - size]) // y-1,x-1, z-1
                               - new Vector3(0, gravity / M, 0); // Gravity.


            // The edge at y-axis where x=0 and z=0.
            active = size * size * i;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size*size], points.vel[active], points.vel[active + size*size]) // y+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - size*size], points.vel[active], points.vel[active - size*size]) // y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size + size*size], points.vel[active], points.vel[active + size + size*size]) // z+1, y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size - size*size], points.vel[active], points.vel[active + size - size*size]) // z+1, y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size * size], points.vel[active], points.vel[active + 1 + size * size]) // x+1, y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size * size], points.vel[active], points.vel[active + 1 - size * size]) // x+1, y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size + 1], points.vel[active], points.vel[active + size + 1]) // z+1,x+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size + 1 + size * size], points.vel[active], points.vel[active + size + 1 + size * size]) // z+1,x+1, y+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size + 1 - size * size], points.vel[active], points.vel[active + size + 1 + size * size]) // z+1,x+1, y-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at y-axis where x=size and z=0.
            active = size * size * i  + (size-1);
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size * size], points.vel[active], points.vel[active + size * size]) // y+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size], points.vel[active], points.vel[active - size * size]) // y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // z+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size + size * size], points.vel[active], points.vel[active + size + size * size]) // z+1, y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + size - size * size], points.vel[active], points.vel[active + size - size * size]) // z+1, y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // x-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size * size], points.vel[active], points.vel[active - 1 + size * size]) // x-1, y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size * size], points.vel[active], points.vel[active - 1 - size * size]) // x-1, y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + size - 1], points.vel[active], points.vel[active + size - 1]) // z+1,x-1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size - 1 + size * size], points.vel[active], points.vel[active + size - 1 + size * size]) // z+1,x-1, y+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active + size - 1 - size * size], points.vel[active], points.vel[active + size - 1 + size * size]) // z+1,x-1, y-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at y-axis where x=size and z=size.
            active = (size * size) * (i+1) -1;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size * size], points.vel[active], points.vel[active + size * size]) // y+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size], points.vel[active], points.vel[active - size * size]) // y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // z-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size + size * size], points.vel[active], points.vel[active - size + size * size]) // z-1, y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size - size * size], points.vel[active], points.vel[active - size - size * size]) // z-1, y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // x-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size * size], points.vel[active], points.vel[active - 1 + size * size]) // x-1, y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size * size], points.vel[active], points.vel[active - 1 - size * size]) // x-1, y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size - 1], points.vel[active], points.vel[active - size - 1]) // z-1,x-1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size - 1 + size * size], points.vel[active], points.vel[active - size - 1 + size * size]) // z-1,x-1, y+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size - 1 - size * size], points.vel[active], points.vel[active - size - 1 + size * size]) // z-1,x-1, y-1
                               - new Vector3(0, gravity / M, 0); // Gravity.

            // The edge at y-axis where x=size and z=size.
            active = (size * size) * (i + 1) - size;
            points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size * size], points.vel[active], points.vel[active + size * size]) // y+1
                               + spring_damper3D(dist, points.pos[active], points.pos[active - size * size], points.vel[active], points.vel[active - size * size]) // y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // z-1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size + size * size], points.vel[active], points.vel[active - size + size * size]) // z-1, y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active - size - size * size], points.vel[active], points.vel[active - size - size * size]) // z-1, y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // x+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size * size], points.vel[active], points.vel[active + 1 + size * size]) // x+1, y+1
                               + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size * size], points.vel[active], points.vel[active + 1 - size * size]) // x+1, y-1

                               + spring_damper3D(dist, points.pos[active], points.pos[active - size + 1], points.vel[active], points.vel[active - size + 1]) // z-1,x+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size + 1 + size * size], points.vel[active], points.vel[active - size + 1 + size * size]) // z-1,x+1, y+1
                               + spring_damper3D(longDiag, points.pos[active], points.pos[active - size + 1 - size * size], points.vel[active], points.vel[active - size + 1 + size * size]) // z-1,x+1, y-1
                               - new Vector3(0, gravity / M, 0); // Gravity.




        }

        // The sides
        for (int i = 1; i < size - 1; i++)
        {
            for (int j = 1; j < size - 1; j++)
            {
                active = i + size*j;
                points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // i + 1
                                   + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // i - 1
                                   + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // j + 1
                                   + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // j - 1
                                   + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size], points.vel[active], points.vel[active - 1 - size]) //i-1,j-1
                                   + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size], points.vel[active], points.vel[active - 1 + size]) //i-1,j+1
                                   + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size], points.vel[active], points.vel[active + 1 - size]) //i+1,j-1
                                   + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size], points.vel[active], points.vel[active + 1 + size]) //i+1,j+1
                                   //one above
                                   + spring_damper3D(dist, points.pos[active], points.pos[size*size], points.vel[active], points.vel[size * size]) // The one above
                                   + spring_damper3D(diag, points.pos[active], points.pos[size * size + active + 1], points.vel[active], points.vel[size * size + active + 1]) // i + 1
                                   + spring_damper3D(diag, points.pos[active], points.pos[size * size + active - 1], points.vel[active], points.vel[size * size + active - 1]) // i - 1
                                   + spring_damper3D(diag, points.pos[active], points.pos[size * size + active + size], points.vel[active], points.vel[size * size + active + size]) // j + 1
                                   + spring_damper3D(diag, points.pos[active], points.pos[size * size + active - size], points.vel[active], points.vel[size * size + active - size]) // j - 1
                                   + spring_damper3D(longDiag, points.pos[active], points.pos[size * size + active - 1 - size], points.vel[active], points.vel[size * size + active - 1 - size]) //i-1,j-1
                                   + spring_damper3D(longDiag, points.pos[active], points.pos[size * size + active - 1 + size], points.vel[active], points.vel[size * size + active - 1 + size]) //i-1,j+1
                                   + spring_damper3D(longDiag, points.pos[active], points.pos[size * size + active + 1 - size], points.vel[active], points.vel[size * size + active + 1 - size]) //i+1,j-1
                                   + spring_damper3D(longDiag, points.pos[active], points.pos[size * size + active + 1 + size], points.vel[active], points.vel[size * size + active + 1 + size]) //i+1,j+1
                                   - new Vector3(0, gravity / M, 0);  // Gravity

                active = size * size * (size - 1) + i + size * j;
                points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // i + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // i - 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // j + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // j - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size], points.vel[active], points.vel[active - 1 - size]) //i-1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size], points.vel[active], points.vel[active - 1 + size]) //i-1,j+1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size], points.vel[active], points.vel[active + 1 - size]) //i+1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size], points.vel[active], points.vel[active + 1 + size]) //i+1,j+1

                                  + spring_damper3D(dist, points.pos[active], points.pos[active - (size * size)], points.vel[active], points.vel[active - (size * size)]) // The one below
                                  + spring_damper3D(diag, points.pos[active], points.pos[-size * size + active + 1], points.vel[active], points.vel[-size * size + active + 1]) // i + 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[-size * size + active - 1], points.vel[active], points.vel[-size * size + active - 1]) // i - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[-size * size + active + size], points.vel[active], points.vel[-size * size + active + size]) // j + 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[-size * size + active - size], points.vel[active], points.vel[-size * size + active - size]) // j - 1
                                  + spring_damper3D(longDiag, points.pos[active], points.pos[-size * size + active - 1 - size], points.vel[active], points.vel[-size * size + active - 1 - size]) //i-1,j-1
                                  + spring_damper3D(longDiag, points.pos[active], points.pos[-size * size + active - 1 + size], points.vel[active], points.vel[-size * size + active - 1 + size]) //i-1,j+1
                                  + spring_damper3D(longDiag, points.pos[active], points.pos[-size * size + active + 1 - size], points.vel[active], points.vel[-size * size + active + 1 - size]) //i+1,j-1
                                  + spring_damper3D(longDiag, points.pos[active], points.pos[-size * size + active + 1 + size], points.vel[active], points.vel[-size * size + active + 1 + size]) //i+1,j+1
                                  - new Vector3(0, gravity / M, 0);  // Gravity

                active = j * size + i * size * size;
                points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size * size], points.vel[active], points.vel[active + size * size]) // i + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size * size], points.vel[active], points.vel[active - size * size]) // i - 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // j + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // j - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - size * size - size], points.vel[active], points.vel[active - size * size - size]) //i-1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - size * size + size], points.vel[active], points.vel[active - size * size + size]) //i-1,j+1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + size * size - size], points.vel[active], points.vel[active + size * size - size]) //i+1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + size * size + size], points.vel[active], points.vel[active + size * size + size]) //i+1,j+1

                                  + spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + 1]) // one in
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size * size + 1], points.vel[active], points.vel[active + size * size + 1]) // i + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size * size + 1], points.vel[active], points.vel[active - size * size + 1]) // i - 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size + 1], points.vel[active], points.vel[active + size + 1]) // j + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size + 1], points.vel[active], points.vel[active - size + 1]) // j - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - size * size - size + 1], points.vel[active], points.vel[active - size * size - size + 1]) //i-1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - size * size + size + 1], points.vel[active], points.vel[active - size * size + size + 1]) //i-1,j+1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + size * size - size + 1], points.vel[active], points.vel[active + size * size - size + 1]) //i+1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + size * size + size + 1], points.vel[active], points.vel[active + size * size + size + 1]) //i+1,j+1
                                  - new Vector3(0, gravity / M, 0);  // Gravity

                active = j * size + i * size * size + (size-1);
                points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + size * size], points.vel[active], points.vel[active + size * size]) // i + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size * size], points.vel[active], points.vel[active - size * size]) // i - 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) // j + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) // j - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - size * size - size], points.vel[active], points.vel[active - size * size - size]) //i-1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - size * size + size], points.vel[active], points.vel[active - size * size + size]) //i-1,j+1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + size * size - size], points.vel[active], points.vel[active + size * size - size]) //i+1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + size * size + size], points.vel[active], points.vel[active + size * size + size]) //i+1,j+1

                                  + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - 1]) // one in
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size * size - 1], points.vel[active], points.vel[active + size * size - 1]) // i + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size * size - 1], points.vel[active], points.vel[active - size * size - 1]) // i - 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size - 1], points.vel[active], points.vel[active + size - 1]) // j + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size - 1], points.vel[active], points.vel[active - size - 1]) // j - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - size * size - size - 1], points.vel[active], points.vel[active - size * size - size - 1]) //i-1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - size * size + size - 1], points.vel[active], points.vel[active - size * size + size - 1]) //i-1,j+1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + size * size - size - 1], points.vel[active], points.vel[active + size * size - size - 1]) //i+1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + size * size + size - 1], points.vel[active], points.vel[active + size * size + size - 1]) //i+1,j+1
                                  - new Vector3(0, gravity / M, 0);  // Gravity

                active = i + size*size*j;
                points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + size * size]) // i + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - size * size]) // i - 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size*size], points.vel[active], points.vel[active + size]) // j + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size*size], points.vel[active], points.vel[active - size]) // j - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size * size], points.vel[active], points.vel[active - 1 - size * size]) //i-1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size * size], points.vel[active], points.vel[active - 1 + size * size]) //i-1,j+1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size * size], points.vel[active], points.vel[active + 1 - size * size]) //i+1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size * size], points.vel[active], points.vel[active + 1 + size * size]) //i+1,j+1

                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size], points.vel[active], points.vel[active + size]) //one in.
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + 1 + size], points.vel[active], points.vel[active + size * size + size]) // i + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - 1 + size], points.vel[active], points.vel[active - size * size + size]) // i - 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size * size + size], points.vel[active], points.vel[active + size + size]) // j + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size * size + size], points.vel[active], points.vel[active - size + size]) // j - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size * size + size], points.vel[active], points.vel[active - 1 - size * size + size]) //i-1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size * size + size], points.vel[active], points.vel[active - 1 + size * size + size]) //i-1,j+1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size * size + size], points.vel[active], points.vel[active + 1 - size * size + size]) //i+1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size * size + size], points.vel[active], points.vel[active + 1 + size * size + size]) //i+1,j+1
                                  - new Vector3(0, gravity / M, 0);  // Gravity

                active = i + size * size * j + size*(size-1);
                points.acc[active] = spring_damper3D(dist, points.pos[active], points.pos[active + 1], points.vel[active], points.vel[active + size * size]) // i + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - 1], points.vel[active], points.vel[active - size * size]) // i - 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size * size], points.vel[active], points.vel[active + size]) // j + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size * size], points.vel[active], points.vel[active - size]) // j - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size * size], points.vel[active], points.vel[active - 1 - size * size]) //i-1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size * size], points.vel[active], points.vel[active - 1 + size * size]) //i-1,j+1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size * size], points.vel[active], points.vel[active + 1 - size * size]) //i+1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size * size], points.vel[active], points.vel[active + 1 + size * size]) //i+1,j+1

                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size], points.vel[active], points.vel[active - size]) //one in.
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + 1 - size], points.vel[active], points.vel[active + size * size - size]) // i + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - 1 - size], points.vel[active], points.vel[active - size * size - size]) // i - 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active + size * size - size], points.vel[active], points.vel[active + size - size]) // j + 1
                                  + spring_damper3D(dist, points.pos[active], points.pos[active - size * size - size], points.vel[active], points.vel[active - size - size]) // j - 1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 - size * size - size], points.vel[active], points.vel[active - 1 - size * size - size]) //i-1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active - 1 + size * size - size], points.vel[active], points.vel[active - 1 + size * size - size]) //i-1,j+1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 - size * size - size], points.vel[active], points.vel[active + 1 - size * size - size]) //i+1,j-1
                                  + spring_damper3D(diag, points.pos[active], points.pos[active + 1 + size * size - size], points.vel[active], points.vel[active + 1 + size * size - size]) //i+1,j+1
                                  - new Vector3(0, gravity / M, 0);  // Gravity
            }
        }

        // calculate the velocity and positions for all masses.
        for (int y = 0; y < size; y++)
        {
           for (int z = 0; z < size; z++)
           {
              for (int x = 0; x < size; x++)
              {
                  active = x + z * size + y * size * size;
                  points.vel[active] = points.vel[active] + h * points.acc[active];
                  points.pos[active] = points.pos[active] + h * points.vel[active];
              }
           }
        }

        mesh.vertices = points.pos;
    }

    public Vector3 spring_damper3D(float distance, Vector3 pos_from, Vector3 pos_to, Vector3 v_from, Vector3 v_to)
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
