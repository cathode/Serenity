using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Serenity.Data
{
    public static class DataExtensions
    {
        /// <summary>
        /// Ensures that the current <see cref="DbConnection"/> is in an open state.
        /// </summary>
        /// <param name="connection"></param>
        public static void EnsureOpen(this DbConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
    }
}
