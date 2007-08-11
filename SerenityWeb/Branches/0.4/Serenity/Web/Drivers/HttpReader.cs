using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity.IO;

namespace Serenity.Web.Drivers
{
	public sealed class HttpReader : Reader<CommonContext>
	{
		public HttpReader(WebDriver driver)
		{

		}
		private WebDriver driver;
		public override CommonContext Read(byte[] buffer, out bool result)
		{
			CommonContext context = new CommonContext(this.driver);

			string requestContent = Encoding.ASCII.GetString(buffer);
			int headerSize = requestContent.IndexOf("\r\n\r\n");

			if (headerSize != -1)
			{
				headerSize += 4;

				int indexOf = requestContent.IndexOf("\r\n");
				string line = requestContent.Substring(0, indexOf);
				requestContent = requestContent.Substring(indexOf + 2);
				string requestUri = "/";
				string[] methodParts = line.Split(' ');

				//First line must be "<METHOD> <URI> HTTP/<VERSION>" which translates to 3 elements
				//when split by the space char.
				if (methodParts.Length == 3)
				{
					//Get down to business.
					switch (methodParts[0])
					{
						//WS: Normal HTTP methods:
						case "HEAD":
						case "GET":
						case "POST":
						case "PUT":
						case "DELETE":
						case "TRACE":
						case "OPTIONS":
						case "CONNECT":
						//WS: WebDAV extension methods:
						case "PROPFIND":
						case "PROPPATCH":
						case "MKCOL":
						case "COPY":
						case "MOVE":
						case "LOCK":
						case "UNLOCK":
							context.Request.Method = methodParts[0];
							break;

						default:
							//WS: We need to generate an error here if the method is not supported.
							ErrorHandler.Handle(context, StatusCode.Http405MethodNotAllowed, methodParts[0]);
							result = true;
							return context;
					}
					//Request URI is the "middle"
					requestUri = methodParts[1];

					switch (methodParts[2])
					{
						case "HTTP/0.9":
							context.ProtocolVersion = new Version(0, 9);
							break;
						case "HTTP/1.0":
							context.ProtocolVersion = new Version(1, 0);
							break;
						case "HTTP/1.1":
							context.ProtocolVersion = new Version(1, 1);
							break;

						default:
							ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "An invalid HTTP version was detected");
							result = true;
							return context;
					}
				}
				else
				{
					ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "The first line of the request was invalid");
					result = true;
					return context;
				}
				indexOf = requestContent.IndexOf("\r\n");
				while (indexOf != -1)
				{
					line = requestContent.Substring(0, indexOf);
					requestContent = requestContent.Substring(indexOf + 2);
					if (line.Length > 0)
					{
						int n = line.IndexOf(':');
						if (n != -1)
						{
							context.Request.Headers.Add(line.Substring(0, n), line.Substring(n + 2));
						}
					}
					indexOf = requestContent.IndexOf("\r\n");
				}
				//HTTP 1.1 states that requests must define a "Host" header even if an absolute
				//request URI is requested.
				if (context.Request.Headers.Contains("Host"))
				{
					//HTTP 1.1 and later allows a relative URI or an absolute URI to be requested.
					if (requestUri.StartsWith("/"))
					{
						//relative requesturi
						context.Request.Url = new Uri("http://"
							+ context.Request.Headers["Host"].PrimaryValue + requestUri);
					}
					else if (requestUri.StartsWith("http://") || requestUri.StartsWith("https://"))
					{
						//absolute requesturi
						context.Request.Url = new Uri(requestUri);
					}
					else
					{
						//invalid url scheme for HTTP.
						ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "Invalid request URI scheme");
						result = true;
						return context;
					}
				}
				else
				{
					//Request is invalid because it doesnt have a Host header.
					ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "No Host header included");
					result = true;
					return context;
				}

				bool hasContentLength = context.Request.Headers.Contains("Content-Length");
				bool hasTransferEncoding = context.Request.Headers.Contains("Transfer-Encoding");
				bool hasBody = hasContentLength | hasTransferEncoding;

				if (hasBody)
				{
					//Check if the client sent the Content-Type header, which is required if
					//a message body is included with the request. 
					if (context.Request.Headers.Contains("Content-Type"))
					{
						//Content-Length and Transfer-Encoding headers can't coexist.
						if (hasContentLength && !hasTransferEncoding)
						{
							ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
							result = true;
							return context;
						}
						else if (hasTransferEncoding && !hasContentLength)
						{
							ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
							result = true;
							return context;
						}
						else
						{
							ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "Content-Length and Transfer-Encoding headers cannot exist in the same request.");
							result = true;
							return context;
						}
					}
					else
					{
						ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "No Content-Type header included with request that includes a message body.");
					}
				}
				result = true;
				return context;
			}
			result = false;
			return null;
		}

		public override CommonContext Read(Stream stream, out bool result)
		{
			byte[] buffer = new byte[stream.Length];
			stream.Read(buffer, 0, buffer.Length);

			return this.Read(buffer, out result);
		}
	}
}
