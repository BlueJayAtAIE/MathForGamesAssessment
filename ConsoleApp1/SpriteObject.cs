using System;
using MathFunctions;
using Raylib;
using static Raylib.Raylib;

namespace MatrixHierarchies
{
    class SpriteObject : SceneObject
    {
        Texture2D texture = new Texture2D();
        Image image = new Image();

        SceneObject parentObject = new SceneObject();

        public AABB collider = new AABB(new MathFunctions.Vector3(0, 0, 0), new MathFunctions.Vector3(0, 0, 0));
        SceneObject[] colliderCornerPoints = new SceneObject[4] { new SceneObject(), new SceneObject(), new SceneObject(), new SceneObject() };
        MathFunctions.Vector3[] pCornersArray = new MathFunctions.Vector3[4];

        public float Width
        {
            get { return texture.width; }
        }
        public float Height
        {
            get { return texture.height; }
        }

        public SpriteObject()
        {
            // Purposefully Blank.
        }

        public void Load(string fileName, SceneObject parent)
        {
            image = LoadImage(fileName);
            texture = LoadTextureFromImage(image);

            parentObject = parent;

            // Min.
            colliderCornerPoints[0].SetPosition(parentObject.GlobalTransform.m7 - (texture.width / 2), parentObject.GlobalTransform.m8 - (texture.height / 2));
            // Max.
            colliderCornerPoints[1].SetPosition(parentObject.GlobalTransform.m7 + (texture.width / 2), parentObject.GlobalTransform.m8 + (texture.height / 2));
            // Other two corners.
            colliderCornerPoints[2].SetPosition(parentObject.GlobalTransform.m7 - (texture.width / 2), parentObject.GlobalTransform.m8 + (texture.height / 2));
            colliderCornerPoints[3].SetPosition(parentObject.GlobalTransform.m7 + (texture.width / 2), parentObject.GlobalTransform.m8 - (texture.height / 2));

            for (int i = 0; i < colliderCornerPoints.Length; i++)
            {
                AddChild(colliderCornerPoints[i]);
            }
        }

        // Overrides ---------------------------------------------------------
        public override void OnUpdate(float deltaTime)
        {

            collider.Resize(new MathFunctions.Vector3(parentObject.GlobalTransform.m7 - (texture.width / 2), parentObject.GlobalTransform.m8 - (texture.height / 2), 0),
                            new MathFunctions.Vector3(parentObject.GlobalTransform.m7 + (texture.width / 2), parentObject.GlobalTransform.m8 + (texture.height / 2), 0));

            base.OnUpdate(deltaTime);
        }

        public override void OnDraw()
        {
            // Pulls the rotation in radians and converts to degrees for use in Raylib's draw funtion.
            float rotation = (float)Math.Atan2(globalTransform.m2, globalTransform.m1);

            // Here we make a Vector2 from our own math library and then use a function to convert it to a Raylib Vector2.
            // This is done to A) show we can convert between the two and so B) so we can use the Vector2 we worked on rather than using Raylib's.
            MathFunctions.Vector2 drawTransform = new MathFunctions.Vector2(globalTransform.m7, globalTransform.m8);

            DrawTextureEx(texture, drawTransform.ConvertedToRaylibV2(), rotation * (float)(180.0f / Math.PI), 1, Color.WHITE);
        }
    }

    class Projectile : SceneObject
    {
        private float lifetime = 1.5f;
        private float speed = 300f;
        private MathFunctions.Vector3 direction = new MathFunctions.Vector3(0, 0, 0);
        public Sphere projectileCollider = new MathFunctions.Sphere(new MathFunctions.Vector3(0, 0, 0), 0);

        public Projectile(MathFunctions.Vector3 dir)
        {
            direction = dir;
        }

        public Projectile(float xDir, float yDir)
        {
            direction.x = xDir;
            direction.y = yDir;
        }

        public override void OnUpdate(float deltaTime)
        {
            lifetime -= 1 * deltaTime;
            if (lifetime <= 0)
            {
                removeMe = true;
            }

            projectileCollider.Resize(new MathFunctions.Vector3(GlobalTransform.m7, GlobalTransform.m8, 0), 6);

            MathFunctions.Vector3 facing = new MathFunctions.Vector3(direction.x, direction.y, 1) * deltaTime * speed;
            Translate(facing.x, facing.y);
        }

        public override void OnDraw()
        {
            DrawCircle((int)projectileCollider.center.x, (int)projectileCollider.center.y, (int)projectileCollider.radius, Color.ORANGE);
        }
    }

    class Target : SceneObject
    {
        public Sphere targetCollider = new MathFunctions.Sphere(new MathFunctions.Vector3(0, 0, 15), 0);
        private float spawnPoint;

        public Target()
        {
            spawnPoint = Tools.rng.Next(40, 600);
            globalTransform.m7 = spawnPoint;
            spawnPoint = Tools.rng.Next(40, 440);
            globalTransform.m8 = spawnPoint;
        }

        public void Respawn()
        {
            spawnPoint = Tools.rng.Next(40, 600);
            globalTransform.m7 = spawnPoint;
            spawnPoint = Tools.rng.Next(40, 440);
            globalTransform.m8 = spawnPoint;
        }

        public override void OnUpdate(float deltaTime)
        {
            targetCollider.Resize(new MathFunctions.Vector3(GlobalTransform.m7, GlobalTransform.m8, 0), 15);
        }

        public override void OnDraw()
        {
            DrawCircle((int)targetCollider.center.x, (int)targetCollider.center.y, (int)targetCollider.radius + 1, Color.MAROON);
            DrawCircle((int)targetCollider.center.x, (int)targetCollider.center.y, (int)targetCollider.radius, Color.RED);
            DrawCircle((int)targetCollider.center.x, (int)targetCollider.center.y, (int)targetCollider.radius - 3, Color.WHITE);
            DrawCircle((int)targetCollider.center.x, (int)targetCollider.center.y, (int)targetCollider.radius - 6, Color.RED);
            DrawCircle((int)targetCollider.center.x, (int)targetCollider.center.y, (int)targetCollider.radius - 9, Color.WHITE);
            DrawCircle((int)targetCollider.center.x, (int)targetCollider.center.y, (int)targetCollider.radius - 12, Color.RED);
        }
    }
}
