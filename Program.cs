using SixLaborsBrokenConversion.Properties;

namespace SixLaborsBrokenConversion;

/// <summary>
///     For version 65 of nightly sixlabors package we've met issue with image conversion
///     We take original image from resources and convert it without options by sixlabors to jpg
///     Result image in brokenImageModel.jpg file can be opened in MS Paint but cannot be
///     edited or saved properly
///     it can cause failures on s3 or cdn storages during upload with
///     "File signature doesn't match" exception
/// </summary>
internal class Program
{
    const string BrokenFilePath = @"./brokenImageModel.jpg";

    static async Task Main(string[] args)
    {
        //get fine image from project resources
        var sixLaborsImage = Image.Load(Resources.imageModel);

        using var ms = new MemoryStream();
        
        // fill ms stream with original image converted to sixlabors jpeg image
        await sixLaborsImage.SaveAsJpegAsync(ms, CancellationToken.None);

        // save image to file
        await using var fileStream = File.Create(BrokenFilePath);
        ms.Seek(0, SeekOrigin.Begin);
        await ms.CopyToAsync(fileStream);

        // try to open and save it with MS Paint
        // ???
        // Failure
    }
}