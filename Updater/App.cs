﻿using System.Collections.Generic;
using System.IO;

namespace Updater
{
    public enum Environment
    {
        development = 0,
        staging = 1,
        production = 2
    }

    public static class App
    {
        public static Environment Environment { get; set; } = Environment.development;
        public static bool IsDocker { get; set; }
        public static Models.Config Config { get; set; } = new Models.Config();
        private static string _rootPath { get; set; } = "";

        public static string RootPath
        {
            get
            {
                if (string.IsNullOrEmpty(_rootPath))
                {
                    _rootPath = Path.GetFullPath(".").Replace("\\", "/");
                }
                return _rootPath;
            }
        }

        public static string MapPath(string path = "")
        {
            path = path.Replace("\\", "/");
            if (path.Substring(0, 1) == "/") { path = path[1..]; } //remove slash at beginning of string
            if (IsDocker)
            {
                return Path.Combine(RootPath, path).Replace("\\", "/");
            }
            else
            {
                return Path.Combine(RootPath.Replace("/", "\\"), path.Replace("/", "\\"));
            }
        }
    }
}
