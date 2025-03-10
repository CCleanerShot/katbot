import type { BazaarType } from '$lib/types';

type ModalState = { [key: string]: { isOpened: boolean } | { [key: string]: any } };

export const modalState = $state({
	BazaarAddModal: { type: 'ADD' as BazaarType, isOpened: false as boolean }
} satisfies ModalState);
