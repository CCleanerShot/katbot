export const utility = {
	capitalize: (input: string): string => {
		return input[0].toUpperCase() + input.slice(1);
	},
	formatNumber: (input: number | bigint): string => {
		const results = utility.reverseString(input.toString()).split(/(.{3})/g);
		return utility.reverseString(results.join(' '));
	},
	randomNumber: (min: number, max: number): number => {
		return min + Math.round(Math.random() * (max - min));
	},
	/** NOTE: returns a new array */
	reverseArray: <T extends any>(input: T[]): T[] => {
		const array = [];

		for (let i = input.length - 1; i >= 0; i--) {
			array.push(input[i]);
		}

		return array;
	},
	reverseString: (input: string): string => {
		const array = [];

		for (let i = input.length - 1; i >= 0; i--) {
			array.push(input[i]);
		}

		return array.join('');
	},
	sleep: async (milliseconds: number) => {
		return new Promise((res, rej) => setTimeout(() => res(true), milliseconds));
	}
} as const;
