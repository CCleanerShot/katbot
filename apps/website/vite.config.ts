import tailwindcss from '@tailwindcss/vite';
import { svelteTesting } from '@testing-library/svelte/vite';
import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [sveltekit(), tailwindcss()],
	resolve: {
		preserveSymlinks: true
	},
	// // https://stackoverflow.com/questions/68046410/how-to-force-vite-clearing-cache-in-vue3
	// build: {
	// 	rollupOptions: {
	// 		output: {
	// 			entryFileNames: `[name]` + Math.random() + `.js`,
	// 			chunkFileNames: `[name]` + Math.random() + `.js`,
	// 			assetFileNames: `[name]` + Math.random() + `.[ext]`
	// 		}
	// 	}
	// },
	test: {
		workspace: [
			{
				extends: './vite.config.ts',
				plugins: [svelteTesting()],
				test: {
					name: 'client',
					environment: 'jsdom',
					clearMocks: true,
					include: ['src/**/*.svelte.{test,spec}.{js,ts}'],
					exclude: ['src/lib/server/**'],
					setupFiles: ['./vitest-setup-client.ts']
				}
			},
			{
				extends: './vite.config.ts',

				test: {
					name: 'server',
					environment: 'node',
					include: ['src/**/*.{test,spec}.{js,ts}'],
					exclude: ['src/**/*.svelte.{test,spec}.{js,ts}']
				}
			}
		]
	}
});
