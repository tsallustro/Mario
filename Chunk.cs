using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Chunks
{
    class Chunk
    {
        private List<IGameObject> objects;

        // We need the top 5 and bottom 5 rows as binary matrices
        private int[,] highRows;
        private int[,] lowRows;

        private List<Chunk> compatibleChunks;

        public Chunk()
        {
            this.objects = new List<IGameObject>();
            highRows = new int[5, 50];
            lowRows = new int[5, 50];
        }

        public Chunk(List<IGameObject> objects)
        {
            this.objects = objects;
            highRows = new int[5, 50];
            lowRows = new int[5, 50];
        }

        public Chunk(List<IGameObject> objects, int[,] highRows, int[,] lowRows)
        {
            this.objects = objects;
            this.highRows = highRows;
            this.lowRows = lowRows;
        }

        public void SetCompatibleChunks(List<Chunk> compatibleChunks)
        {
            this.compatibleChunks = compatibleChunks;
        }

        public List<Chunk> GetCompatibleChunks()
        {
            return compatibleChunks;
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
