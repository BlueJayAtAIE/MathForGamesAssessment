using System;
using rl = Raylib;

namespace MathFunctions
{
    // ------------------------------------------------------------------------------------------
    // VECTOR2 ----------------------------------------------------------------------------------
    public struct Vector2
    {
        public float x, y;

        /// <summary>
        /// Creates a new Vector 3 with the specified values.
        /// </summary>
        public Vector2(float X, float Y)
        {
            x = X;
            y = Y;
        }

        /// <summary>
        /// Returns the Magnitude of the Vector2.
        /// </summary>
        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        /// <summary>
        /// Returns the Magnitude squared of the Vector2.
        /// This calculation is faster but also less accurate.
        /// </summary>
        public float MagnitudeSqr()
        {
            return x * x + y * y;
        }

        /// <summary>
        /// Gets the distance between two Points (represented by Vector2s).
        /// </summary>
        public float Distance(Vector2 compareTo)
        {
            float diffX = x - compareTo.x;
            float diffY = y - compareTo.y;
            return (float)Math.Sqrt(diffX * diffX + diffY * diffY);
        }

        /// <summary>
        /// Changes the Vector2 into a Normalized (Unit) Vector version of itself.
        /// </summary>
        public void Normalize()
        {
            float m = Magnitude();
            x /= m;
            y /= m;
        }

        /// <summary>
        /// Returns the Normalized (Unit) Vector version of itself without changing the Vector2 into it.
        /// </summary>
        public Vector2 GetNormalized()
        {
            return (this / Magnitude());
        }

        /// <summary>
        /// Returns a perpendicular Vector2 facing Right relitive to the original Vector2.
        /// </summary>
        public Vector2 GetPerpendicular()
        {
            return new Vector2(-y, x);
        }

        /// <summary>
        /// Returns a perpendicular Vector2 facing Left relitive to the original Vector2.
        /// </summary>
        /// <param name="isFacingLeft">If true, returns a left facing perpendicular Vector2.</param>
        public Vector2 GetPerpendicular(bool isFacingLeft)
        {
            if (isFacingLeft)
            {
                return new Vector2(y, -x);
            }
            return GetPerpendicular();
        }

        /// <summary>
        /// Finds the angle in radians between the Vector2 and specified other Vector2.
        /// </summary>
        /// <param name="compareTo">Other Vector2 to compare to for angle calculation.</param>
        public float AngleBetween(Vector2 compareTo)
        {
            // Normalize both Vector2s
            Vector2 a = GetNormalized();
            Vector2 b = compareTo.GetNormalized();

            return (float)Math.Acos(a.DotProduct(b));
        }

        /// <summary>
        /// DEBUG TOOL: prints the values of the Vector2 to the console.
        /// </summary>
        public void PrintValues(string name)
        {
            Console.WriteLine($"Hello, I am Vector2 {name}. I am defined by ({x}, {y}).");
        }

        /// <summary>
        /// DEBUG TOOL: prints the Magnitude of the Vector2 to the console.
        /// </summary>
        public void PrintMagnitude(string name)
        {
            Console.WriteLine($"Hello, I am Vector2 {name}. My magnitude is {Magnitude()}");
        }

        // Conversions
        public rl.Vector2 ConvertedToRaylibV2()
        {
            rl.Vector2 tmp = new rl.Vector2();
            tmp.x = x;
            tmp.y = y;
            return tmp;
        }

        // Addition
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        // Subtraction
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        // Multiplication
        public static Vector2 operator *(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.x * rhs, lhs.y * rhs);
        }
        public static Vector2 operator *(float lhs, Vector2 rhs)
        {
            return new Vector2(lhs * rhs.x, lhs * rhs.y);
        }

        // Division
        public static Vector2 operator /(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.x / rhs, lhs.y / rhs);
        }
        public static Vector2 operator /(float lhs, Vector2 rhs)
        {
            return new Vector2(lhs / rhs.x, lhs / rhs.y);
        }

        // Dot Product
        public float DotProduct(Vector2 rhs)
        {
            return x * rhs.x + y * rhs.y;
        }
    }

    // ------------------------------------------------------------------------------------------
    // VECTOR3 ----------------------------------------------------------------------------------
    public struct Vector3
    {
        public float x, y, z;

        /// <summary>
        /// Creates a new Vector3 with the specified values.
        /// </summary>
        public Vector3(float X, float Y, float Z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        /// <summary>
        /// Returns the Magnitude of the Vector3.
        /// </summary>
        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Returns the Magnitude squared of the Vector3.
        /// This calculation is faster but also less accurate.
        /// </summary>
        public float MagnitudeSqr()
        {
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// Gets the distance between two Points (represented by Vector3s).
        /// </summary>
        public float Distance(Vector3 compareTo)
        {
            float diffX = x - compareTo.x;
            float diffY = y - compareTo.y;
            float diffZ = z - compareTo.z;
            return (float)Math.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ);
        }

        /// <summary>
        /// Changes the Vector3 into a Normalized (Unit) Vector version of itself.
        /// </summary>
        public void Normalize()
        {
            float m = Magnitude();
            x /= m;
            y /= m;
            z /= m;
        }

        /// <summary>
        /// Returns the Normalized (Unit) Vector version of itself without changing the Vector3 into it.
        /// </summary>
        public Vector3 GetNormalized()
        {
            return (this / Magnitude());
        }

        /// <summary>
        /// Finds the angle in radians between the Vector3 and specified other Vector3.
        /// </summary>
        /// <param name="compareTo">Other Vector3 to compare to for angle calculation.</param>
        public float AngleBetween(Vector3 compareTo)
        {
            // Normalize both Vector3s
            Vector3 a = GetNormalized();
            Vector3 b = compareTo.GetNormalized();

            return (float)Math.Acos(a.DotProduct(b));
        }

        /// <summary>
        /// DEBUG TOOL: prints the values of the Vector3 to the console.
        /// </summary>
        public void PrintValues(string name)
        {
            Console.WriteLine($"Hello, I am Vector3 {name}. I am defined by ({x}, {y}, {z}).");
        }

        /// <summary>
        /// DEBUG TOOL: prints the Magnitude of the Vector3 to the console.
        /// </summary>
        public void PrintMagnitude(string name)
        {
            Console.WriteLine($"Hello, I am Vector3 {name}. My magnitude is {Magnitude()}.");
        }

        // Addition
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        // Subtraction
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        // Multiplication
        public static Vector3 operator *(Vector3 lhs, float rhs)
        {
            return new Vector3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }
        public static Vector3 operator *(float lhs, Vector3 rhs)
        {
            return new Vector3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
        }

        // Division
        public static Vector3 operator /(Vector3 lhs, float rhs)
        {
            return new Vector3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }
        public static Vector3 operator /(float lhs, Vector3 rhs)
        {
            return new Vector3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
        }

        // Dot Product
        public float DotProduct(Vector3 rhs)
        {
            return x * rhs.x + y * rhs.y + z * rhs.z;
        }

        // Cross Product
        public Vector3 CrossProduct(Vector3 rhs)
        {
            return new Vector3(
           y * rhs.z - z * rhs.y,
           z * rhs.x - x * rhs.z,
           x * rhs.y - y * rhs.x);
        }
    }

    // ------------------------------------------------------------------------------------------
    // VECTOR4 ----------------------------------------------------------------------------------
    public struct Vector4
    {
        public float x, y, z, w;

        public Vector4(float X, float Y, float Z, float W)
        {
            x = X;
            y = Y;
            z = Z;
            w = W;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z + w * w);
        }

        public void Normalize()
        {
            float m = Magnitude();
            x /= m;
            y /= m;
            z /= m;
            w /= m;
        }

        /// <summary>
        /// DEBUG TOOL: prints the values of the Vector4 to the console.
        /// </summary>
        public void PrintValues(string name)
        {
            Console.WriteLine($"Hello, I am Vector4 {name}. I am defined by ({x}, {y}, {z}, {w}).");
        }

        // Addition
        public static Vector4 operator +(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
        }

        // Subtraction
        public static Vector4 operator -(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
        }

        // Multiplication
        public static Vector4 operator *(Vector4 lhs, float rhs)
        {
            return new Vector4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
        }
        public static Vector4 operator *(float lhs, Vector4 rhs)
        {
            return new Vector4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
        }

        // Division
        public static Vector4 operator /(Vector4 lhs, float rhs)
        {
            return new Vector4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
        }
        public static Vector4 operator /(float lhs, Vector4 rhs)
        {
            return new Vector4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
        }

        // Dot Product
        public float Dot(Vector4 rhs)
        {
            return x * rhs.x + y * rhs.y + z * rhs.z + w * rhs.w;
        }

        // Cross Product
        public Vector4 Cross(Vector4 rhs)
        {
            return new Vector4(
           y * rhs.z - z * rhs.y,
           z * rhs.x - x * rhs.z,
           x * rhs.y - y * rhs.x,
           0);
        }
    }


    // ------------------------------------------------------------------------------------------
    // MATRIX2 ----------------------------------------------------------------------------------
    public struct Matrix2
    {
        public float m1, m2, m3, m4;

        public static Matrix2 identity = new Matrix2(1, 0, 0, 1);

        /// <summary>
        /// Creates a Matrix2 to the specified floats.
        /// </summary>
        public Matrix2(float M1, float M2, float M3, float M4)
        {
            m1 = M1; m2 = M3;
            m3 = M2; m4 = M4;
        }

        void Set(Matrix2 m)
        {
            m1 = m.m1; m3 = m.m3;
            m2 = m.m2; m4 = m.m4;
        }

        /// <summary>
        /// Returns the transpotition of the Matrix2.
        /// </summary>
        public Matrix2 GetTransposed()
        {
            return new Matrix2(
                m1, m2,
                m3, m4);
        }

        // Scaling --------------------------------------
        public void SetScaled(float x, float y)
        {
            m1 = x; m3 = 0;
            m2 = 0; m4 = y;
        }

        public void SetScaled(Vector2 v)
        {
            m1 = v.x; m3 = 0;
            m2 = 0; m4 = v.y;
        }

        public void Scale(float x, float y)
        {
            Matrix2 m = new Matrix2();
            m.SetScaled(x, y);

            Set(this * m);
        }

        public void Scale(Vector2 v)
        {
            Matrix2 m = new Matrix2();
            m.SetScaled(v);

            Set(this * m);
        }

        // Rotating ----------------------------------------------
        // X
        public void SetRotateX(double radians)
        {
            Matrix2 m = new Matrix2(
                1, (float)-Math.Sin(radians),
                0, (float)Math.Cos(radians));

            Set(m);
        }

        public void RotateX(double radians)
        {
            Matrix2 m = new Matrix2();
            m.SetRotateX(radians);
            Set(this * m);
        }

        // Y
        public void SetRotateY(double radians)
        {
            Matrix2 m = new Matrix2(
                (float)Math.Cos(radians), 0,
                (float)Math.Sin(radians), 1);

            Set(m);
        }

        public void RotateY(double radians)
        {
            Matrix2 m = new Matrix2();
            m.SetRotateY(radians);
            Set(this * m);
        }

        /// <summary>
        /// DEBUG TOOL: For printing out the numbers in the Matrix2.
        /// </summary>
        public void PrintCels()
        {
            Console.WriteLine($"{m1}, {m2}");
            Console.WriteLine($"{m3}, {m4}");
        }
        
        // Multiplication
        public static Matrix2 operator *(Matrix2 lhs, Matrix2 rhs)
        {
            return new Matrix2(
                lhs.m1 * rhs.m1 + lhs.m2 + rhs.m3, lhs.m1 * rhs.m2 + lhs.m2 * rhs.m4,
                lhs.m3 * rhs.m1 + lhs.m4 + rhs.m3, lhs.m3 * rhs.m2 + lhs.m4 * rhs.m4);
        }
        public static Vector2 operator *(Matrix2 lhs, Vector2 rhs)
        {
            return new Vector2((lhs.m1 * rhs.x) + (lhs.m3 * rhs.y),
                (lhs.m2 * rhs.x) + (lhs.m4 * rhs.y));
        }
    }

    // ------------------------------------------------------------------------------------------
    // MATRIX3 ----------------------------------------------------------------------------------
    public struct Matrix3
    {
        public float m1, m2, m3, m4, m5, m6, m7, m8, m9;

        public static Matrix3 identity = new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        /// <summary>
        /// Creates a Matrix3 to the specified floats.
        /// </summary>
        public Matrix3(float M1, float M2, float M3, float M4, float M5, float M6, float M7, float M8, float M9)
        {
            m1 = M1; m2 = M2; m3 = M3;
            m4 = M4; m5 = M5; m6 = M6;
            m7 = M7; m8 = M8; m9 = M9;
        }

        /// <summary>
        /// Sets the Matrix3 to the supplied Matrix3.
        /// </summary>
        void Set(Matrix3 m)
        {
            m1 = m.m1; m4 = m.m4; m7 = m.m7;
            m2 = m.m2; m5 = m.m5; m8 = m.m8;
            m3 = m.m3; m6 = m.m6; m9 = m.m9;
        }

        /// <summary>
        /// Returns the transpotition of the Matrix3.
        /// </summary>
        public Matrix3 GetTransposed()
        {
            return new Matrix3(
                m1, m4, m7,
                m2, m5, m8,
                m3, m6, m9);
        }

        /// <summary>
        /// DEBUG TOOL: For printing out the numbers in the Matrix3.
        /// </summary>
        public void PrintCels()
        {
            Console.WriteLine($"{m1}, {m2}, {m3}");
            Console.WriteLine($"{m4}, {m5}, {m6}");
            Console.WriteLine($"{m7}, {m8}, {m9}");
        }

        // Scaling --------------------------------------
        public void SetScaled(float x, float y, float z)
        {
            m1 = x; m4 = 0; m7 = 0;
            m2 = 0; m5 = y; m8 = 0;
            m3 = 0; m6 = 0; m9 = z;
        }

        public void SetScaled(Vector3 v)
        {
            m1 = v.x; m4 = 0; m7 = 0;
            m2 = 0; m5 = v.y; m8 = 0;
            m3 = 0; m6 = 0; m9 = v.z;
        }

        public void Scale(float x, float y, float z)
        {
            Matrix3 m = new Matrix3();
            m.SetScaled(x, y, z);

            Set(this * m);
        }

        public void Scale(Vector3 v)
        {
            Matrix3 m = new Matrix3();
            m.SetScaled(v);

            Set(this * m);
        }

        // Rotating ----------------------------------------------
        // X
        public void SetRotateX(double radians)
        {
            Matrix3 m = new Matrix3(
                1, 0, 0,
                0, (float)Math.Cos(radians), (float)Math.Sin(radians),
                0, -(float)Math.Sin(radians), (float)Math.Cos(radians));

            Set(m);
        }

        public void RotateX(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateX(radians);
            Set(this * m);
        }

        // Y
        public void SetRotateY(double radians)
        {
            Matrix3 m = new Matrix3(
                (float)Math.Cos(radians), 0, -(float)Math.Sin(radians),
                0, 1, 0,
                (float)Math.Sin(radians), 0, (float)Math.Cos(radians));

            Set(m);
        }

        public void RotateY(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateY(radians);
            Set(this * m);
        }

        // Z
        public void SetRotateZ(double radians)
        {
            Matrix3 m = new Matrix3(
                (float)Math.Cos(radians), (float)Math.Sin(radians), 0,
                -(float)Math.Sin(radians), (float)Math.Cos(radians), 0,
                0, 0, 1);

            Set(m);
        }

        public void RotateZ(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateZ(radians);
            Set(this * m);
        }

        // Euler Angle Based
        public void SetEuler(float pitch, float yaw, float roll)
        {
            Matrix3 x = new Matrix3();
            Matrix3 y = new Matrix3();
            Matrix3 z = new Matrix3();
            x.SetRotateX(pitch);
            y.SetRotateY(yaw);
            z.SetRotateZ(roll);

            Set(z * y * x);
        }

        // Translating ------------------------
        public void SetTranslation(float x, float y)
        {
            m7 = x; m8 = y; m9 = 1;
        }
        public void Translate(float x, float y)
        {
            m7 += x; m8 += y;
        }

        // Multiplication
        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            // NOTES: So because of the magic of column/row major stuff we need this transpose at the end.
            // Please just. Don't ask questions.

            Matrix3 result = new Matrix3(
            lhs.m1 * rhs.m1 + lhs.m4 * rhs.m2 + lhs.m7 * rhs.m3,
            lhs.m1 * rhs.m4 + lhs.m4 * rhs.m5 + lhs.m7 * rhs.m6,
            lhs.m1 * rhs.m7 + lhs.m4 * rhs.m8 + lhs.m7 * rhs.m9,

            lhs.m2 * rhs.m1 + lhs.m5 * rhs.m2 + lhs.m8 * rhs.m3,
            lhs.m2 * rhs.m4 + lhs.m5 * rhs.m5 + lhs.m8 * rhs.m6,
            lhs.m2 * rhs.m7 + lhs.m5 * rhs.m8 + lhs.m8 * rhs.m9,

            lhs.m3 * rhs.m1 + lhs.m6 * rhs.m2 + lhs.m9 * rhs.m3,
            lhs.m3 * rhs.m4 + lhs.m6 * rhs.m5 + lhs.m9 * rhs.m6,
            lhs.m3 * rhs.m7 + lhs.m6 * rhs.m8 + lhs.m9 * rhs.m9);

            return result.GetTransposed();
        }

        public static Vector3 operator *(Matrix3 lhs, Vector3 rhs)
        {
            return new Vector3((lhs.m1 * rhs.x) + (lhs.m4 * rhs.y) + (lhs.m7 * rhs.z),
                (lhs.m2 * rhs.x) + (lhs.m5 * rhs.y) + (lhs.m8 * rhs.z),
                (lhs.m3 * rhs.x) + (lhs.m6 * rhs.y) + (lhs.m9 * rhs.z));
        }
    }

    // ------------------------------------------------------------------------------------------
    // MATRIX4 ----------------------------------------------------------------------------------
    public struct Matrix4
    {
        public float m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14, m15, m16;

        public static Matrix4 identity = new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

        public Matrix4(float M1, float M2, float M3, float M4, float M5, float M6, float M7, float M8, float M9, float M10, float M11, float M12, float M13, float M14, float M15, float M16)
        {
            m1 = M1; m2 = M2; m3 = M3; m4 = M4;
            m5 = M5; m6 = M6; m7 = M7; m8 = M8;
            m9 = M9; m10 = M10; m11 = M11; m12 = M12;
            m13 = M13; m14 = M14; m15 = M15; m16 = M16;
        }

        /// <summary>
        /// Sets the Matrix4 to the supplied Matrix4.
        /// </summary>
        void Set(Matrix4 m)
        {
            m1 = m.m1; m5 = m.m5; m9 = m.m9; m13 = m.m13;
            m2 = m.m2; m6 = m.m6; m10 = m.m10; m14 = m.m14;
            m3 = m.m3; m7 = m.m7; m11 = m.m11; m15 = m.m15;
            m4 = m.m4; m8 = m.m8; m12 = m.m12; m16 = m.m16;
        }

        /// <summary>
        /// Returns the transpotition of the Matrix4.
        /// </summary>
        public Matrix4 GetTransposed()
        {
            return new Matrix4(
                m1, m2, m3, m4,
                m5, m6, m7, m8,
                m9, m10, m11, m12,
                m13, m14, m15, m16);
        }

        /// <summary>
        /// DEBUG TOOL: For printing out the numbers in the Matrix4.
        /// </summary>
        public void PrintCels()
        {
            Console.WriteLine($"{m1}, {m2}, {m3}, {m4}");
            Console.WriteLine($"{m5}, {m6}, {m7}, {m8}");
            Console.WriteLine($"{m9}, {m10}, {m11}, {m12}");
            Console.WriteLine($"{m13}, {m14}, {m15}, {m16}");
        }

        // Scaling -----------------------------------
        public void SetScaled(float x, float y, float z)
        {
            m1 = x; m2 = 0; m3 = 0; m4 = 0;
            m5 = 0; m6 = y; m7 = 0; m8 = 0;
            m9 = 0; m10 = 0; m11 = z; m12 = 0;
            m13 = 0; m14 = 0; m15 = 0; m16 = 1;
        }
        public void SetScaled(Vector3 v)
        {
            m1 = v.x; m2 = 0; m3 = 0; m4 = 0;
            m5 = 0; m6 = v.y; m7 = 0; m8 = 0;
            m9 = 0; m10 = 0; m11 = v.z; m12 = 0;
            m13 = 0; m14 = 0; m15 = 0; m16 = 1;
        }

        public void Scale(float x, float y, float z)
        {
            Matrix4 m = new Matrix4();
            m.SetScaled(x, y, z);

            Set(this * m);
        }
        public void Scale(Vector3 v)
        {
            Matrix4 m = new Matrix4();
            m.SetScaled(v);

            Set(this * m);
        }

        // Rotating ------------------------
        // X
        public void SetRotateX(double radians)
        {
            Matrix4 m = new Matrix4(
            1, 0, 0, 0,
            0, (float)Math.Cos(radians), (float)Math.Sin(radians), 0,
            0, -(float)Math.Sin(radians), (float)Math.Cos(radians), 0,
            0, 0, 0, 1);

            Set(m);
        }
        public void RotateX(double radians)
        {
            Matrix4 m = new Matrix4();
            m.SetRotateX(radians);
            Set(this * m);
        }

        // Y
        public void SetRotateY(double radians)
        {
            Matrix4 m = new Matrix4(
                (float)Math.Cos(radians), 0, -(float)Math.Sin(radians), 0,
                0, 1, 0, 0,
                (float)Math.Sin(radians), 0, (float)Math.Cos(radians), 0,
                0, 0, 0, 1);

            Set(m);
        }
        public void RotateY(double radians)
        {
            Matrix4 m = new Matrix4();
            m.SetRotateY(radians);
            Set(this * m);
        }

        // Z
        public void SetRotateZ(double radians)
        {
            Matrix4 m = new Matrix4(
                (float)Math.Cos(radians), (float)Math.Sin(radians), 0, 0,
                -(float)Math.Sin(radians), (float)Math.Cos(radians), 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

            Set(m);
        }
        public void RotateZ(double radians)
        {
            Matrix4 m = new Matrix4();
            m.SetRotateZ(radians);
            Set(this * m);
        }

        // Euler Angle Based
        public void SetEuler(float pitch, float yaw, float roll)
        {
            Matrix4 x = new Matrix4();
            Matrix4 y = new Matrix4();
            Matrix4 z = new Matrix4();
            x.SetRotateX(pitch);
            y.SetRotateY(yaw);
            z.SetRotateZ(roll);

            Set(z * y * x);
        }

        // Translating ------------------------
        public void SetTranslation(float x, float y, float z)
        {
            m13 = x; m14 = y; m15 = z; m16 = 1;
        }
        public void Translate(float x, float y, float z)
        {
            m13 += x; m14 += y; m15 += z;
        }

        // Multiplication
        public static Matrix4 operator *(Matrix4 lhs, Matrix4 rhs)
        {
            // NOTES: So because of the magic of column/row major stuff we need this transpose at the end.
            // Please just. Don't ask questions. 

            Matrix4 result = new Matrix4(
           rhs.m1 * lhs.m1 + rhs.m2 * lhs.m5 + rhs.m3 * lhs.m9 + rhs.m4 * lhs.m13,
           rhs.m1 * lhs.m2 + rhs.m2 * lhs.m6 + rhs.m3 * lhs.m10 + rhs.m4 * lhs.m14,
           rhs.m1 * lhs.m3 + rhs.m2 * lhs.m7 + rhs.m3 * lhs.m11 + rhs.m4 * lhs.m15,
           rhs.m1 * lhs.m4 + rhs.m2 * lhs.m8 + rhs.m3 * lhs.m12 + rhs.m4 * lhs.m16,

           rhs.m5 * lhs.m1 + rhs.m6 * lhs.m5 + rhs.m7 * lhs.m9 + rhs.m8 * lhs.m13,
           rhs.m5 * lhs.m2 + rhs.m6 * lhs.m6 + rhs.m7 * lhs.m10 + rhs.m8 * lhs.m14,
           rhs.m5 * lhs.m3 + rhs.m6 * lhs.m7 + rhs.m7 * lhs.m11 + rhs.m8 * lhs.m15,
           rhs.m5 * lhs.m4 + rhs.m6 * lhs.m8 + rhs.m7 * lhs.m12 + rhs.m8 * lhs.m16,

           rhs.m9 * lhs.m1 + rhs.m10 * lhs.m5 + rhs.m11 * lhs.m9 + rhs.m12 * lhs.m13,
           rhs.m9 * lhs.m2 + rhs.m10 * lhs.m6 + rhs.m11 * lhs.m10 + rhs.m12 * lhs.m14,
           rhs.m9 * lhs.m3 + rhs.m10 * lhs.m7 + rhs.m11 * lhs.m11 + rhs.m12 * lhs.m15,
           rhs.m9 * lhs.m4 + rhs.m10 * lhs.m8 + rhs.m11 * lhs.m12 + rhs.m12 * lhs.m16,

           rhs.m13 * lhs.m1 + rhs.m14 * lhs.m5 + rhs.m15 * lhs.m9 + rhs.m16 * lhs.m13,
           rhs.m13 * lhs.m2 + rhs.m14 * lhs.m6 + rhs.m15 * lhs.m10 + rhs.m16 * lhs.m14,
           rhs.m13 * lhs.m3 + rhs.m14 * lhs.m7 + rhs.m15 * lhs.m11 + rhs.m16 * lhs.m15,
           rhs.m13 * lhs.m4 + rhs.m14 * lhs.m8 + rhs.m15 * lhs.m12 + rhs.m16 * lhs.m16);

            return result.GetTransposed();
        }
        public static Vector4 operator *(Matrix4 lhs, Vector4 rhs)
        {
            return new Vector4(
           (lhs.m1 * rhs.x) + (lhs.m5 * rhs.y) + (lhs.m9 * rhs.z) + (lhs.m13 * rhs.w),
           (lhs.m2 * rhs.x) + (lhs.m6 * rhs.y) + (lhs.m10 * rhs.z) + (lhs.m14 * rhs.w),
           (lhs.m3 * rhs.x) + (lhs.m7 * rhs.y) + (lhs.m11 * rhs.z) + (lhs.m15 * rhs.w),
           (lhs.m4 * rhs.x) + (lhs.m8 * rhs.y) + (lhs.m12 * rhs.z) + (lhs.m16 * rhs.w));
        }
    }
}
