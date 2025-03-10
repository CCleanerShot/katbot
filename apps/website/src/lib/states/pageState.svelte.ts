import { page } from '$app/state';
import type { BasePageData } from '$lib/types';

/** this shouldnt be needed, but im not sure how to force re-rendering of related components whenever an anchor is clicked (while also not reloading the page) */
export const pageState = $state({ page: { data: { description: "", title: "" } as BasePageData } });
