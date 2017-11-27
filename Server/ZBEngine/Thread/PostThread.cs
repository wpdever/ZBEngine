using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ZBEngine
{
    public delegate void VoidDele();

    public abstract class PostThread
    {
        public int ThreadID = -1;
        public int FPS = 0;//当前线程的 帧数 空逻辑的时候 帧数 接近 1000，当FPS <=10 的时候说明 当前服务器非常繁忙

        private SuperQueue<VoidDele> event_que = new SuperQueue<VoidDele>();

        protected abstract void Update();

        protected abstract void LateUpdate();

        public void Start()
        {
            Task.Factory.StartNew(DoWhile, TaskCreationOptions.LongRunning);
        }

        public void Post(VoidDele func, bool NextFrameExcute = false)
        {
            if (func == null)
                return;
            if (NextFrameExcute)
            {
                event_que.Enqueue(func);
                return;
            }
            if (CheckThread())
            {
                func?.Invoke();
                return;
            }
            event_que.Enqueue(func);
        }

        public bool CheckThread()
        {
            return ThreadID == Thread.CurrentThread.ManagedThreadId;
        }

        private void DoWhile()
        {
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            int ExecutCount = 0;
            int StartTiks = Environment.TickCount;
            while (true)
            {
                try { Update(); }
                catch (Exception e)
                {
                    Debug.LogError(e.Message); Debug.LogException(e.StackTrace);
                }
                Queue<VoidDele> q = event_que.GetQueue();//取出所有的 事件队列
                if (q != null)
                {
                    foreach (VoidDele func in q)
                    {
                        try { func?.Invoke(); } catch (Exception e) { Debug.LogError(e.Message); Debug.LogException(e.StackTrace); }
                    }
                }
                else
                {
                    Thread.Sleep(1);//为了节约 CPU 的开销,这样的话空闲的时候cpu 会非常低
                }
                ExecutCount++;
                //计算Result的逻辑 和 TimeTools.GetDurTickCount一样的，只是为了减少一次函数调用的开销，所以copy 了一份实现 过来
                int curTicount = Environment.TickCount;
                int result = curTicount - StartTiks;
                if (result < 0)//因为Environment.TickCount达到 int.maxValue的临界点的时候 会产生一次 负数，所以 需要规避一下
                {
                    result = StartTiks - curTicount;
                }
                if (result > 1000)//1s
                {
                    //LateUpdate 平均每秒 执行 1次，不需要执行次数太多
                    try { LateUpdate(); } catch (Exception e) { Debug.LogError(e.Message); Debug.LogException(e.StackTrace); }
                    FPS = ExecutCount;
                    ExecutCount = 0;
                    //Debug.Log("LogicThead FPS : " + FPS);
                    StartTiks = Environment.TickCount;
                }
            }
        }
    }
}
