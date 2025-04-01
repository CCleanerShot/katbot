import type { AuctionBuy } from '$lib/mongodb/collections/AuctionBuy';
import type { BazaarBuy } from '$lib/mongodb/collections/BazaarBuy';
import type { BazaarSell } from '$lib/mongodb/collections/BazaarSell';
import { sidebarState } from '$lib/states/sidebarState.svelte';
import type { SocketMessage } from '$lib/types';
import { utility } from '$lib/utility/utility';

export class WebsocketService {
	maxRetry: number = 5;
	retries: number = $state(0);
	retrying: boolean = $state(false);
	socket: WebSocket | null = $state(null);
	url: string | URL;
	protocols?: string | string[];

	constructor(url: string | URL, protocols?: string | string[]) {
		this.url = url;
		this.protocols = protocols;
		this.InitalizeSocket(false);
	}

	emit() {
		const data = { test: 'asda' };

		if (this.socket === null) {
			return;
		}

		this.socket.send(JSON.stringify(data));
	}

	InitalizeSocket(retry = true) {
		this.retrying = true;

		if (retry) {
			this.retries += 1;
		}

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

			sidebarState.SkyblockAlertsSidebar.items.auctionItems.push(...data.auctionItems);
			sidebarState.SkyblockAlertsSidebar.items.bazaarBuys.push(...data.bazaarBuys);
			sidebarState.SkyblockAlertsSidebar.items.bazaarSells.push(...data.bazaarSells);
		});
	}
}
