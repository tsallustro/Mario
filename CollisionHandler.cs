using System;
using System.Collections.Generic;
using GameObjects;
using Microsoft.Xna.Framework;



namespace Collisions
{
    class CollisionHandler
    {

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
                            if (Math.Abs(obj2PosX - obj1PosX) < 8 + Math.Abs(obj1VelX * GameTime.ElapsedGameTime.TotalSeconds))       // Check to see if obj2 is close to obj1 X
                            {
                                //System.Diagnostics.Debug.WriteLine("XPos Close:" + obj1.GetType());

                                if (Math.Abs(obj2PosY - obj1PosY) < 8 + Math.Abs(obj1VelY * GameTime.ElapsedGameTime.TotalSeconds))   // Check to see if obj2 is close to obj1 Y
                                {
                                    System.Diagnostics.Debug.WriteLine("YPos Close:" + obj1.GetType());
                                    // Check Bottom Collision
                                    if (
                                        obj1AABB.Top < obj2AABB.Bottom &&
                                        obj1AABB.Bottom + obj1VelY > obj2AABB.Top &&
                                        obj1AABB.Right > obj2AABB.Left &&
                                        obj1AABB.Left < obj2AABB.Right)
                                    {
                                    }

                                    // Check Top Collision
                                    if (obj1AABB.Top - obj1VelY == obj2AABB.Bottom &&
                                        obj1AABB.Bottom > obj2AABB.Top &&
                                        obj1AABB.Right > obj2AABB.Left &&
                                        obj1AABB.Left < obj2AABB.Right)
                                    {

                                    }
                                    // Check Left Collision
                                    if (obj1AABB.Top > obj2AABB.Bottom &&
                                        obj1AABB.Bottom < obj2AABB.Top &&
                                        obj1AABB.Right > obj2AABB.Left &&
                                        obj1AABB.Left - obj1VelX < obj2AABB.Right)
                                    {
                                    }

                                    // Check Right Collision
                                    if (obj1AABB.Top > obj2AABB.Bottom &&
                                        obj1AABB.Bottom < obj2AABB.Top &&
                                        obj1AABB.Right + obj1VelX > obj2AABB.Left &&
                                        obj1AABB.Left < obj2AABB.Right)
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
