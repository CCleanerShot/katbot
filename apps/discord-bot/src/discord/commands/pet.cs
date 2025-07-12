using Discord;
using Discord.Interactions;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public partial class DiscordCommands : InteractionModuleBase
{
    [SlashCommand("pet", "pet the kat")]
    public async Task pet([Summary("target", "user to harass")] IUser targetUser)
    {
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
            image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
            image.Save(outStream, new PngEncoder());
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