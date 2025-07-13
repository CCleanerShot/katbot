using Discord;
using Discord.Interactions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("pet", "pet the kat")]
    public async Task pet([Summary("target", "user to harass")] IUser targetUser)
    {
        int sizeImage = 128;
        int sizeFull = 400;
        byte[] imageBytes;
        string saveLocation = "test2.gif";
        string avatar = targetUser.GetAvatarUrl();
        var fetch = await Program.Client.GetAsync(avatar);
        var stream = await fetch.Content.ReadAsStreamAsync();
        var outStream = new FileStream(saveLocation, FileMode.Create);

        using (BinaryReader br = new BinaryReader(stream))
        {
            imageBytes = br.ReadBytes(500000);
            br.Close();
        }

        stream.Close();

        //     metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
        //     metadata.FrameDelay = frameDelay;

        //     // Add the color image to the gif.
        //     gif.Frames.AddFrame(image.Frames.RootFrame);

        // gif.SaveAsGif("output.gif");

        using (Image<Rgba32> fullGif = new(sizeFull, sizeFull))
        {
            for (int frame = 0; frame < 5; frame++)
            {
                int offset = frame * 10;
                using (Image<Rgba32> frameImage = new(sizeFull, sizeFull))
                using (Image<Rgba32> avatarImage = SixLabors.ImageSharp.Image.Load<Rgba32>(imageBytes))
                {
                    for (int y = 0; y < sizeImage; y++)
                    {
                        for (int x = 0; x < sizeImage; x++)
                        {
                            frameImage[x + offset, y + offset] = avatarImage[x, y];
                        }
                    }

                    fullGif.Frames.AddFrame(frameImage.Frames.RootFrame);
                }
            }

            GifEncoder encoder = new GifEncoder();
            fullGif.SaveAsPng(outStream);
        }

        BinaryWriter bw = new BinaryWriter(outStream);

        try
        {
            bw.Write(imageBytes);
        }

        finally
        {
            bw.Close();
        }

        await RespondAsync("okay");
    }
}