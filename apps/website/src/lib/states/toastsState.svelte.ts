import type { ToastProps } from '$lib/types';
import { utility } from '$lib/utility/utility';

export const toastsState = $state([] as ToastProps[]);

export const toastActions = {
	addToast: ({ message, type }: Omit<ToastProps, 'id'>) => {
		toastsState.push({ id: utility.randomNumber(0, 1000000000), message, type });
	}
} as const;
