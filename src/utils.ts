import nbt from "prismarine-nbt";
import { TextChannel } from "discord.js";
import { readFileSync, writeFile } from "node:fs";
import type { FinishedAuctionItem } from "./classes";

class CustomUtils {
	Array2D(y: number, x: number) {
		return Array(y).fill(Array(x).fill(null));
	}

	/** fills an entire 2D array using its actual index */
	Array2DFill<T, K extends T[][]>(array: K, fill_function: (i: number) => T): K {
		let index = 0;

		for (let row = 0; row < array.length; row++) {
			for (let col = 0; col < array[col].length; col++) {
				index++;
				array[row][col] = fill_function(index);
			}
		}

		return array;
	}

	FormatPrice(price: number): string {
		let multiple = 1;
		const result = [];
		const sPrice = price.toString();

		for (let i = sPrice.length - 1; i >= 0; i--) {
			multiple++;
			const letter = sPrice[i];
			result.unshift(letter);

			if (multiple % 4 === 0) {
				multiple = 1;
				result.unshift(" ");
			}
		}

		return result.join("").trim();
	}

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

	/** send bulk text */
	async SendBulkText(textChannel: TextChannel, message: string) {
		const chunks = message.match(/[\s\S]{1,1900}\n/g) || [];

		for (const chunk of chunks) {
			await textChannel.send(chunk);
		}
	}

	/** send bulk text as code blocks */
	async SendBulkTextCode(textChannel: TextChannel, message: string) {
		const chunks = message.match(/[\s\S]{1,1900}\n/g) || [];

		for (const chunk of chunks) {
			const result = "```" + chunk + "```";
			await textChannel.send(result);
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

	/** returns the string back with a number of leading spaces based on the maxValue */
	SpaceText(maxValue: number, currentString: string): string {
		const spaces = Array(Math.max(maxValue - currentString.length, 0))
			.fill(" ")
			.join("");

		return spaces + currentString;
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
