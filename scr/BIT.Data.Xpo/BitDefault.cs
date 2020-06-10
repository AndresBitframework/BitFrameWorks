﻿
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BIT.Data
{
    public static class BitFrameworksDefault
    {
        public static void RegisterFunction(Session session, IRegistableFunction Function)
        {
            if(!Function.Exist(session))
            {
                Function.Register(session);
            }
        }
    }
}
