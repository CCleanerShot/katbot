using System.Reflection;
using Cyotek.Data.Nbt;

public class AuctionsRouteProductNBT
{
    static NbtDocument document = new NbtDocument();
    public static Dictionary<string, Tag> Tags = new Dictionary<string, Tag>();

    #region BASE
    public TagCompound BASE = default!;
    public TagByte COUNT = default!;
    public TagShort DAMAGE = default!;
    public TagShort FAKE_ID = default!;
    public TagCompound ROOT = default!;
    public TagCompound TAG = default!;
    #endregion

    #region /tag
    public TagCompound DISPLAY = default!;
    public TagCompound EXTRA_ATTRIBUTES = default!;
    public TagInt HIDE_FLAGS = default!;
    public TagCompound? SKULL_OWNER = default!;
    #endregion

    #region /tag/display
    public TagList LORE = default!;
    public TagString NAME = default!;
    public TagInt? COLOR = default!;
    #endregion

    #region /tag/ExtraAttributes
    public TagString ID = default!; // somehow the only non-optional tag

    public Dictionary<string, Tag?> ExtraTags = new Dictionary<string, Tag?>()
    {
        { "ability_scroll", null },
        { "additional_coins", null },
        { "ammo", null },
        { "anvil_uses", null },
        { "art_of_war_count", null },
        { "artOfPeaceApplied", null },
        { "attributes", null },
        { "auction", null },
        { "backpack_color", null },
        { "baseStatBoostPercentage", null },
        { "bass_weight", null },
        { "bid", null },
        { "blaze_consumer", null },
        { "blazetekk_channel", null },
        { "blocks_walked", null },
        { "blocksBroken", null },
        { "blood_god_kills", null },
        { "bookshelf_loops", null },
        { "bookworm_books", null },
        { "boss_tier", null },
        { "bossId", null },
        { "bottle_of_jyrre_last_update", null },
        { "bottle_of_jyrre_seconds", null },
        { "bow_kills", null },
        { "builder's_ruler_data", null },
        { "builder's_wand_data", null },
        { "cake_owner", null },
        { "century_year", null },
        { "champion_combat_xp", null },
        { "charges_used", null },
        { "chimera_found", null },
        { "collected_coins", null },
        { "color", null },
        { "compact_blocks", null },
        { "date", null },
        { "divan_powder_coating", null },
        { "drill_fuel", null },
        { "drill_part_engine", null },
        { "drill_part_fuel_tank", null },
        { "drill_part_upgrade_module", null },
        { "dungeon_item", null },
        { "dungeon_item_level", null },
        { "dungeon_paper_id", null },
        { "dungeon_potion", null },
        { "dungeon_skill_req", null },
        { "dye_donated", null },
        { "dye_item", null },
        { "edition", null },
        { "effects", null },
        { "eman_kills", null },
        { "enchantments", null },
        { "enhanced", null },
        { "ethermerge", null },
        { "EXE", null },
        { "expertise_kills", null },
        { "extended", null },
        { "farmed_cultivating", null },
        { "farming_for_dummies_count", null },
        { "favorite_ancient_gdrag", null },
        { "favorite_crop", null },
        { "favorite_evil_skin", null },
        { "favorite_gemstone", null },
        { "favorite_shark", null },
        { "favorite_snake", null },
        { "fh_selected_skin", null },
        { "fishes_caught", null },
        { "fungi_cutter_mode", null },
        { "gems", null },
        { "gemstone_gauntlet_meter", null },
        { "gemstone_slots", null },
        { "ghast_blaster", null },
        { "gilded_gifted_coins", null },
        { "glowing", null },
        { "handles_found", null },
        { "hecatomb_s_runs", null },
        { "historic_dungeon_score", null },
        { "hot_potato_count", null },
        { "hotPotatoBonus", null },
        { "intelligence_earned", null },
        { "is_shiny", null },
        { "item_tier", null },
        { "jalapeno_count", null },
        { "lastForceEvolvedTime", null },
        { "lava_creatures_killed", null },
        { "leaderPosition", null },
        { "leaderVotes", null },
        { "levels_found", null },
        { "magma_cube_absorber", null },
        { "magmaCubesKilled", null },
        { "mana_disintegrator_count", null },
        { "maxed_stats", null },
        { "mined_crops", null },
        { "model", null },
        { "modifier", null },
        { "mousemat_pitch", null },
        { "mousemat_yaw", null },
        { "names_found", null },
        { "necromancer_souls", null },
        { "new_year_cake_bag_data", null },
        { "new_years_cake", null },
        { "originTag", null },
        { "pandora-rarity", null },
        { "party_hat_color", null },
        { "party_hat_emoji", null },
        { "party_hat_year", null },
        { "pelts_earned", null },
        { "PERSONAL_DELETOR_ACTIVE", null },
        { "pet_exp", null },
        { "petInfo", null },
        { "pickonimbus_durability", null },
        { "player", null },
        { "players_clicked", null },
        { "polarvoid", null },
        { "post_card_minion_tier", null },
        { "post_card_minion_type", null },
        { "potion", null },
        { "potion_level", null },
        { "potion_name", null },
        { "potion_type", null },
        { "power_ability_scroll", null },
        { "price", null },
        { "promising_pickaxe_breaks", null },
        { "puzzles_solved", null },
        { "raffle_win", null },
        { "raffle_year", null },
        { "raider_kills", null },
        { "ranchers_speed", null },
        { "rarity_upgrades", null },
        { "recipient_id", null },
        { "recipient_name", null },
        { "recipient_team", null },
        { "rift_discrite_seconds", null },
        { "rift_transferred", null },
        { "runes", null },
        { "runic_kills", null },
        { "seconds_held", null },
        { "shop_dungeon_floor_completion_required", null },
        { "skin", null },
        { "soul_durability", null },
        { "spawnedFor", null },
        { "spider_kills", null },
        { "splash", null },
        { "spray", null },
        { "stats_book", null },
        { "stored_drill_fuel", null },
        { "sword_kills", null },
        { "talisman_enrichment", null },
        { "td_attune_mode", null },
        { "thunder_charge", null },
        { "tickets", null },
        { "timestamp", null },
        { "toxophilite_combat_xp", null },
        { "trainingWeightsHeldTime", null },
        { "trapsDefused", null },
        { "tuned_transmission", null },
        { "tuning_fork_tuning", null },
        { "ultimateSoulEaterData", null },
        { "upgrade_level", null },
        { "uuid", null },
        { "WAI", null },
        { "winning_bid", null },
        { "winning_team", null },
        { "wood_singularity_count", null },
        { "year", null },
        { "yearObtained", null },
        { "ZEE", null },
        { "zombie_kills", null },
    };
    #endregion

    public AuctionsRouteProductNBT(string item_bytes)
    {
        byte[] bytes = Convert.FromBase64String(item_bytes);
        document.Load(new MemoryStream(bytes));

        ROOT = document.DocumentRoot;
        BASE = ((ROOT.Value[0] as TagList)!.Value[0] as TagCompound)!;
        FAKE_ID = (BASE.Value["id"] as TagShort)!;
        COUNT = (BASE.Value["Count"] as TagByte)!;
        TAG = (BASE.Value["tag"] as TagCompound)!;
        DAMAGE = (BASE.Value["Damage"] as TagShort)!;

        // /tag
        EXTRA_ATTRIBUTES = (TAG.Value["ExtraAttributes"] as TagCompound)!;
        DISPLAY = (TAG.Value["display"] as TagCompound)!;
        HIDE_FLAGS = (TAG.Value["HideFlags"] as TagInt)!;

        if (TAG.Value.Contains("SkullOwner"))
            SKULL_OWNER = TAG.Value["SkullOwner"] as TagCompound;

        // /tag/display
        NAME = (DISPLAY.Value["Name"] as TagString)!;
        LORE = (DISPLAY.Value["Lore"] as TagList)!;

        if (DISPLAY.Contains("color"))
            COLOR = (DISPLAY.Value["color"] as TagInt)!;

        // /tag/ExtraAttributes
        ID = (EXTRA_ATTRIBUTES.Value["id"] as TagString)!;

        foreach (Tag tag in EXTRA_ATTRIBUTES.Value)
        {
            ExtraTags[tag.Name] = tag;

            if (!Tags.ContainsKey(tag.Name))
                Tags.Add(tag.Name, tag);
        }
    }
}