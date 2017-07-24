using System;
using System.Collections.Generic;

namespace xpf.Http
{
    public class HttpCookie
    {
        public HttpCookie()
        {
        }

        public HttpCookie(string rawCookie)
        {
            var headerParts = rawCookie.Split(';');
            for (int i = 0; i < headerParts.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(headerParts[i]))
                {
                    var name = "";
                    var value = "";
                    if (headerParts[i].Contains("="))
                    {
                        name = headerParts[i].Substring(0, headerParts[i].IndexOf('=')).Trim();
                        value = headerParts[i].Substring(name.Length + 2);
                    }
                    else
                        name = headerParts[i].Trim();

                    if (i == 0)
                    {
                        // The first part is always the name of the cookie
                        this.Name = name;
                        this.Value = value;
                    }
                    else
                    {
                        switch (name)
                        {
                            case "expires":
                                this.Expiry = DateTime.Parse(value);
                                break;
                            case "path":
                                this.Path = value;
                                break;
                            case "domain":
                                this.Domain = value;
                                break;
                            case "secure":
                                this.IsSecure = true;
                                break;
                            case "HttpOnly":
                                this.IsHttpOnly = true;
                                break;
                            case "Version":
                                this.Version = value;
                                break;
                        }
                    }
                }
            }
        }

        public bool IsHttpOnly { get; set; }

        public bool IsSecure { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Version { get; set; }
        public DateTime Expiry { get; set; }

        public bool IsExpired { get { return this.Expiry < DateTime.Now; } }

        public string Domain { get; set; }

        public string Path { get; set; }


    }
}