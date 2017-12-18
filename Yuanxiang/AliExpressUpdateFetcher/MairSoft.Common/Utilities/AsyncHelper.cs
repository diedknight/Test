//===============================================================================
// 
// Copyright (c) 2005 Francis Mair (frank@mair.net.nz)
//
// Description:
// This class provides a FireAndForget method that facilitates calling an arbitrary 
// method asynchronously without worrying about or waiting for the return value. 
// This simplifies things since the SDK requires us to always call EndInvoke after 
// the asynchronous call completes.
//
// The usage model is along these lines:
// (example assumes the existence of void SomeMethod(string, double).
//
// SomeDelegate sd = new SomeDelegate(SomeMethod);
// FAsyncHelper.FireAndForget(sd, "pi", 3.1415927);
//
// Note - there's an extra (IMO) level of indirection in the following code that 
// I don't think should be necessary.  The goal is to call Delegate.DynamicInvoke 
// using BeginInvoke. Using BeginInvoke gets us asynchrony, using DynamicInvoke
// makes this generically reusable for any delegate.  However, when I call 
// DynamicInvoke directly I get an undetailed execution engine fatal error.  
// When I use BeginInvoke to call a shim method that just turns around and calls
// DynamicInvoke, things work.  Strange (and consistent on the 1.0 and 1.1 runtimes).
//
//===============================================================================

using System;

namespace MairSoft.Common.Utilities
{
	/// <summary>
	/// Summary description for FAsyncHelper.
	/// </summary>
    public class AsyncHelper
    {
        /// <summary>
        /// Delegate to our shim process
        /// </summary>
        delegate void DynamicInvokeShimProc(Delegate d,object[] args);

        /// <summary>
        /// Static delegate to encapsulate our shim method
        /// </summary>
        static DynamicInvokeShimProc dynamicInvokeShim = new DynamicInvokeShimProc(DynamicInvokeShim);

        /// <summary>
        /// Static callback which is called when shim method completes, and is used
        /// for ensuring that we call EndInvoke
        /// </summary>
        static AsyncCallback dynamicInvokeDone = new AsyncCallback(DynamicInvokeDone);

        /// <summary>
        /// This is the method that you need to call if you want fire and forget semantics
        /// on your delegate
        /// </summary>
        /// <param name="d"></param>
        /// <param name="args"></param>
        public static void FireAndForget(Delegate d,params object[] args)
        {
            dynamicInvokeShim.BeginInvoke(d,args,dynamicInvokeDone,null);
        }

        /// <summary>
        /// This is our static method which is called asychronously and which actually
        /// invokes the required method as specified in the passed delegate
        /// </summary>
        /// <param name="d"></param>
        /// <param name="args"></param>
        static void DynamicInvokeShim(Delegate d,object[] args)
        {
            d.DynamicInvoke(args);
        }

        /// <summary>
        /// Have added this callback so that we can ensure that EndInvoke is always
        /// called, as per the SDK:
        /// "CAUTION: Always call EndInvoke after your asynchronous call completes."
        /// </summary>
        /// <param name="ar"></param>
        static void DynamicInvokeDone(IAsyncResult ar)
        {
            dynamicInvokeShim.EndInvoke(ar);
        }
    }
}

//===============================================================================
