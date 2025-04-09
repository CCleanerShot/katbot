export const utility = {
	capitalize: (input: string): string => {
		return input[0].toUpperCase() + input.slice(1);
	},
	formatNumber: (input: number | bigint): string => {
		const results = utility.reverse(input.toString()).split(/(.{3})/g);

		console.log(results, utility.reverse(input.toString()), input);
		return utility.reverse(results.join(' '));
	},
	randomNumber: (min: number, max: number): number => {
		return min + Math.round(Math.random() * (max - min));
	},
	reverse: (input: string): string => {
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
