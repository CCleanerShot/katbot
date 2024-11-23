import { readFileSync, writeFileSync } from "node:fs";

type ConfigData = {
	NUMBER_OF_FETCHED_PAGES: number;
	MINIMUM_PRICE_FOR_SALE: number;
	MINIMUM_MINUTES_FOR_SALE: number;
};
class Config {
	data: ConfigData;

	constructor() {
		const result = readFileSync("config.json", "utf8");
		const json = JSON.parse(result) as ConfigData;
		this.data = json;
	}

	SaveConfig() {
		writeFileSync("config.json", JSON.stringify(this.data, null, 4));
	}
}

export const myConfig = new Config();
