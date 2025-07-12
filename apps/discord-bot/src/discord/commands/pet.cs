using Discord;
using Discord.Interactions;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ZstdSharp.Unsafe;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("pet", "pet the kat")]
    public async Task pet([Summary("target", "user to harass")] IUser targetUser)
    {
        int size = 100;
        byte[] imageBytes;
        string saveLocation = "test2.png";
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

        using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(imageBytes))
        {
            image.Mutate(x => x.Resize(size, size));
            image.Save(outStream, new PngEncoder());
        }

        using (SixLabors.ImageSharp.Image<Rgba32> fullImage = new(400, 400))
        {
            using (SixLabors.ImageSharp.Image avatarImage = SixLabors.ImageSharp.Image.Load(imageBytes))
            {

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        // fullImage[x, y] = avatarImage.;

                    }

                }

                fullImage[0, 0] = Rgba32.ParseHex("#FFFFFF");
                avatarImage.Save(outStream, new PngEncoder());
            }
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

        Embed embed = new EmbedBuilder()
            .WithColor(Color.Blue)
            .WithTitle("brazillian goes to the store with $1")
            .WithImageUrl(Settings.PUBLIC_PATH_RAMOJUSD_GIF_URL)
            .Build();

        await RespondAsync("okay");
    }
}