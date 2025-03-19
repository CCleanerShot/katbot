import type { API_CONTRACTS } from '$lib/other/apiContracts';
import type { BazaarType } from '$lib/types';

type ModalState = { [key: string]: { isOpened: boolean } | { [key: string]: any } };

export const modalState = $state({
	none: {
		isOpened: false as boolean
	}
} satisfies ModalState);
