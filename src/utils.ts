import { TextChannel } from "discord.js";
import { readFileSync, writeFile } from "node:fs";
import type { FinishedAuctionItem } from "./classes";

class CustomUtils {
	/** this is just a helper methof to easier display the methods. kinda annoying to see all the random BS methods on a static class */
	static me: CustomUtils;
	/**
	 * Helper function to sleep.
	 */
	Sleep(milliseconds: number): Promise<void> {
		return new Promise((res, rej) => {
			setTimeout(() => res(), milliseconds);
		});
	}

	WriteAuctionData(data: FinishedAuctionItem[]) {
		writeFile("data.txt", JSON.stringify(data), (err) => {
			if (err) {
				console.log("error writing", err);
			}
		});
	}

	ReadAuctionData(): FinishedAuctionItem[] {
		const buffer = readFileSync("data.txt");
		const stringResult = buffer.toString();

		if (stringResult === "") {
			return [];
		}

		const json = JSON.parse(stringResult);
		return json.data as FinishedAuctionItem[];
	}

	/** Hypixel gives some text special colors (created with a group of special characters). This helps remove them */
	RemoveSpecialText(text: string): string {
		let result = text;

		result = text.replace(/§.[a]?/g, "");
		return result;
	}

	async SendBulkText(textChannel: TextChannel, message: string) {
		const chunks = message.match(/[\s\S]{1,2000}/g) || [];

		for (const chunk of chunks) {
			await textChannel.send(chunk);
		}
	}
}

export const utils = new CustomUtils();
