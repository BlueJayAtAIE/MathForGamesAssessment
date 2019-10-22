using System;
using System.Collections.Generic;
using Raylib;
using static Raylib.Raylib;

namespace MatrixHierarchies
{
    class Game
    {
        Timer gameTime = new Timer();

        private float timer = 0;
        private int fps = 1;
        private int frames;
        private float debugTimer = 0;

        private float deltaTime;
        private float playerSpeed = 100f;

        List<SceneObject> Hierarchy = new List<SceneObject>();

        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();

        MathFunctions.AABB playerCollider = new MathFunctions.AABB();

        Color boxColor = Color.GREEN;
        MathFunctions.AABB boxCollider = new MathFunctions.AABB(new MathFunctions.Vector3(120, 120, 0), new MathFunctions.Vector3(200, 200, 0));

        float textColorF = 1.0f;

        public void Init()
        {
            SetTargetFPS(60);

            Hierarchy.Add(tankObject);

            tankSprite.Load("tankBlue_outline.png");
            // Sprite is facing the wrong way... fix that here.
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // Sets an offset for the base, so it rotates around the center.
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);

            turretSprite.Load("barrelBlue.png");
            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // Set the turret offset from the tank base.
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);


            turretObject.AddChild(turretSprite);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turretObject);

            tankObject.SetPosition(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);
            
            //tankObject.SetPosition(0, 0);
        }

        public void Shutdown()
        {
            // Purposefully Blank.
        }

        public void Update()
        {
            // Time Calculations CALL ONLY ONCE PER UPDATE ----------
            deltaTime = gameTime.GetDeltaTime();

            timer += deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;

            textColorF++;
            if (textColorF <= 0 || textColorF >= 350) textColorF = 0;

            // PLAYER MOVEMENT --------------------------------------
            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_W))
            {
                MathFunctions.Vector3 facing = new MathFunctions.Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) * deltaTime * playerSpeed;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                MathFunctions.Vector3 facing = new MathFunctions.Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) * deltaTime * -playerSpeed;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_Q))
            {
                turretObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(deltaTime);
            }
            if (IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                Hierarchy.Add(new Projectile(turretObject.GlobalTransform.m4, turretObject.GlobalTransform.m5));
                // TODO: This is going to need a lot of testing bc I dont know what I'm doing :ok_hand:
            }

            playerCollider.Resize(new MathFunctions.Vector3(tankObject.GlobalTransform.m7 - (tankSprite.Width / 2), tankObject.GlobalTransform.m8 - (tankSprite.Height / 2), 0),
                                  new MathFunctions.Vector3(tankObject.GlobalTransform.m7 + (tankSprite.Width / 2), tankObject.GlobalTransform.m8 + (tankSprite.Height / 2), 0));

            // Check to see if the item acually needs to be deleted
            for (int i = 0; i < Hierarchy.Count; i++)
            {
                if (Hierarchy[i].removeMe)
                {
                    Hierarchy.Remove(Hierarchy[i]);
                }
            }

            // Collision Demo
            if (boxCollider.Overlaps(playerCollider))
            {
                boxColor = Color.RED;
            }
            else
            {
                boxColor = Color.GREEN;
            }

            // Update
            foreach (SceneObject s in Hierarchy)
            {
                s.Update(deltaTime);
            }

            // Debug - KEEP COMMENTED UNLESS TESTING ----------------
            //debugTimer++;
            //if (debugTimer % 5000 == 0)
            //{
            //    turretObject.LocalTransform.PrintCels();
            //}
            //Console.WriteLine($"OtherCollider dets ={playerCollider.min.x}, {playerCollider.min.y} {playerCollider.max.x}, {playerCollider.max.y}");
            //Console.WriteLine($"BoxCollider dets ={boxCollider.min.x}, {boxCollider.min.y} {boxCollider.max.x}, {boxCollider.max.y}");
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.WHITE);
            DrawText($"FPS: {fps.ToString()}", 10, 10, 12, ColorFromHSV(new Vector3(textColorF, 1, 1)));

            DrawRectangle(120, 120, 80, 80, boxColor);

            DrawCircle((int)playerCollider.Corners()[0].x, (int)playerCollider.Corners()[0].y, 6, Color.PURPLE);
            DrawCircle((int)playerCollider.Corners()[1].x, (int)playerCollider.Corners()[1].y, 6, Color.PURPLE);
            DrawCircle((int)playerCollider.Corners()[2].x, (int)playerCollider.Corners()[2].y, 6, Color.DARKPURPLE);
            DrawCircle((int)playerCollider.Corners()[3].x, (int)playerCollider.Corners()[3].y, 6, Color.DARKPURPLE);

            DrawCircle((int)boxCollider.Corners()[0].x, (int)boxCollider.Corners()[0].y, 6, Color.BLUE);
            DrawCircle((int)boxCollider.Corners()[1].x, (int)boxCollider.Corners()[1].y, 6, Color.BLUE);
            DrawCircle((int)boxCollider.Corners()[2].x, (int)boxCollider.Corners()[2].y, 6, Color.DARKBLUE);
            DrawCircle((int)boxCollider.Corners()[3].x, (int)boxCollider.Corners()[3].y, 6, Color.DARKBLUE);

            foreach (SceneObject s in Hierarchy)
            {
                s.Draw();
            }

            EndDrawing();
        }
    }
}
