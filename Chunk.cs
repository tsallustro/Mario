using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Chunks
{
    class Chunk
    {
        private List<IGameObject> objects;
        private int highestRow;
        private int lowestRow;

        public Chunk()
        {
            this.objects = new List<IGameObject>();
        }

        public Chunk(List<IGameObject> objects)
        {
            this.objects = objects;
        }

        public Chunk(List<IGameObject> objects, int highestRow, int lowestRow)
        {
            this.objects = objects;
            this.highestRow = highestRow;
            this.lowestRow = lowestRow;
        }

        public List<IGameObject> GetObjects()
        {
            return objects;
        }

        public void SetHighestRow(int row)
        {
            highestRow = row;
        }

        public int GetHighestRow()
        {
            return highestRow;
        }

        public void SetLowestRow(int row)
        {
            lowestRow = row;
        }

        public int GetLowestRow()
        {
            return lowestRow;
        }

        public void AddObject(IGameObject obj)
        {
            objects.Add(obj);
        }
    }
}
