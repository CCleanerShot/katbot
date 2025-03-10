export const utility = {
	capitalize: (input: string) => {
		return input[0].toUpperCase() + input.slice(1);
	}
} as const;
