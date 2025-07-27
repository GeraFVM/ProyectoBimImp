using System;
using System.IO;

namespace Application.Services
{
    public static class FileConverterService
    {
        public static string ConvertToBase64(Stream fileStream)
        {
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream), "El flujo del archivo no puede ser nulo.");
            }

            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}
