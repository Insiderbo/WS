﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Trading_Bot.Exceptions
{
  public class InvalidAuthenticationException : Exception
  {
    #region Constants
    const string AUTHENTICATION = "Authentication";
    const string DEFAULT_API = "DEFAULT_API";
    const string DEFAULT_API_TEST = "DEFAULT_API_TEST";
    const string AUTH_KEY = "AUTH_KEY";
    const string AUTH_SECRET = "AUTH_SECRET";
    const string AUTH_PASSPHRASE = "AUTH_PASSPHRASE";
    const string AUTH_URL = "AUTH_URL";
    const string SOCKET_URL = "SOCKET_URL";
    const string VALUE = "value";
    #endregion
    #region Constructor
    /// <summary>
    /// Creates a default ".xml" file which provides the correct structure for the "AuthenticationConfig" Class to parse correctly.
    /// </summary>
    public InvalidAuthenticationException()
    {
      try
      {
        // Get the user's desktop location.
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        // Create a file name for the user's xml file.
        const string authenticationXmlFileName = "UserAuthentication.xml";
        string userFilePath = desktopPath + "\\" + authenticationXmlFileName;

        List<string> lines = new();
        lines.Add("<?xml version="+'\u0022' + "1.0" + '\u0022' + " encoding=" + '\u0022' + "utf-8" + '\u0022' + " ?>");
        lines.Add($"<{AUTHENTICATION}>");
        lines.Add($"</{AUTHENTICATION}>");
        // To make it a valid ".xml" file so the file knows how to encode/decode the file appropriately.
        File.WriteAllLines(userFilePath, lines);

        // To make sure the thread creating the file isn't still creating before we load it.
        Thread.Sleep(10);
        XmlDocument doc = new();
        doc.Load(userFilePath);

        // Now let's create an empty xml file for our user!
        XmlElement docElement = doc.DocumentElement;
        XmlElement defaultApi = doc.CreateElement(DEFAULT_API);
        XmlElement defaultApiTest = doc.CreateElement(DEFAULT_API_TEST);

        AppendAttributes(defaultApi);
        AppendAttributes(defaultApiTest);

        docElement.AppendChild(defaultApi);
        docElement.AppendChild(defaultApiTest);
        doc.AppendChild(docElement);


        doc.Save(userFilePath);
      }
      catch
      {
        // Swallow Exception
      }
    }
    #endregion

    #region Private
    /// <summary>
    /// If successful, return a list of the now added elements.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    private List<XmlNode> AppendAttributes(XmlElement element)
    {
      try
      {
        List<XmlNode> attributes = new();

        // Get the document owner
        XmlDocument doc = element.OwnerDocument;

        XmlElement authKeyNode = doc.CreateElement(AUTH_KEY);
        XmlElement authSecretNode = doc.CreateElement(AUTH_SECRET);
        XmlElement authPassphraseNode = doc.CreateElement(AUTH_PASSPHRASE);
        XmlElement authUrlNode = doc.CreateElement(AUTH_URL);
        XmlElement authSocketNode = doc.CreateElement(SOCKET_URL);

        authKeyNode.SetAttribute(VALUE, "YourApiKey");
        authSecretNode.SetAttribute(VALUE, "YourSecret");
        authPassphraseNode.SetAttribute(VALUE, "YourPassphrase");
        authUrlNode.SetAttribute(VALUE, "TheUri");
        authSocketNode.SetAttribute(VALUE, "TheSocketUri");

        element.AppendChild(authKeyNode);
        element.AppendChild(authSecretNode);
        element.AppendChild(authPassphraseNode);
        element.AppendChild(authUrlNode);

        attributes.Add(authKeyNode);
        attributes.Add(authSecretNode);
        attributes.Add(authPassphraseNode);
        attributes.Add(authUrlNode);
        attributes.Add(authSocketNode);

        return attributes;
      }
      catch
      {
        // Throw exception up to notify that the operation was unsuccessful.
        return null;
      }
    }
    #endregion
  }
}
