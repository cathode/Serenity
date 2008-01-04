/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/

namespace Serenity.Web
{
    /// <summary>
    /// Provides a unified way for status code error messages to be displayed to the client.
    /// </summary>
    public static class ErrorHandler
    {
        #region Methods - Public
        public static bool Handle(CommonContext context, StatusCode code)
        {
            return ErrorHandler.Handle(context, code, null);
        }
        public static bool Handle(CommonContext context, StatusCode code, string message)
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

                case 406:
                case 407:
                case 408:
                case 409:
                case 410:
                    break;

                case 500:
                    context.Response.Write("Error: 500 Internal Server Error\r\nThe server encountered an error and was unable to process your request.");
                    break;
                case 501:
                    context.Response.Write("Error: 501 Not Implemented\r\nThe feature you requested is not available on the server.");
                    break;
                case 502:
                case 503:
                    break;

                default:
                    isError = false;
                    break;
            }

            if (isError && !string.IsNullOrEmpty(message))
            {
                context.Response.Write("\r\nAdditional information about this error:\r\n");
                context.Response.Write(message);
            }

            return isError;
        }
        #endregion
    }
}