﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PSHostsFiles;

namespace PSHostsFilesTest
{
    [TestFixture]
    public class RemoveTest : ReadWriteScenario
    {
        [Test]
        public void can_remove_an_entry()
        {
            var expectedString = SampleHostsFile.AsString.Replace("\r\n192.168.1.1         anotherserver.net", "");
            Assert.That(expectedString, Is.Not.EqualTo(SampleHostsFile.AsString), "verify replace setup actualy did something.");

            var hostsFile = SampleHostsFile.AsStreamReader();

            var result = new MemoryStream();
            var resultStream = new StreamWriter(result);

            var sut = new Remove();

            sut.RemoveFromStream("anotherserver.net", hostsFile, resultStream);

            result.Seek(0, SeekOrigin.Begin);
            Assert.That(new StreamReader(result).ReadToEnd(), Is.EqualTo(expectedString));
        }

        [Test]
        public void can_read_and_write_to_the_same_file()
        {
            string filename = GetFileWithContents(@"
127.0.0.1           localhost
10.90.82.100        somehost

", Encoding.UTF8);

            var sut = new Remove();
         
            sut.RemoveFromFile("somehost", filename);

            Encoding encodingUsed;
            string fileContents = ReadFileContents(filename, out encodingUsed);

            Assert.That(fileContents, Is.EqualTo(@"
127.0.0.1           localhost

"));
        }
    }
}