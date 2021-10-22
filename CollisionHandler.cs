﻿using System;
using System.Collections.Generic;
using GameObjects;
using Sprites;
using Microsoft.Xna.Framework;
using States;


namespace Collisions
{
    class CollisionHandler
    {
        //ints used for GameObject.Collision()
        public static readonly int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

        // Contructor
        public CollisionHandler()
        {
        }

        public void Update(GameTime GameTime, List<IGameObject> GameObjects)
        {
            Rectangle obj1AABB;     // AABB of parsed GameObject
            Rectangle obj2AABB;     // AABB of Potential collisionable GameObject with obj1
            foreach (GameObject obj1 in GameObjects)                        // Parsing through every GameObject
            {
                if (obj1.GetVelocity().X != 0 || obj1.GetVelocity().Y != 0)           // Object1 has velocity, Do check
                {
                    obj1AABB = obj1.GetAABB();

                    float obj1PosX = obj1AABB.Center.X;
                    float obj1PosY = obj1AABB.Center.Y;
                    float obj1VelX = obj1.GetVelocity().X;
                    float obj1VelY = obj1.GetVelocity().Y;
                    foreach (GameObject obj2 in GameObjects)
                    {
                        if (obj1 != obj2)
                        {
                            obj2AABB = obj2.GetAABB();
                            float obj2PosX = obj2AABB.Center.X;
                            float obj2PosY = obj2AABB.Center.Y;
                            // Determine if a check needs to be made based on location of GameObjects

                            // For some reason, left/right collisions do not work if this is uncommented. -Jesse

                            //if (Math.Abs(obj2PosX - obj1PosX) < 8 + Math.Abs(obj1VelX * GameTime.ElapsedGameTime.TotalSeconds))       // Check to see if obj2 is close to obj1 X
                            //{
                            //System.Diagnostics.Debug.WriteLine("XPos Close:" + obj1.GetType());

                            //if (Math.Abs(obj2PosY - obj1PosY) < 16 + Math.Abs(obj1VelY * GameTime.ElapsedGameTime.TotalSeconds))   // Check to see if obj2 is close to obj1 Y
                            //{
                            if (obj1.TopCollision(obj2))
                            {
                                obj1.Sprite.isCollided = true;
                                obj2.Sprite.isCollided = true;

                                /*
                                 * If object is a block, we must update it first so it can
                                 * accurately detect Mario's speed on collision
                                 */
                                if (obj2 is Block block)
                                {
                                    obj2.Collision(BOTTOM, obj1);
                                    obj1.Collision(TOP, obj2);
                                } else
                                {
                                    obj1.Collision(TOP, obj2);
                                    obj2.Collision(BOTTOM, obj1);
                                }
                            } else if (obj1.BottomCollision(obj2))
                            {
                                obj1.Sprite.isCollided = true;
                                obj2.Sprite.isCollided = true;
                                obj1.Collision(BOTTOM, obj2);
                                obj2.Collision(TOP, obj1);
                            } else if (obj1.LeftCollision(obj2))
                            {
                                obj1.Sprite.isCollided = true;
                                obj2.Sprite.isCollided = true;
                                obj1.Collision(LEFT, obj2);
                                obj2.Collision(RIGHT, obj1);
                            } else if (obj1.RightCollision(obj2))
                            {
                                obj1.Sprite.isCollided = true;
                                obj2.Sprite.isCollided = true;
                                obj1.Collision(RIGHT, obj2);
                                obj2.Collision(LEFT, obj1);
                            } else
                            {
                                obj1.Sprite.isCollided = false;
                                obj2.Sprite.isCollided = false;
                            }
                                //}
                            //}
                        }
                    }
                }
            }
        }
    }
}
