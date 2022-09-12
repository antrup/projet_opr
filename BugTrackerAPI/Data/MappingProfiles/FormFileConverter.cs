using AutoMapper;

namespace BugTrackerAPI.Data.MappingProfiles
{
    // Recipe to convert an IformFile to an array of byte
    public class FormFileConverter : ITypeConverter<IFormFile, byte[]>
    {
        public byte[] Convert(IFormFile source, byte[] destination, ResolutionContext context)
        {
            var bytesArray = Array.Empty<byte>();

            if (source != null && source.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    source.CopyTo(ms);
                    bytesArray = ms.ToArray();
                }
            }
            return bytesArray;
        }
    }
}
