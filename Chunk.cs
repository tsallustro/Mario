using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Chunks
{
    class Chunk
    {
        private List<IGameObject> objects;

        public Chunk()
        {
            this.objects = new List<IGameObject>();
        }

        public Chunk(List<IGameObject> objects)
        {
            this.objects = objects;
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
