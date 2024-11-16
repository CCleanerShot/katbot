import { readFileSync, writeFile } from "fs";
import { RequestTypes } from "./types";

export class CustomUtils {
	/**
	 * Helper function to sleep.
	 */
	static Sleep(milliseconds: number): Promise<void> {
		return new Promise((res, rej) => {
			setTimeout(() => res(), milliseconds);
		});
	}

	static WriteAuctionData(data: RequestTypes.AuctionItem[]) {
		writeFile("data.txt", JSON.stringify(data), (err) => {
			if (err) {
				console.log("error writing", err);
			}
		});
	}

	static ReadAuctionData(): RequestTypes.AuctionItem[] {
		const buffer = readFileSync("data.txt");
		const stringResult = buffer.toString();

		if (stringResult === "") {
			return [];
		} else {
			const json = JSON.parse(stringResult);
			return json.data as RequestTypes.AuctionItem[];
		}
	}
}
