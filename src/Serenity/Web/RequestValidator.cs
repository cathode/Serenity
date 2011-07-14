using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web
{
    public abstract class RequestValidator
    {
        /// <summary>
        /// Validates a <see cref="Request"/> and determines if it meets the requirements of the HTTP specification.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <remarks>
        /// If the <see cref="Request"/> fails validation, the <see cref="Response"/> is modified with relevant information that notifies the client
        /// about the failed validation.
        /// </remarks>
        public abstract bool ValidateRequest(Request request, Response response);
    }
}
