import type { MongoCollection } from "$lib/mongodb/MongoCollection";
import {Collection as MongoDBCollection} from "mongodb"

// See https://svelte.dev/docs/kit/types#app.d.ts
// for information about these interfaces
declare global {
	namespace App {
		// interface Error {}
		// interface Locals {}
		// interface PageData {}
		// interface PageState {}
		// interface Platform {}

	}
}

export {};
