import type { ArrayType } from '$lib/types';

export const autocompleteState = $state({
	afterAction: (() => ({})) as (input: string) => void,
	element: undefined as HTMLInputElement | undefined,
	initValue: '' as string,
	results: [] as ArrayType,
	updateKey: '' as string,
	updateObj: {} as Record<string, any>
});
