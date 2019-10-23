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

        // false = static hitbox, true = resizing hitbox.
        bool collisionToggle = false;
        bool showHitboxCorners = false;

        bool isCollidingWall = false;

        List<SceneObject> Hierarchy = new List<SceneObject>();

        SceneObject projectileHolder = new SceneObject();

        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();

        Target target = new Target();
        int score = 0;

        MathFunctions.AABB playerCollider = new MathFunctions.AABB(new MathFunctions.Vector3(0, 0, 0), new MathFunctions.Vector3(0, 0, 0));
        SceneObject[] playerCornerPoints = new SceneObject[4] { new SceneObject(), new SceneObject(), new SceneObject(), new SceneObject()};
        MathFunctions.Vector3[] pCornersArray = new MathFunctions.Vector3[4];

        Color boxColor = Color.GREEN;
        MathFunctions.AABB boxCollider = new MathFunctions.AABB(new MathFunctions.Vector3(120, 120, 0), new MathFunctions.Vector3(200, 200, 0));

        MathFunctions.AABB solidCollider = new MathFunctions.AABB(new MathFunctions.Vector3(460, 260, 0), new MathFunctions.Vector3(500, 300, 0));

        public float rainbowColorF = 1.0f;
        public Color rainbow = new Color();

        public void Init()
        {
            Hierarchy.Add(projectileHolder);
            Hierarchy.Add(tankObject);
            Hierarchy.Add(target);

            tankSprite.Load("tankBlue_outline.png");
            // Sprite is facing the wrong way... fix that here.
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // Sets an offset for the base, so it rotates around the center.
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);

            turretSprite.Load("barrelBlue.png");
            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // Set the turret offset from the tank base.
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);

            // Min.
            playerCornerPoints[0].SetPosition(tankObject.GlobalTransform.m7 - (tankSprite.Width / 2), tankObject.GlobalTransform.m8 - (tankSprite.Height / 2));
            // Max.
            playerCornerPoints[1].SetPosition(tankObject.GlobalTransform.m7 + (tankSprite.Width / 2), tankObject.GlobalTransform.m8 + (tankSprite.Height / 2));
            // Other two corners.
            playerCornerPoints[2].SetPosition(tankObject.GlobalTransform.m7 - (tankSprite.Width / 2), tankObject.GlobalTransform.m8 + (tankSprite.Height / 2));
            playerCornerPoints[3].SetPosition(tankObject.GlobalTransform.m7 + (tankSprite.Width / 2), tankObject.GlobalTransform.m8 - (tankSprite.Height / 2));

            turretObject.AddChild(turretSprite);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turretObject);
            for (int i = 0; i < playerCornerPoints.Length; i++)
            {
                tankObject.AddChild(playerCornerPoints[i]);
            }

            // Final placement of the Tank.
            tankObject.SetPosition(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);

            #region Printing Controls
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n===============================================================================");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("            CONTROLS                                             ");
            Console.WriteLine("   W - Forwards                                                  ");
            Console.WriteLine("   S - Backwards                                                 ");
            Console.WriteLine("   A - Rotate Left                                               ");
            Console.WriteLine("   D - Rotate Right                                              ");
            Console.WriteLine("   Q - Rotate Barrel Left                                        ");
            Console.WriteLine("   E - Rotate Barrel Right                                       ");
            Console.WriteLine("   Spacebar - Fire Projectile                                    ");
            Console.WriteLine("   O - (Debug) Show Hitbox Corners                               ");
            Console.WriteLine("   P - (Debug) Change Collider Logic                             ");
            Console.WriteLine("                                                                 ");
            Console.WriteLine("   Shoot targets for points.                                                   ");
            Console.WriteLine("   Green box interacts with player and bullets.                                ");
            Console.WriteLine("   Purple box is a solid wall, preventing movement.                            ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.Cyan;
            #endregion
        }

        public void Shutdown()
        {
            // Purposefully Blank.
        }

        public void Update()
        {
            #region Time Calculations
            deltaTime = gameTime.GetDeltaTime();

            timer += deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;

            rainbowColorF++;
            if (rainbowColorF <= 0 || rainbowColorF >= 360) rainbowColorF = 0;
            rainbow = ColorFromHSV(new Vector3(rainbowColorF, 1, 1));
            #endregion

            #region Player Input
            if (IsKeyDown(KeyboardKey.KEY_A) && !isCollidingWall)
            {
                tankObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_D) && !isCollidingWall)
            {
                tankObject.Rotate(deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_W) && !isCollidingWall)
            {
                MathFunctions.Vector3 facing = new MathFunctions.Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) * deltaTime * playerSpeed;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S) && !isCollidingWall)
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
                Projectile temp = new Projectile(turretObject.GlobalTransform.m5, -turretObject.GlobalTransform.m4);
                temp.SetPosition(turretObject.GlobalTransform.m7 + (turretObject.GlobalTransform.m5 * 30), turretObject.GlobalTransform.m8 + (-turretObject.GlobalTransform.m4 * 30));
                projectileHolder.AddChild(temp);
            }
            if (IsKeyPressed(KeyboardKey.KEY_P))
            {
                collisionToggle = !collisionToggle;
            }
            if (IsKeyPressed(KeyboardKey.KEY_O))
            {
                showHitboxCorners = !showHitboxCorners;
            }
            #endregion

            #region Collision Box Updates
            if (collisionToggle)
            {
                // For an AABB that resizes during transforms.
                for (int i = 0; i < playerCornerPoints.Length; i++)
                {
                    pCornersArray[i] = new MathFunctions.Vector3(playerCornerPoints[i].GlobalTransform.m7, playerCornerPoints[i].GlobalTransform.m8, 0);
                }
                playerCollider.Fit(pCornersArray);
            }
            else
            {
                // For a static AABB. 
                playerCollider.Resize(new MathFunctions.Vector3(tankObject.GlobalTransform.m7 - (tankSprite.Width / 2), tankObject.GlobalTransform.m8 - (tankSprite.Height / 2), 0),
                                      new MathFunctions.Vector3(tankObject.GlobalTransform.m7 + (tankSprite.Width / 2), tankObject.GlobalTransform.m8 + (tankSprite.Height / 2), 0));
            }
            #endregion

            #region Projectiles
            // Check to see if the projectile acually needs to be deleted
            for (int i = 0; i < projectileHolder.GetChildCount(); i++)
            {
                if (projectileHolder.GetChild(i).removeMe)
                {
                    projectileHolder.RemoveChild(projectileHolder.GetChild(i));
                }
            }
            #endregion

            #region Collision Demo
            if (playerCollider.Overlaps(solidCollider))
            {
                tankObject.SetPosition(tankObject.GlobalTransform.m7 - (0.1f * tankObject.GlobalTransform.m5), tankObject.GlobalTransform.m8 - (0.1f * -tankObject.GlobalTransform.m4));
                isCollidingWall = true;
            }
            else
            {
                isCollidingWall = false;
            }

            if (boxCollider.Overlaps(playerCollider))
            {
                boxColor = Color.RED;
            }
            else
            {
                boxColor = Color.GREEN;
            }

            for (int i = 0; i < projectileHolder.GetChildCount(); i++)
            {
                Projectile temp = (Projectile)projectileHolder.GetChild(i);

                if(temp.projectileCollider.Overlaps(target.targetCollider))
                {
                    target.Respawn();
                    score++;
                    projectileHolder.RemoveChild(temp);
                }

                if (!boxCollider.Overlaps(playerCollider))
                {
                    if (temp.projectileCollider.Overlaps(boxCollider))
                    {
                        boxColor = Color.RED;
                        break;
                    }
                    else
                    {
                        boxColor = Color.GREEN;
                    }
                }

            }
            #endregion

            // Update
            foreach (SceneObject s in Hierarchy)
            {
                s.Update(deltaTime);
            }

            #region Debug - KEEP COMMENTED UNLESS TESTING
            //debugTimer++;
            //if (debugTimer % 200 == 0)
            //{
            //    turretObject.GlobalTransform.PrintCels();
            //}
            //Console.WriteLine($"OtherCollider dets ={playerCollider.min.x}, {playerCollider.min.y} {playerCollider.max.x}, {playerCollider.max.y}");
            //Console.WriteLine($"BoxCollider dets ={boxCollider.min.x}, {boxCollider.min.y} {boxCollider.max.x}, {boxCollider.max.y}");
            #endregion
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.RAYWHITE);

            DrawRectangle(120, 120, 80, 80, boxColor);

            DrawRectangle(455, 255, 50, 50, Color.DARKPURPLE);

            if (showHitboxCorners)
            {
                Color cornerColor = new Color();

                DrawText("HITBOX VIEW", 60, 10, 12, rainbow);

                for (int i = 0; i < 4; i++)
                {
                    if (i % 2 == 0)
                    {
                        cornerColor = Color.GREEN;
                    }
                    else
                    {
                        cornerColor = Color.DARKPURPLE;
                    }

                    DrawCircle((int)playerCollider.Corners()[i].x, (int)playerCollider.Corners()[i].y, 6, cornerColor);
                    DrawCircle((int)boxCollider.Corners()[i].x, (int)boxCollider.Corners()[i].y, 6, cornerColor);
                    DrawCircle((int)solidCollider.Corners()[i].x, (int)solidCollider.Corners()[i].y, 6, cornerColor);
                }
            }


            foreach (SceneObject s in Hierarchy)
            {
                s.Draw();
            }

            DrawText($"FPS: {fps.ToString()}", 10, 10, 12, rainbow);
            DrawText("SCORE:", 540, 10, 14, Color.DARKGRAY);
            DrawText($"{score}", 595, 10, 14, rainbow);
            DrawText("Press P to toggle hitbox logic.", 350, 460, 12, Color.DARKGRAY);
            if (collisionToggle)
            {
                DrawText("Transforming", 540, 460, 12, rainbow);
            }
            else
            {
                DrawText("Static", 540, 460, 12, rainbow);
            }

            EndDrawing();
        }
    }
}
