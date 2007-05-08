using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity.Hdf;

namespace Server.OperatingModes
{
    internal static class DebugMode
    {
        internal static void Run()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(@"title = test
groups
{
	Directories
	{
		columns
		{
			0 = 'Directory Name'
			1 = 'Last Modified'
		}
		rows
		{
			0.icon = folder.png
			0.0 = Icons
			0.1 = 2007-04-27T22:18:56
		}
	}
	Files
	{
		columns
		{
			0 = 'File Name'
			1 = 'File Size'
			2 = 'File Type'
			3 = 'Last Modified'
		}
		rows
		{
			0.icon = page_white.png
		}
	}
}")))
            {
                HdfReader reader = new HdfReader();
                HdfDataset dataset = reader.Read(stream);

                string first = HdfPath.Combine("groups.Directories", "columns", "0");
                string second = HdfPath.GetImmediateParent(first);
                foreach (string part in HdfPath.EnumeratePath(second))
                {
                    Console.WriteLine(part);
                }
            }
        }
    }
}
