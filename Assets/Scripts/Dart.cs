using System.Collections;
using System.Collections.Generic;
using System;
using static MyVector;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField] private float _Vstart;
    [SerializeField] private float _degree;
    [SerializeField] private float _Vmax;
    [SerializeField] private Transform _Collider;
    private bool ChangeMoveSide;
    public States CurState;

    public vect PosNow, V, g;
    public bool active = false, pressed = false;
    public int Score;
    public float Tension;
   

    private vect accel(vect g, vect v, float vmax, float h)  
    {
        return g - (float)Math.Exp(-h / 10000f) * g.modul() * v * (v).modul() / (vmax * vmax);
    }
    private void Euler(float dt, vect g, ref vect r, ref vect v, float vmax)
    {
        vect a = new vect();
        a = accel(g, v, vmax, r.y);
        v += a * dt;
        r += v * dt;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        _degree *= (float)Math.PI / 180f;

        V.x = _Vstart * (float)Math.Cos(_degree);
        V.y = _Vstart * (float)Math.Sin(_degree);
        V.z = 0f;

        g.x = 0f; g.y = -9.81f; g.z = 0f;

        PosNow.x = transform.position.x;
        PosNow.y = transform.position.y;

        CurState = States.FirstState;
    }

    // Update is called once per frame
    void Update()
    {
        switch(CurState)
        {
            case States.FirstState:
                MoveDart();
                if (Input.GetMouseButtonDown(0))
                {
                    CurState = States.SecondState;
                } 
                break;

            case States.SecondState:
                if (Input.GetMouseButtonDown(1))
                {
                    pressed = true;
                }
                if (pressed && Tension < 1)
                {
                    Tension += 0.01f;
                } 
                if (Input.GetMouseButtonUp(1))
                {
                    V.x = _Vmax * Tension * (float)Math.Cos(_degree);
                    V.y = _Vmax * Tension * (float)Math.Sin(_degree);
                    V.z = 0f;
                    Tension = 0;
                    pressed = false;
                    active = true;
                    CurState = States.ThirdState;
                    this.GetComponent<Rigidbody>().isKinematic = false;                    
                } 
                break;
                
            case States.ThirdState:
                if (active)
                {
                    Euler(0.05f, g, ref PosNow, ref V, _Vmax);

                    var Pos = new Vector3();
                    Pos.x = PosNow.x;
                    Pos.y = PosNow.y;
                    Pos.z = transform.position.z;

                    transform.position = Pos;
                    _Collider.position = Pos;
                }
                break;
        } 
        
        if (Input.GetKey("escape")) // если нажат Esc
        {
            Application.Quit();  // выйти из приложения
        }
    }

    void MoveDart()
    {
        float move;
        if (transform.position.z < -17)
            ChangeMoveSide = true;
        else if (transform.position.z > 17)
            ChangeMoveSide = false;
        
        if (ChangeMoveSide)
            move = 0.2f;
        else
            move = -0.2f;
        transform.position = new Vector3(transform.position.x, transform.position.y,  transform.position.z + move);
        PosNow.z = transform.position.z;
    }

    private void OnCollisionEnter(Collision collision)
    {
        active = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        Score = CountScore();
        var dartObj = GameObject.Find("Dartboard");
        if (collision.gameObject == dartObj)
        {
            var points = GameObject.Find("Points").GetComponent<TextMesh>();
            if (int.Parse(points.text) >= Score)
            {
                points.text = (int.Parse(points.text) - Score).ToString();
            }
            if (int.Parse(points.text) == 0)
            {
                GameObject.Find("Won").transform.position = new Vector3(0, 27, 0);
                
            }
        }

        Invoke("Respawn", 2);
    }

    public void Respawn()
    {
        this.GetComponent<Transform>().position = new Vector3(0, 15, 0);
        CurState = States.FirstState;
        V.x = _Vstart * (float)Math.Cos(_degree);
        V.y = _Vstart * (float)Math.Sin(_degree);
        V.z = 0f;
        PosNow.x = transform.position.x;
        PosNow.y = transform.position.y;
    }

    public int CountScore()
    {
        int sector, coef;
        float R = (float)Math.Sqrt(transform.position.z * transform.position.z
                                        + (transform.position.y - 20) * (transform.position.y - 20));
        if (R <= 0.81f) return 50;
        else if (R <= 1.81f) return 25;
        else if (R <= 6.81f)  coef = 1;
        else if (R <= 8.16f)  coef = 3;
        else if (R <= 11.85f)  coef = 1;
        else if (R <= 13.22f)  coef = 2;
        else coef = 0;

        float alfa = 180f / (float)Math.PI * (float)Math.Atan((transform.position.y - 20)/transform.position.z);

        if ((alfa >= 81 || alfa < -81) && transform.position.y > 20) sector = 20;
        else if ((alfa >= 81 || alfa < -81) && transform.position.y < 20) sector = 3;
        else if (alfa >= -81 && alfa < -63 && transform.position.z > 0) sector = 19;
        else if (alfa >= -81 && alfa < -63 && transform.position.z < 0) sector = 1;
        else if (alfa >= -63 && alfa < -45 && transform.position.z > 0) sector = 7;
        else if (alfa >= -63 && alfa < -45 && transform.position.z < 0) sector = 18;
        else if (alfa >= -45 && alfa < -27 && transform.position.z > 0) sector = 16;
        else if (alfa >= -45 && alfa < -27 && transform.position.z < 0) sector = 4;
        else if (alfa >= -27 && alfa < -9 && transform.position.z > 0) sector = 8;
        else if (alfa >= -27 && alfa < -9 && transform.position.z < 0) sector = 13;
        else if (alfa >= -9 && alfa < 9 && transform.position.z > 0) sector = 11;
        else if (alfa >= -9 && alfa < 9 && transform.position.z < 0) sector = 6;
        else if (alfa >= 9 && alfa < 27 && transform.position.z > 0) sector = 14;
        else if (alfa >= 9 && alfa < 27 && transform.position.z < 0) sector = 10;
        else if (alfa >= 27 && alfa < 45 && transform.position.z > 0) sector = 9;
        else if (alfa >= 27 && alfa < 45 && transform.position.z < 0) sector = 15;
        else if (alfa >= 45 && alfa < 63 && transform.position.z > 0) sector = 12;
        else if (alfa >= 45 && alfa < 63 && transform.position.z < 0) sector = 2;
        else if (alfa >= 63 && alfa < 81 && transform.position.z > 0) sector = 5;
        else sector = 17;
        
        return sector * coef;
    }
}
public enum States
{
    FirstState,
    SecondState,
    ThirdState
};

