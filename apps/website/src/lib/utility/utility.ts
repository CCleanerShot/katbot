export const utility = {
	capitalize: (input: string): string => {
		return input[0].toUpperCase() + input.slice(1);
	},
	randomNumber: (min: number, max: number): number => {
		return min + Math.round(Math.random() * (max - min));
	},
	sleep: async (milliseconds: number) => {
		return new Promise((res, rej) => setTimeout(() => res(true), milliseconds));
	}
} as const;
