import { GetPlayerItem } from "./GetPlayerItem";

export class GetPlayerResponse {
	success: boolean;
	player: GetPlayerItem;

	constructor(params: {
		success: boolean;
		player: GetPlayerItem;
	}) {
		this.success = params.success;
		this.player = params.player;
	}
}
