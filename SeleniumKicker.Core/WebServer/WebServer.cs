using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SeleniumKicker.Core.WebServer
{
  public class WebServer
  {
    private readonly HttpListener _listener = new HttpListener();
    private readonly Func<HttpListenerRequest, byte[]> _responderMethod;

    private WebServer(IReadOnlyCollection<string> prefixes, Func<HttpListenerRequest, byte[]> method)
    {
      if (!HttpListener.IsSupported)
        throw new NotSupportedException(
            "Needs Windows XP SP2, Server 2003 or later.");

      // URI prefixes are required, for example 
      // "http://localhost:8080/index/".
      if (prefixes == null || prefixes.Count == 0)
        throw new ArgumentException("prefixes");
      foreach (var s in prefixes)
        _listener.Prefixes.Add(s);

      _responderMethod = method ?? throw new ArgumentException("method");
      _listener.Start();
    }

    public WebServer(Func<HttpListenerRequest, byte[]> method, params string[] prefixes)
        : this(prefixes, method) { }

    public void Run()
    {
      ThreadPool.QueueUserWorkItem((o) =>
      {
        
        try
        {
          while (_listener.IsListening)
          {
            ThreadPool.QueueUserWorkItem((c) =>
                  {
                    var ctx = c as HttpListenerContext;
                    try
                    {
                      // ReSharper disable once PossibleNullReferenceException
                      var buf = _responderMethod(ctx.Request);
                      ctx.Response.ContentLength64 = buf.Length;
                      ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                    }
                    catch
                    {
                      // ignored
                    } // suppress any exceptions
                    finally
                    {
                      // always close the stream
                      // ReSharper disable once PossibleNullReferenceException
                      ctx.Response.OutputStream.Close();
                    }
                  }, _listener.GetContext());
          }
        }
        catch
        {
          // ignored
        } // suppress any exceptions
      });
    }
  }
}