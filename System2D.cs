using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*** NOTERING ***

  * Det är vänster-hands-system för trianglarna i unity.
  * 
  * För att använda koden skapa en tomt gameobject. tryck "add component" och lägg till Mesh Renderer, Mesh Filter och även det här scriptet. 
  * Gör ett nytt material och lägg till i menyn vid scriptet i component listan.
  * tada!! profit. Det finns problem med koden men det borde funka ganska okej.
  * 




 /*** NOTERING ***/




public class System2D : MonoBehaviour
{
    //Desides the size for our plane, where the amount of masses is: size*size 
    public int size = 10;
    public float gravity = 0.2f;
    
    //Variables for the system.
    public float R = 1f;
    public float M = 5f;
    public float K = 1f;
    public float dist = 1.0f;
    public Vector3 point = new Vector3(10,10,-5);  // flyttar ned en verecies så mycket från startpositionen.
    
    Vector3[] vertices; // The vertices in our mesh. Also the possition of our masses.
    Vector3[] vel;      // velocity of each point.
    Vector3[] acc;      // acceleration fo each point.

    
    int[] triangles;    //triangles for the mesh
    Vector3[] normals;  //normals for the mesh
    Vector2[] uv;       //uv's for the mesh.

    
    Mesh mesh;          //The mesh.

    MeshFilter mf;      //allows the mesh to be drawn to the view.
    MeshRenderer mr;    //The Mesh-Renderer takes the geometry from the Mesh-Filter and renders it at the position defined by the GameObject’s Transform component
    public Material mat;

    //Is called once before the first rendered image.
    void Start()
    {
        // Gives variables the reference to our components added on the gameobject.
        mf = GetComponent<MeshFilter>(); 
        mr = GetComponent<MeshRenderer>();

        mesh = new Mesh(); 
        mf.mesh = mesh;
        mr.material = mat;
        InitiateMesh();


    
        mesh.vertices = vertices;
    }

    //Main loop. 
    private void Update()
    {
        


        UpdateVertPos3D(); //The modell is implemented in here.

        checkFloor(); //check collision with the floor. Now at -10.0f lower than the start point.


        //Just testing of system
        int vert = size * size - size;
        for (int i = 0; i < size; i++)
        {
            vertices[vert +i] = point + new Vector3(i*0.1f,0,0);
            vel[vert+i] = new Vector3(0, 0, 0);
            acc[vert+i] = new Vector3(0, 0, 0);

        }

    }



    /***    Functions   ***/

    void InitiateMesh()
    {
        //Gives all vertex their start positions.
        InitiateVertPos();

        CalculateTriangles();

        CalculateNormals();

        CalculateUvs();
    }

    //Gives all vertex their start possitions.
    void InitiateVertPos()
    {
        //Allocate a vertex point each mass.
        vertices = new Vector3[size * size];
        vel = new Vector3[size * size];
        acc = new Vector3[size * size];

        int row = 0;
        //Loopar igenom alla vecticies och ger dem dess värden.
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                vertices[x + row*size] = new Vector3(x, y, 0);
                vel[x + row * size] = new Vector3(0, 0, 0);
                acc[x + row * size] = new Vector3(0, 0, 0);

                //Debug.Log("pos: " + x + ", " + y);
            }
            row++;
        }

        mesh.vertices = vertices;
    }

    //Decides how the triangles will be drawn.
    void CalculateTriangles()
    {
        //Amount of triangles needed.
        int tri = (size - 1) * (size - 1) * 2;

        triangles = new int[tri * 3]; //initiate space for the points of the triangles.
        //Debug.Log("Amount of triangles: " + tri);

        int row_counter = 0;
        int vert_counter = 0;
        //Loops for each Square and makes 2 triangles.
        for (int y = 0; y < size -1; y++)
        {
            for (int x = 0; x < size -1; x++)
            {
                triangles[vert_counter + 0] = size * row_counter + x;
                triangles[vert_counter + 1] = size * row_counter + x + size;
                triangles[vert_counter + 2] = size * row_counter + x + size + 1;

                triangles[vert_counter + 3] = size * row_counter + x;
                triangles[vert_counter + 4] = size * row_counter + x + size + 1;
                triangles[vert_counter + 5] = size * row_counter + x + 1;
                vert_counter += 6;
                /*Debug.Log("Triangles: \n" +
                (size * row + x) + "," + (size * row + x + size) + "," + (size * row + x + size + 1) + "\n" + 
                 (size * row + x) + "," + (size * row + x + size + 1) + "," + (size * row + x + 1) + "\n" ); */
            }
            row_counter++;
        }

        mesh.triangles = triangles;
    }

    //uncomplete function. 
    void CalculateNormals()
    {
        //Makes all normals point to one direction.
        normals = new Vector3[vertices.Length];

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -Vector3.forward;
        }


        mesh.normals = normals;
    }

    //uncomplete function.
    void CalculateUvs()
    {
        uv = new Vector2[vertices.Length];

        for (int i = 0; i < uv.Length; i++)
        {
            
        }
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);
        mesh.uv = uv;
    }

    //Gives all vertex their next possitions.2D!!
    void UpdateVertPos()
    {
        
        float diag = Mathf.Sqrt(dist * dist + dist * dist);
        float h = 0.1f;


        //Calculate acceleration for all messes.
        int active; //stores the position which will be connected.

        //Goes through all point which is'nt at the edges.
        int row = 1;
        for (int y = 1; y < size -1; y++)
        {
            for (int x = 1; x < size -1; x++)
            {
                active = x + row * size;
                acc[active] = spring_damper2D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1])
                            + spring_damper2D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1])
                            + spring_damper2D(dist, vertices[active], vertices[active + size], vel[active], vel[active +size])
                            + spring_damper2D(dist, vertices[active], vertices[active - size], vel[active], vel[active -size]);


            }
            row++;
        }
        

        //Felet är inte i kanterna!
        //position (0,0)
        active = 0;
        acc[active] = spring_damper2D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1]) //the connection right 
               + spring_damper2D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size]);  //the connection up
        //position (size,0)
        active = size-1;
        acc[active] = spring_damper2D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1]) //the connection left 
               + spring_damper2D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size]);  //the connection up

        //position (0,size)
        active = size * size - size;
        acc[active] = spring_damper2D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1]) //the connection right 
               + spring_damper2D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size]);  //the connection down
        //position (size,size)
        active = size * size - 1;
        acc[active] = spring_damper2D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1]) //the connection left 
               + spring_damper2D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size]);  //the connection down
        

        //the edges
        for (int i = 1; i < size -1; i++)
        {
            active = i;
            acc[active] = spring_damper2D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size]); //bottom edge.
            
            active = i * size;
            acc[active] = spring_damper2D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1]); //left edge.
            

            
            active = i * size + size -1;
            acc[active] = spring_damper2D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1]); //right edge.

            
            active = size * size - 1 - i;
            acc[active] = spring_damper2D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size]); //top edge.
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

    void UpdateVertPos3D() //3D!!!
    {

        float diag = Mathf.Sqrt(dist * dist + dist * dist);
        float h = 0.1f;


        //Calculate acceleration for all messes.
        int active; //stores the position which will be connected.

        //Goes through all point which is'nt at the edges.
        
        for (int y = 1; y < size - 1; y++)
        {
            for (int x = 1; x < size - 1; x++)
            {
                
                active = x + y * size;
                acc[active] = spring_damper3D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1])
                            + spring_damper3D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1])
                            + spring_damper3D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size])
                            + spring_damper3D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size]) - new Vector3( 0,gravity / M,0);
                            ;


            }
            
        }

        
        
        //position (0,0)
        active = 0;
        acc[active] = spring_damper3D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1]) //the connection right 
               + spring_damper3D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size]) - new Vector3(0, gravity / M, 0);  //the connection up
        //position (size,0)
        active = size - 1;
        acc[active] = spring_damper3D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1]) //the connection left 
               + spring_damper3D(dist, vertices[active], vertices[active + size], vel[active], vel[active + size]) - new Vector3(0, gravity / M, 0);  //the connection up

        //position (0,size)
        active = size * size - size;
        acc[active] = spring_damper3D(dist, vertices[active], vertices[active + 1], vel[active], vel[active + 1]) //the connection right 
               + spring_damper3D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size]) - new Vector3(0, gravity / M, 0);  //the connection down
        //position (size,size)
        active = size * size - 1;
        acc[active] = spring_damper3D(dist, vertices[active], vertices[active - 1], vel[active], vel[active - 1]) //the connection left 
               + spring_damper3D(dist, vertices[active], vertices[active - size], vel[active], vel[active - size]) - new Vector3(0, gravity / M, 0);  //the connection down
        

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

    //The function from matlab.
    public Vector2 spring_damper2D(float distance, Vector2 x_from, Vector2 x_to, Vector2 v_from, Vector2 v_to)
    {

        Vector2 direction = x_to - x_from;
        Vector2 velocity = v_to - v_from;

        //ACCELERATION FROM SPRING.
        //Absolute value from direction.
        float abs = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
        //Normalized direction vector.
        Vector2 normalized_vector;
        normalized_vector.x = direction.x / abs;
        normalized_vector.y = direction.y / abs;
        //Adds the distance to our direction vector.
        Vector2 desired_distance;
        desired_distance.x = distance * normalized_vector.x;
        desired_distance.y = distance * normalized_vector.y;
        //Adds spring constant to the length difference for uor spring.
        Vector2 acc_spring = K * (direction - desired_distance);

        //ACCELERATION FROM DAMPER.
        Vector2 acc_damper = R * velocity;

        //FINAL ACCELERATION.
        return (acc_spring + acc_damper);
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

    private void checkFloor()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if( vertices[x + y*size].y < -10.0f)
                {
                    vertices[x + y * size].y = -10.0f;

                    acc[x + y * size].y = 0.0f;
                    acc[x + y * size].x = 0.0f;

                    vel[x + y * size].y = 0.0f;
                    vel[x + y * size].x = 0.0f;
                }

            }
        }
    }
}
