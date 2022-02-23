using System;
using System.IO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Files
{
    public class FileAccessor : IFileAccessor
    {
        public bool SaveFile()
        {
            throw new System.NotImplementedException();
        }

        public void GetFile()
        {
            throw new NotImplementedException();
        }
    }
}