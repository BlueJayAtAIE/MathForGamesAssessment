using System;
using System.Collections.Generic;
using System.Text;
using Raylib;
using static Raylib.Raylib;

namespace MatrixHierarchies
{
    class SpriteObject : SceneObject
    {
        Texture2D texture = new Texture2D();
        Image image = new Image();

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

        }

        public void Load(string fileName)
        {
            Image img = LoadImage(fileName);
            texture = LoadTextureFromImage(img);
        }

        public override void OnDraw()
        {
            // Pulls the rotation in radians and converts to degrees for use in Raylib's draw funtion.
            float rotation = (float)Math.Atan2(GlobalTransform.m2, GlobalTransform.m1);

            DrawTextureEx(texture, new Vector2(globalTransform.m7, globalTransform.m8), rotation * (float)(180.0f / Math.PI), 1, Color.WHITE);
        }
    }
}
