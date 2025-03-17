type StatusType = 'error' | 'loading' | 'none';

export const fetchState = $state({ status: 'NONE' as StatusType });
