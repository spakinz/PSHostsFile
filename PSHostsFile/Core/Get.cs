﻿using System.Collections.Generic;
using System.IO;

namespace PSHostsFile.Core
{
    public class Get
    {
        public IEnumerable<HostsFileEntry> LoadFromStream(StreamReader input)
        {
            List<HostsFileEntry> results = new List<HostsFileEntry>();

            string line;

            while((line = input.ReadLine()) != null)
            {
                if (! HostsFileUtil.IsLineAHostFilesEntry(line))
                    continue;

                results.Add(HostsFileUtil.GetHostsFileEntry(line));
            }

            return results;
        }

        public IEnumerable<HostsFileEntry> LoadFromHostsFiles()
        {
            string hostsPath = GetHostsPath();

            using(var file = new FileStream(hostsPath, FileMode.Open, FileAccess.Read))
            {
                return LoadFromStream(new StreamReader(file));
            }
        }

        public static string GetHostsPath()
        {
            var systemPath = System.Environment.GetEnvironmentVariable("SystemRoot");
            var hostsPath = System.IO.Path.Combine(systemPath, "system32\\drivers\\etc\\hosts");

            if (!File.Exists(hostsPath))
                throw new FileNotFoundException("Hosts file not found at expected location.");
            return hostsPath;
        }
    }
}