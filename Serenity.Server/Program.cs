/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Serenity.Web;
using System.Xml.Linq;
using Serenity.Net;
using System.Diagnostics;

namespace Serenity.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new HttpConnectionTester().RunTest();
            Console.WriteLine("Done. Press any key...");
            Console.ReadLine();

            return;
            Console.WriteLine("Serenity Console Mode, starting up...");
            WebServer server = new WebServer();
            server.LoadBuiltinApplications();
            server.LoadApplication(new PastebinApplication());
            server.Start();
            Console.WriteLine("Server shutting down. Press any key...");
            Console.ReadLine();
        }
    }
    internal class HttpConnectionTester : HttpConnection
    {
        internal HttpConnectionTester()
            : base(new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
        {
        }
        public void RunTest()
        {
            var testInput = "GET / HTTP/1.1\r\nHost: localhost\r\nConnection: keep-alive\r\nCache-Control: max-age=0\r\nUser-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\nAccept-Encoding: gzip,deflate,sdch\r\nAccept-Language: en-US,en;q=0.8\r\nAccept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.3\r\n\r\n";
            var buffer = Encoding.ASCII.GetBytes(testInput);
            Stopwatch sw = new Stopwatch();

            int count = 500000;
            int samples = 5;
            TimeSpan total = new TimeSpan(0);

            Console.WriteLine("{0} samples of {1} iterations each.", samples, count);
            for (int k = 0; k < samples; ++k)
            {
                sw.Reset();
                sw.Start();
                for (int i = 0, m = count; i < m; ++i)
                {
                    this.ProcessBufferContents(buffer);
                }
                sw.Stop();
                var elapsed = sw.Elapsed;
                total += elapsed;
                Console.WriteLine("{3}: {2} iterations in {0}ms, {1}ms per iteration (average).", elapsed.TotalMilliseconds, (elapsed.TotalMilliseconds / count), count, k);
            }
            Console.WriteLine("Averages: {0} iterations in {1}ms, {2}ms per iteration.", count, total.TotalMilliseconds / samples, (total.TotalMilliseconds / samples) / count);
        }
    }
    internal sealed class PastebinApplication : WebApplication
    {
        public override void InitializeResources()
        {
            this.ApplicationRoot.Name = "Pastebin";
            this.ApplicationRoot.AddChild(new ResourceGraphNode(new PostResource()));
            this.ApplicationRoot.AddChild(new ResourceGraphNode(new SubmitPostResource()));
        }

        public override void ProcessRequest(Request request, Response response)
        {
            throw new NotImplementedException();
        }
    }
    internal sealed class SubmitPostResource : Resource
    {
        internal SubmitPostResource()
        {
            this.Name = "SubmitPost";
            this.ContentType = MimeType.TextPlain;
        }
        public override void OnRequest(Request request, Response response)
        {
            base.OnRequest(request, response);

            foreach (var dat in request.RequestData)
                response.Write("Request data: " + dat.Name + "\n" + "Contents: " + dat.ReadAllText() + "\n\n");
        }
    }

    internal sealed class PostResource : Resource
    {
        internal PostResource()
        {
            this.Name = "Post";
            this.ContentType = MimeType.TextHtml;
        }
        public override void OnRequest(Request request, Response response)
        {
            base.OnRequest(request, response);

            response.Write(
@"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\'>
<html>
<head>
    <title>Pastebin - Post</title>
    <link href='http://assets3.pastie.org/stylesheets/display.css?1312786552' media='screen'
        rel='stylesheet' type='text/css' />
</head>
<body>
    <form action='SubmitPost' class='new_paste' enctype='multipart/form-data' id='new_paste'
    method='post'>
        <input id='lang' name='lang' type='hidden' />
    <p>
        <textarea class='pastebox' cols='40' id='paste_body' name='paste[body]' rows='22'
            tabindex='20'></textarea>
    </p>
    <input type='hidden' id='paste_authorization' name='paste[authorization]' value='booger'>
    <input type='hidden' name='key' value=''>
    <p class='private' id='private' style='display: none; float: left;'>
        <img src='/images/icons/16x16/locked.gif' />
        This paste will be private.
    </p>
    <div class='submit' style='margin-bottom: 0;'>
        <span style='margin-right: 1.7em'>
            <input name='paste[restricted]' type='hidden' value='0' /><input id='paste_restricted'
                name='paste[restricted]' onclick='toggle_private();' title='Mark paste private'
                type='checkbox' value='1' />
            <img src='/images/icons/16x16/locked.gif' style='vertical-align: middle' title='Click the checkbox to make this paste private' />
        </span>
        <input src='http://assets2.pastie.org/images/paste_button.png?1312786552' tabindex='30'
            type='image' />
    </div>
    <br style='clear: both' />
    </form>
</body>
</html>"
            );
        }
    }
}
