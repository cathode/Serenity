using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web
{
    /// <summary>
    /// Provides a unified way for status code error messages to be displayed to the client.
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Handles a supplied StatusCode.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="context"></param>
        /// <returns>True if an error response was generated,
        /// or false if no error response necessary.</returns>
        public static bool Handle(StatusCode code, CommonContext context)
        {
            context.Response.Status = code;
            bool isError = true;
            switch (code.Code)
            {
                case 400:
                    context.Response.Write("Error: 400 Bad Request\r\nThe request sent by your browser was incorrectly formed or contained invalid data. This may indicate an error with your browser software.");
                    break;
                case 401:
                    context.Response.Write("Error: 401 Unauthorized\r\n");
                    break;
                case 403:
                    context.Response.Write("Error: 403 Forbidden\r\n");
                    break;
                case 404:
                    context.Response.Write("Error: 404 Not Found\r\nThe resource you requested was not found on the server.");
                    break;
                case 405:
                    context.Response.Write("Error: 405 Method Not Allowed\r\nYour browser sent a method that was unrecognized or not part of the HTTP standard.");
                    break;

                default:
                    isError = false;
                    break;
            }

            return isError;
        }
    }
}