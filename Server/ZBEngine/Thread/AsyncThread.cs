using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ZBEngine
{
    public static class AsyncThread
    {
        public static void Post(Action func)
        {
            if (func == null)
                return;
            Task.Factory.StartNew(func);//这个 函数可以利用多核的优势，根据核心数来来启用相应的，线程池数量
        }
    }
}
