import { utility } from '$lib/common/utility';
import { SocketMessageType, type SocketMessage } from '$lib/types';
import { sidebarState } from '$lib/states/sidebarState.svelte';
import { PUBLIC_DOMAIN, PUBLIC_PREFIX_WEBSOCKET } from '$env/static/public';

export class WebsocketService {
	maxRetry: number = 5;
	retries: number = $state(0);
	retrying: boolean = $state(false);
	socket: WebSocket | null = $state(null);
	url: string | URL;
	protocols?: string | string[];

	constructor(protocols?: string | string[]) {
		this.url = new URL(`${utility.getProtocol('ws')}://${PUBLIC_DOMAIN}/${PUBLIC_PREFIX_WEBSOCKET}`);
		this.protocols = protocols;
		this.InitalizeSocket(false);
	}

	InitalizeSocket(retry = true) {
		this.retrying = true;

		if (retry) {
			this.retries += 1;
		}

		const url = new URL(this.url);
		this.socket = new WebSocket(this.url, this.protocols);

		this.socket.addEventListener('open', (e) => {
			this.retrying = false;
		});

		this.socket.addEventListener('error', (e) => {
			console.log('error', e);
		});

		this.socket.addEventListener('close', async (e) => {
			this.socket = null;

			if (this.retries < this.maxRetry) {
				await utility.sleep(2000);
				this.InitalizeSocket();
			} else {
				this.retries = 0;
				this.retrying = false;
			}
		});

		this.socket.addEventListener('message', (e) => {
			const data = JSON.parse(e.data) as SocketMessage;
			console.log('message', data);

			switch (data.type) {
				case SocketMessageType.AUCTIONS:
					sidebarState.SkyblockAlertsSidebar.items.auctionSocketMessages = data.auctionSocketMessages;
					break;
				case SocketMessageType.BAZAAR:
					sidebarState.SkyblockAlertsSidebar.items.bazaarSocketMessagesBuy = data.bazaarSocketMessagesBuy;
					sidebarState.SkyblockAlertsSidebar.items.bazaarSocketMessagesSell = data.bazaarSocketMessagesSell;
					break;
			}
		});
	}
}
