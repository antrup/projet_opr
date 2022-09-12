using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BugTrackerAPI.Data.Validation
{
    [AttributeUsage(AttributeTargets.Property |
  AttributeTargets.Field, AllowMultiple = false)]

    // Custom validation rules to check if file provided (screenshot) is a genuine jpg and less than 2MB
    public class FileValidation : ValidationAttribute
    {
        // check file signature determined by the first few bytes at the start of a file
        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                using (var reader = new BinaryReader(file.OpenReadStream()))
                {
                    var signatures = _fileSignature[".jpg"];
                    var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                    return signatures.Any(signature =>
                        headerBytes.Take(signature.Length).SequenceEqual(signature));
                }
            }
            return true;
        }

        // dictionnary of signature based on file extension
        private static readonly Dictionary<string, List<byte[]>> _fileSignature =
            new Dictionary<string, List<byte[]>>
            {
                { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                }
                },
            };
    }

    [AttributeUsage(AttributeTargets.Property |
    AttributeTargets.Field, AllowMultiple = false)]
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

