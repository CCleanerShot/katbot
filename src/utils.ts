import { readFileSync, writeFile } from "fs";
import { FinishedAuctionItem } from "./classes";

class CustomUtils {

	/** this is just a helper methof to easier display the methods. kinda annoying to see all the random BS methods on a static class */
	static me: CustomUtils
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
		} else {
			const json = JSON.parse(stringResult);
			return json.data as FinishedAuctionItem[];
		}
	}

	/** Hypixel gives some text special colors (created with a group of special characters). This helps remove them */
	RemoveSpecialText(text: string): string {
		let result = text;

		result = text.replace(/§.[a]?/g, "")
		return result
	}
}

export const utils = new CustomUtils();
