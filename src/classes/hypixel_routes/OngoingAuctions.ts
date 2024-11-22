import { OngoingAuctionItem } from "./OngoingAuctionItem";

/** /auctions */
export class OngoingAuctions {
	success: boolean;
	page: number;
	totalPages: number;
	totalAuctions: number;
	lastUpdated: number;
	auctions: OngoingAuctionItem[];

	constructor(params: {
		success: boolean;
		page: number;
		totalPages: number;
		totalAuctions: number;
		lastUpdated: number;
		auctions: OngoingAuctionItem[];
	}) {
		this.success = params.success;
		this.page = params.page;
		this.totalPages = params.totalPages;
		this.totalAuctions = params.totalAuctions;
		this.lastUpdated = params.lastUpdated;
		this.auctions = params.auctions;
	}
}
