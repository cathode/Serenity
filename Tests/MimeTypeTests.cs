﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Serenity.Core;

namespace Tests
{
    [TestFixture]
    public class MimeTypeTests
    {
        [Test]
        public void ParseShouldSucceedWithValidInput()
        {
            string[] inputs = {
                                  "text/plain",
                                  "application/xml",
                                  "image/png",
                                  "audio/vnd.rn-realaudio",
                                  "application/xhtml+xml",
                                  "multipart/form-data"
                              };
            MimeType[] outputs = {
                                     MimeType.TextPlain,
                                     MimeType.ApplicationXml,
                                     MimeType.ImagePng,
                                     MimeType.AudioVendorRNRealAudio,
                                     MimeType.ApplicationXhtmlPlusXml,
                                     MimeType.MultipartFormData
                                 };

            for (int i = 0; i < inputs.Length; i++)
            {
                try
                {
                    var parsed = MimeType.Parse(inputs[i]);

                    Assert.AreEqual(outputs[i], parsed);
                }
                catch (Exception ex)
                {
                    Assert.Fail(string.Format("Failed to parse mimetype input: \"{0}\". Exception: {1}", inputs[i], ex.ToString()));
                }
            }
        }
    }
}
