namespace Treinou.Domain.ValueObjects
{
    public class ProfilePicture : SeedWork.ValueObject
    {
        public string Path { get; set; }

        public ProfilePicture(string path)
            => Path = path;
        
    }
}
