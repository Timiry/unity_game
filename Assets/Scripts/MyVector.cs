using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MyVector : MonoBehaviour
{
    public struct vect
    {
        public float x, y, z;
        public float modul()
        { return (float)Math.Sqrt(x * x + y * y + z * z); }
        public float Mod
        {
            get { return (float)Math.Sqrt(x * x + y * y + z * z); }
        }
        public vect(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static vect operator +(vect a, vect b)
        {
            vect c = new vect();
            c.x = a.x + b.x;
            c.y = a.y + b.y;
            return c;
        }
        public static float operator *(vect a, vect b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
        public static vect operator *(float c, vect a)
        {
            vect h = new vect();
            h.x = a.x * c;
            h.y = a.y * c;
            h.z = a.z * c;
            return h;
        }
        public static vect operator *(vect a, float c)
        {
            vect h = new vect();
            h.x = a.x * c;
            h.y = a.y * c;
            h.z = a.z * c;
            return h;
        }
        public static vect operator /(vect a, float c)
        {
            vect h = new vect();
            h.x = a.x / c;
            h.y = a.y / c;
            h.z = a.z / c;
            return h;
        }
        public static vect operator -(vect a, vect b)
        {
            vect c = new vect();
            c.x = a.x - b.x;
            c.y = a.y - b.y;
            c.z = a.z - b.z;
            return c;
        }
        public static vect operator &(vect a, vect b)
        {
            vect c = new vect();
            c.x = a.y * b.z - a.z * b.y;
            c.y = a.z * b.y - a.y * b.z;
            c.z = a.x * b.z - a.z * b.x;
            return c;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
