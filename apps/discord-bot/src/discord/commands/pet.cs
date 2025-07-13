using Discord;
using Discord.Interactions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public partial class DiscordCommands : InteractionModuleBase
{
    /// <summary>
    /// TODO: cache
    /// </summary>
    /// <param name="targetUser"></param>
    /// <returns></returns>
    [SlashCommand("pet", "pet the kat")]
    public async Task pet([Summary("target", "user to harass")] IUser targetUser)
    {
        try
        {
            int frameCount = 5;
            int sizeImage = 128;
            int sizeFull = 180;
            byte[] imageBytes;
            string saveLocation = "test2.gif";
            string avatar = targetUser.GetAvatarUrl();
            var fetch = await Program.Client.GetAsync(avatar);
            var stream = await fetch.Content.ReadAsStreamAsync();
            var outStream = new FileStream(saveLocation, FileMode.Create);
            Image<Rgba32> gifImage = SixLabors.ImageSharp.Image.Load<Rgba32>(Settings.PATH_PET);

            gifImage.Mutate(x => x.Resize(sizeFull, sizeFull));

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
                    int translateX = sizeFull - sizeImage - 6;
                    int translateY = sizeFull - sizeImage - 2;

                    switch (frame)
                    {
                        case 1:
                            break;
                        case 2:
                            translateX += 2;
                            break;
                        case 3:
                            stretchY = 0.8f;
                            translateX += 2;
                            translateY += (int)(sizeImage * 0.2f);
                            break;
                        case 4:
                            stretchY = 0.5f;
                            translateX += 2;
                            translateY += (int)(sizeImage * 0.5f);
                            break;
                        case 5:
                            stretchY = 0.8f;
                            translateY += (int)(sizeImage * 0.2f);
                            break;
                    }

                    using (Image<Rgba32> frameImage = new(sizeFull, sizeFull))
                    using (Image<Rgba32> avatarImage = SixLabors.ImageSharp.Image.Load<Rgba32>(imageBytes))
                    {
                        avatarImage.Mutate(x => x.Resize((int)(avatarImage.Width * stretchX), (int)(avatarImage.Height * stretchY)));

                        for (int y = 0; y < (int)(sizeImage * stretchY); y++)
                            for (int x = 0; x < (int)(sizeImage * stretchX); x++)
                                frameImage[x + translateX, y + translateY] = avatarImage[x, y];

                        for (int y = 0; y < sizeFull; y++)
                            for (int x = 0; x < sizeFull; x++)
                                if (gifImage.Frames[frame - 1][x, y].A != 0)
                                    frameImage[x, y] = gifImage.Frames[frame - 1][x, y];

                        frameImage.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay = 1;
                        fullGif.Frames.AddFrame(frameImage.Frames.RootFrame);
                    }
                }

                fullGif.Frames.RemoveFrame(0);
                fullGif.SaveAsPng(outStream);
            }

            await RespondWithFileAsync(outStream, $"petthe{targetUser.Username}.gif");
        }

        catch (Exception e)
        {
            Program.Utility.Log(Enums.LogLevel.ERROR, e.ToString());
            await RespondAsync($"Action failed! Ping <@{Settings.ADMIN_1}> for details.");
        }
    }
}