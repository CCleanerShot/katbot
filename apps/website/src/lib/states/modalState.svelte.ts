import type { API_CONTRACTS } from '$lib/other/apiContracts';
import type { BazaarType } from '$lib/types';

type ModalState = { [key: string]: { isOpened: boolean } | { [key: string]: any } };

export const modalState = $state({
	BazaarAddModal: {
		action: 'POST=>/api/bazaar/buy' as Extract<keyof typeof API_CONTRACTS, `POST${string}/bazaar/${string}`>,
		type: 'ADD' as BazaarType,
		isOpened: false as boolean
	}
} satisfies ModalState);
