import { client } from "./discord/client";
import { Environment } from "./environments";
import { fetchBazaar } from "./flipper/bazaarloop";

// TODO: write wrapper for env so you dont have to memorize ENVS
// process.env.DISCORD_PUBLIC_KEY

client.login(Environment.DISCORD_TOKEN);
fetchBazaar();
