using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Chunks
{
    public class Chunk
    {
        private List<IGameObject> objects;

        // We need the top 5 and bottom 5 rows as binary matrices
        private int[,] highRows;
        private int[,] lowRows;

        public Chunk()
        {
            this.objects = new List<IGameObject>();
            highRows = new int[5, 50];
            lowRows = new int[7, 50];
        }

        public Chunk(List<IGameObject> objects)
        {
            this.objects = objects;
            highRows = new int[5, 50];
            lowRows = new int[7, 50];
        }

        public Chunk(List<IGameObject> objects, int[,] highRows, int[,] lowRows)
        {
            this.objects = objects;
            this.highRows = highRows;
            this.lowRows = lowRows;
        }
 
        public int[,] GetHighRows()
        {
            return highRows;
        }

        public int[,] GetLowRows()
        {
            return lowRows;
        }

        public List<IGameObject> GetObjects()
        {
            return objects;
        }

        public void AddObject(IGameObject obj)
        {
            objects.Add(obj);
        }
    }
}
