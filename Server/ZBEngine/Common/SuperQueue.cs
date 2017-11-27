using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ZBEngine
{
    //ConcurrentStack的 目的 是为了 取出队列列表的时候，可以保证先后顺序
    public class SuperQueue<T> : ConcurrentQueue<T>
    {
        Queue<T> q = new Queue<T>();

        //集体取是为了 减少 队列的 原子 操作 所带来的竞争损耗
        public Queue<T> GetQueue()
        {
            if (IsEmpty) return null;//用 IsEmpty 比 Count 效率 更高
            q.Clear();
            while (TryDequeue(out T result))
            {
                q.Enqueue(result);
            }
            return q;
        }
    }
}
