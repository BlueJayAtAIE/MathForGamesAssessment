using System;
using System.Collections.Generic;
using System.IO;
using Raylib;
using static Raylib.Raylib;

namespace MatrixHierarchies
{
    class Game
    {
        // Time Related
        Timer gameTime = new Timer();

        public float remainingTime = 1;
        bool infiniteTime = false;

        private float timer = 0;
        private int fps = 1;
        private int frames;

        private float deltaTime;
        private float playerSpeed = 100f;

        // Debug
        // false = static hitbox, true = resizing hitbox.
        bool collisionToggle = false;
        bool showHitboxCorners = false;

        // Hierarchy, SceneObjects and "holders"
        List<SceneObject> Hierarchy = new List<SceneObject>();

        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();

        SceneObject projectileHolder = new SceneObject();

        SceneObject destructableHolder = new SceneObject();

        SceneObject targetHolder = new SceneObject();

        SceneObject wallHolder = new SceneObject();
        
        // Score related
        int score = 0;
        private int[] highscores = new int[3];
        private bool newHighscore = false;
        private int newScorePlace = -1;
        private static string highscorePath = "highscores.txt";

        // Player related
        MathFunctions.AABB playerCollider = new MathFunctions.AABB(new MathFunctions.Vector3(0, 0, 0), new MathFunctions.Vector3(0, 0, 0));
        SceneObject[] playerCornerPoints = new SceneObject[4] { new SceneObject(), new SceneObject(), new SceneObject(), new SceneObject()};
        MathFunctions.Vector3[] pCornersArray = new MathFunctions.Vector3[4];
        MathFunctions.Vector3 playerFacing = new MathFunctions.Vector3(0, 0, 0);
        MathFunctions.Matrix3 lastPlayerTransform = new MathFunctions.Matrix3();

        // Collision
        Color boxColor = Color.GREEN;
        MathFunctions.AABB boxCollider = new MathFunctions.AABB(new MathFunctions.Vector3(120, 120, 0), new MathFunctions.Vector3(200, 200, 0));

        bool isCollidingWall = false;

        // Extra juice
        public float rainbowColorF = 1.0f;
        public Color rainbow = new Color();

        public void Init()
        {
            GetScores();

            //TODO: SET UP THE WALLS AROUND THE MAP - DONE BUT ALSO MAKE THE PLAYER COLLIDE WITH THEM
            
            // Add the holders and tank objects to the Hierarchy.
            Hierarchy.Add(wallHolder);
            Hierarchy.Add(projectileHolder);
            Hierarchy.Add(targetHolder);
            Hierarchy.Add(destructableHolder);
            Hierarchy.Add(tankObject);

            // Add a specified amount of Targets to the game. Reccomended 2.
            for (int i = 0; i < 2; i++)
            {
                targetHolder.AddChild(new Target());
            }
           
            // Add a specified amount of Destructable Objects to the game. Reccomended 4.
            for (int i = 0; i < 4; i++)
            {
                destructableHolder.AddChild(new Destructable());
            }

            // Changing the positions of all the walls to fit the screen.
            Wall topWall = new Wall(true);
            Wall bottomWall = new Wall(true);
            Wall leftWall = new Wall(false);
            Wall RightWall = new Wall(false);

            topWall.SetPosition(40, 0);
            bottomWall.SetPosition(40, 440);
            leftWall.SetPosition(0, 0);
            RightWall.SetPosition(600, 0);

            // Adding walls to the wallholder SceneObject.
            wallHolder.AddChild(topWall);
            wallHolder.AddChild(bottomWall);
            wallHolder.AddChild(leftWall);
            wallHolder.AddChild(RightWall);

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
            Console.WriteLine("            CONTROLS                                ");
            Console.WriteLine("   W - Forwards                                     ");
            Console.WriteLine("   S - Backwards                                    ");
            Console.WriteLine("   A - Rotate Left                                  ");
            Console.WriteLine("   D - Rotate Right                                 ");
            Console.WriteLine("   Q - Rotate Barrel Left                           ");
            Console.WriteLine("   E - Rotate Barrel Right                          ");
            Console.WriteLine("   Spacebar - Fire Projectile                       ");
            Console.WriteLine("   I - (Debug) Disable game timer                   ");
            Console.WriteLine("   O - (Debug) Show Hitbox Corners                  ");
            Console.WriteLine("   P - (Debug) Change Collider Logic                ");
            Console.WriteLine("                                                    ");
            Console.WriteLine("   Shoot targets for points.                        ");
            Console.WriteLine("   Green box interacts with player and bullets.     ");
            Console.WriteLine("   Boxes are solid, you can break them with 3 shots.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.Cyan;
            #endregion
        }

        public void Shutdown()
        {
            SetScores();
            GetScores();

            bool okayToGo = false;
            while (!okayToGo)
            {
                BeginDrawing();

                ClearBackground(Color.RAYWHITE);

                wallHolder.Draw();

                // Displays only if the player played the game in full.
                if (remainingTime <= 0)
                {
                    DrawText("Time's Up!", 240, (int)(GetScreenHeight() / 4.5f), 32, Color.RED);
                }

                // Changes text upon player getting a new highscore.
                if (newHighscore)
                {
                    DrawText("NEW HIGHSCORE!", 220, GetScreenHeight() / 3, 24, rainbow);
                }
                else
                {
                    DrawText("Highscores:", 255, GetScreenHeight() / 3, 24, Color.GRAY);
                }

                // Displays leaderboard.
                // If the player got a new highscore, it will be highlighted.
                for (int i = 0; i < highscores.Length; i++)
                {
                    Color hsColor;
                    if (newScorePlace == i)
                    {
                        hsColor = rainbow;
                    }
                    else
                    {
                        hsColor = Color.DARKGRAY;
                    }

                    DrawText($"{i + 1}: {highscores[i]}", 300, 190 + (25 * i), 20, hsColor);
                }

                DrawText("Press space to exit.", 245, 270, 16, Color.GRAY);

                EndDrawing();

                if (IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    okayToGo = true;
                }

                UpdateMin();
            }
        }

        /// <summary>
        /// "Update Minimal"; A version of update that updates only time and the rainbow text.
        /// To be used only on the ending screen.
        /// </summary>
        public void UpdateMin()
        {
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

            // Updates the rainbow color every frame!
            rainbowColorF++;
            if (rainbowColorF <= 0 || rainbowColorF >= 360) rainbowColorF = 0;
            rainbow = ColorFromHSV(new Vector3(rainbowColorF, 1, 1));

            // In-game timer.
            // If the infinite time cheat is on, time will never advance. Once time is
            // resumed, the game will likely instantly end. Only intended for testing.
            if (!infiniteTime)
            {
                remainingTime = 45 - gameTime.Seconds; // THIS SHOULD BE 45
            }
            else
            {
                remainingTime = 1;
            }

            // Clamped to prevent the player from seeing "Time Remaining: [negitive number]".
            if (remainingTime <= 0)
            {
                remainingTime = 0;
            }
            #endregion

            #region Player Input
            lastPlayerTransform.Set(tankObject.GlobalTransform);

            // Player movement is restricted when colliding with a wall.
            // They can still move their turret and fire however.
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
                playerFacing = new MathFunctions.Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) * deltaTime * playerSpeed;
                tankObject.Translate(playerFacing.x, playerFacing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S) && !isCollidingWall)
            {
                playerFacing = new MathFunctions.Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) * deltaTime * -playerSpeed;
                tankObject.Translate(playerFacing.x, playerFacing.y);
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
                // Create a new projectile going the direction of the turret's rotation.
                Projectile temp = new Projectile(turretObject.GlobalTransform.m5, -turretObject.GlobalTransform.m4);

                // Set the position to be near the end of the turret's barrel upon spawning.
                temp.SetPosition(turretObject.GlobalTransform.m7 + (turretObject.GlobalTransform.m5 * 30), turretObject.GlobalTransform.m8 + (-turretObject.GlobalTransform.m4 * 30));

                // Add it to the projectile holder.
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
            if (IsKeyPressed(KeyboardKey.KEY_I))
            {
                infiniteTime = !infiniteTime;
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
            // Check to see if the projectile acually needs to be deleted first.
            for (int i = 0; i < projectileHolder.GetChildCount(); i++)
            {
                if (projectileHolder.GetChild(i).removeMe)
                {
                    projectileHolder.RemoveChild(projectileHolder.GetChild(i));
                }
            }
            #endregion

            #region Collision Logic
            // PLAYER
            // Checking if the player is hitting any of the destructable objects.
            // This limits movement and pushes the player out of the collision box.
            for (int i = 0; i < destructableHolder.GetChildCount(); i++)
            {
                Destructable temp = (Destructable)destructableHolder.GetChild(i);

                if (playerCollider.Overlaps(temp.destCollider) && temp.destHP > 0)
                {
                    isCollidingWall = true;

                    tankObject.SetPosition(lastPlayerTransform.m7, lastPlayerTransform.m8);

                    // Break here to prevent any more checks setting isCollidingWall to false when you're hitting at least one object.
                    break;
                }
                else
                {
                    isCollidingWall = false;
                }
            }

            // Checking for the player hitting any of the walls.
            // This limits movement and pushes the player out of the collision box.
            for (int i = 0; i < wallHolder.GetChildCount(); i++)
            {
                Wall temp = (Wall)wallHolder.GetChild(i);

                if (playerCollider.Overlaps(temp.wallCollider))
                {
                    isCollidingWall = true;

                    tankObject.SetPosition(lastPlayerTransform.m7, lastPlayerTransform.m8);

                    // Break here to prevent any more checks setting isCollidingWall to false when you're hitting at least one object.
                    break;
                }
                else
                {
                    isCollidingWall = false;
                }
            }

            // Checking to see if the player is in the red/green box.
            // If so, the box will turn red.
            if (boxCollider.Overlaps(playerCollider))
            {
                boxColor = Color.RED;
            }
            else
            {
                boxColor = Color.GREEN;
            }

            // PROJECTILES
            for (int i = 0; i < projectileHolder.GetChildCount(); i++)
            {
                // Grab a Projectile from the holder
                Projectile temp = (Projectile)projectileHolder.GetChild(i);

                // Check against the Destructable items in the holder.
                // This takes away 1 HP from the itme and discards the projectile.
                for (int k = 0; k < destructableHolder.GetChildCount(); k++)
                {
                    Destructable tempDest = (Destructable)destructableHolder.GetChild(k);

                    if (temp.projectileCollider.Overlaps(tempDest.destCollider) && tempDest.destHP > 0)
                    {
                        projectileHolder.RemoveChild(temp);
                        tempDest.destHP--;
                    }
                }
                
                // Check against the Targets in the holder.
                // This will give the player a point and respawn the target elsewhere.
                for (int j = 0; j < targetHolder.GetChildCount(); j++)
                {
                    Target temptarget = (Target)targetHolder.GetChild(j);

                    if (temp.projectileCollider.Overlaps(temptarget.targetCollider))
                    {
                        temptarget.Respawn();
                        score++;
                        projectileHolder.RemoveChild(temp);
                    }
                }

                // Check to see if the projectile is in the green/red box.
                // But first- if the player already is in there, we won't check this.
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

            #endregion
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.RAYWHITE);



            // Green/red square
            DrawRectangle(120, 120, 80, 80, boxColor);

            if (showHitboxCorners)
            {
                Color cornerColor = new Color();

                // For loops a finite 4 times- once for each corner.
                // Sphere colliders are ommited from this since their hitbox is their size.
                for (int i = 0; i < 4; i++)
                {
                    // This will create a checker pattern, purple being min/max.
                    if (i % 2 == 0)
                    {
                        cornerColor = Color.GREEN;
                    }
                    else
                    {
                        cornerColor = Color.DARKPURPLE;
                    }

                    // Player's collider
                    DrawCircle((int)playerCollider.Corners()[i].x, (int)playerCollider.Corners()[i].y, 6, cornerColor);

                    // Red/green box's collider
                    DrawCircle((int)boxCollider.Corners()[i].x, (int)boxCollider.Corners()[i].y, 6, cornerColor);

                    // Destructable items colliders
                    for (int j = 0; j < destructableHolder.GetChildCount(); j++)
                    {
                        Destructable temp = (Destructable)destructableHolder.GetChild(j);

                        if (temp.destHP > 0)
                        {
                            DrawCircle((int)temp.destCollider.Corners()[i].x, (int)temp.destCollider.Corners()[i].y, 6, cornerColor);
                        }
                    }

                    // Walls Colliders
                    for (int j = 0; j < wallHolder.GetChildCount(); j++)
                    {
                        Wall temp = (Wall)wallHolder.GetChild(j);

                        DrawCircle((int)temp.wallCollider.Corners()[i].x, (int)temp.wallCollider.Corners()[i].y, 6, cornerColor);
                    }
                }
            }

            foreach (SceneObject s in Hierarchy)
            {
                s.Draw();
            }

            // TEXT SHOULD BE DRAWN ABOVE ALL OTHER ITEMS; PLACE TEXT BELOW HERE

            DrawText($"FPS: {fps.ToString()}", 10, 10, 12, rainbow);
            if (showHitboxCorners)
            {
                DrawText("HITBOX VIEW", 60, 10, 12, rainbow);
            }

            DrawText($"Time Left: {remainingTime}", 245, 20, 18, Color.RED);

            DrawText("SCORE:", 540, 10, 14, Color.DARKGRAY);
            DrawText($"{score}", 595, 10, 14, rainbow);

            DrawText("Highscores:", 10, 400, 20, rainbow);
            for (int i = 0; i < highscores.Length; i++)
            {
                DrawText($"{i + 1}: {highscores[i]}", 20, 425 + (15 * i), 14, Color.DARKGRAY);
            }

            DrawText("Press P to toggle hitbox logic.", 350, 460, 12, Color.DARKGRAY);
            if (collisionToggle)
            {
                DrawText("Transforming", 540, 460, 12, rainbow);
            }
            else
            {
                DrawText("Static", 565, 460, 12, rainbow);
            }

            EndDrawing();
        }

        /// <summary>
        /// Reads the highscore file and stores them in an array for usein displaying and editing.
        /// </summary>
        public void GetScores()
        {
            StreamReader reader = new StreamReader(highscorePath);
            for (int i = 0; i < highscores.Length; i++)
            {
                int.TryParse(reader.ReadLine(), out highscores[i]);
            }
            reader.Close();
        }

        /// <summary>
        /// Checks the score over the last play session against the values of the previous highscores.
        /// Accordingly updates scores.
        /// </summary>
        public void SetScores()
        {
            StreamWriter writer = new StreamWriter(highscorePath);
            if (score > highscores[0])
            {
                // 1st
                // Record Scores
                writer.WriteLine(score);
                writer.WriteLine(highscores[0]);
                writer.WriteLine(highscores[1]);
                newHighscore = true;
                newScorePlace = 0;
            }
            else if (score < highscores[0] && score > highscores[1])
            {
                // 2nd
                // Record Scores
                writer.WriteLine(highscores[0]);
                writer.WriteLine(score);
                writer.WriteLine(highscores[1]);
                newHighscore = true;
                newScorePlace = 1;
            }
            else if (score < highscores[0] && score < highscores[1] && score > highscores[2])
            {
                // 3rd
                // Record Scores
                writer.WriteLine(highscores[0]);
                writer.WriteLine(highscores[1]);
                writer.WriteLine(score);
                newHighscore = true;
                newScorePlace = 2;
            }
            else
            {
                // None Place with Left Fail
                writer.WriteLine(highscores[0]);
                writer.WriteLine(highscores[1]);
                writer.WriteLine(highscores[2]);
            }
            writer.Close();
        }
    }
}
