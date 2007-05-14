using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity.Hdf;

namespace Server.OperatingModes
{
    internal static class DebugMode
    {
        private static void Fib(int n)
        {
            ulong a = 0;
            ulong b = 1;
            ulong c = 0;

            if (n != 1)
            {
                for (int i = 1; i <= n; i++)
                {
                    Console.WriteLine(a);
                    c = a + b;
                    a = b;
                    b = c;
                }
                
            }
        }
        internal static void Run()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                HdfDataset dataset = new HdfDataset();
                dataset.Add(dataset.CreateElement("name", "value"));
                dataset["name"].Add(dataset.CreateElement("child"));
                dataset["name"]["child"].Add(dataset.CreateElement("subchild", "value"));

                HdfWriterSettings settings = new HdfWriterSettings();
                settings.Format = HdfFormat.Nested;
                HdfWriter writer = new HdfWriter(settings);
                writer.Write(stream, dataset);

                Console.Write(Encoding.UTF8.GetString(stream.ToArray()));
                Console.ReadLine();
            }
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
