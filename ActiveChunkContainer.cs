using System;
using System.Collections.Generic;
using System.Text;
using Chunks;
using GameObjects;

namespace ChunkContainer
{
    public class ActiveChunkContainer
    {
        Queue<Chunk> activeChunks;
        List<IGameObject> objects; // This is the list of objects we update and draw in MarioGame

        public ActiveChunkContainer()
        {
            activeChunks = new Queue<Chunk>();
            objects = new List<IGameObject>();
        }

        public void AddChunk(Chunk chunk)
        {
            activeChunks.Enqueue(chunk);
            objects.AddRange(chunk.GetObjects());
        }

        public void RemoveChunk()
        {
            Chunk removedChunk = activeChunks.Dequeue();

            foreach (IGameObject obj in removedChunk.GetObjects())
            {
                obj.SetQueuedForDeletion(true);
                objects.Remove(obj);
            }
        }

        // Only use this method to add Mario to list
        public void AddObject(IGameObject obj)
        {
            objects.Add(obj);
        }

        public List<IGameObject> GetObjects()
        {
            return objects;
        }
    }
}
