using Discord;
using Discord.Interactions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("pet", "pet the kat")]
    public async Task pet([Summary("target", "user to harass")] IUser targetUser)
    {
        try
        {

            int frameCount = 6;
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

            using (Image<Rgba32> fullGif = new(sizeFull, sizeFull))
            {
                fullGif.Metadata.GetGifMetadata().RepeatCount = 0;
                fullGif.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay = 1;

                for (int frame = 1; frame <= frameCount; frame++)
                {
                    float stretchX = 1.0f;
                    float stretchY = 1.0f;
                    int translateX = 0;
                    int translateY = 0;

                    switch (frame)
                    {
                        case 1:
                            break;
                        case 2:
                            translateX = 2;
                            break;
                        case 3:
                            stretchY = 0.8f;
                            translateX = 2;
                            break;
                        case 4:
                            stretchY = 0.5f;
                            translateX = 2;
                            break;
                        case 5:
                            stretchY = 0.8f;
                            break;
                    }

                    using (Image<Rgba32> frameImage = new(sizeFull, sizeFull))
                    using (Image<Rgba32> avatarImage = SixLabors.ImageSharp.Image.Load<Rgba32>(imageBytes))
                    {
                        avatarImage.Mutate(x => x.Resize((int)(avatarImage.Width * stretchX), (int)(avatarImage.Height * stretchY)));

                        for (int y = 0; y < (int)(sizeImage * stretchY); y++)
                            for (int x = 0; x < (int)(sizeImage * stretchX); x++)
                                frameImage[x + translateX, y + translateY] = avatarImage[x, y];

                        frameImage.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay = 1;
                        fullGif.Frames.AddFrame(frameImage.Frames.RootFrame);
                    }
                }

                fullGif.Frames.RemoveFrame(0);
                GifEncoder encoder = new GifEncoder();
                fullGif.SaveAsGif(outStream);
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

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}