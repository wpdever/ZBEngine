using System;
using System.Collections.Generic;
using System.Text;

namespace ZBEngine
{
    public enum LogType
    {
        //
        // 摘要:
        //     ///
        //     LogType used for Errors.
        //     ///
        Error = 0,
        //
        // 摘要:
        //     ///
        //     LogType used for Asserts. (These could also indicate an error inside Unity itself.)
        //     ///
        Assert = 1,
        //
        // 摘要:
        //     ///
        //     LogType used for Warnings.
        //     ///
        Warning = 2,
        //
        // 摘要:
        //     ///
        //     LogType used for regular log messages.
        //     ///
        Log = 3,
        //
        // 摘要:
        //     ///
        //     LogType used for Exceptions.
        //     ///
        Exception = 4
    }
}
