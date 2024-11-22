import nbt from "prismarine-nbt";
import { TextChannel } from "discord.js";
import { readFileSync, writeFile } from "node:fs";
import type { FinishedAuctionItem } from "./classes";

class CustomUtils {
	LogNested(obj: Record<string, any>, current_iter: number, max_iter: number) {
		for (const key in obj) {
			const nestedItem = obj[key];

			if (typeof nestedItem === "object" && current_iter < max_iter) {
				this.LogNested(nestedItem, current_iter + 1, max_iter);
			} else {
				console.log(key, ":", nestedItem);
			}
		}
	}

	async NBTParse(stringValue: string) {
		type NBTValue = { value: any };
		const data = Buffer.from(stringValue, "base64");
		const nestedData = ((await nbt.parse(data)).parsed.value as any)!.i!.value!.value![0];
		return nestedData as { id: NBTValue; Count: NBTValue; tag: NBTValue; Damage: NBTValue };
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
}

export const myUtils = new CustomUtils();
