import { config } from "dotenv";
import { client } from "./discord/client";
config();

// TODO: write wrapper for env so you dont have to memorize ENVS
// process.env.DISCORD_PUBLIC_KEY

client.login(process.env.DISCORD_TOKEN);
