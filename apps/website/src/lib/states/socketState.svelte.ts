import type { WebsocketService } from '$lib/classes/WebsocketService.svelte';

export const socketState = $state({ socketService: null as WebsocketService | null });
